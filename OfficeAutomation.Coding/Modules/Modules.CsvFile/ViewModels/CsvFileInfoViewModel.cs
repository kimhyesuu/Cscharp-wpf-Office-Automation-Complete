using Modules.CsvFile.Convert;
using OfficeAutomation.Coding.Business.Managers;
using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using OfficeAutomation.Coding.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.CsvFile.ViewModels
{
	public class CsvFileInfoViewModel : BindableBase
	{
		private readonly IClassService<ClassDetailInfoModel> _classDetailInfoService;
		private readonly IClassService<ClassInfoModel>		  _classInfoService;
		private readonly ISettingTypeService					  _settingTypeService;
		private readonly IEventAggregator						  _eventAggregator;

		private ObservableCollection<ClassInfoModel>       _classInfos;
		public  ObservableCollection<ClassInfoModel>       ClassInfos
		{
			get => _classInfos;
			set => SetProperty(ref _classInfos, value);
		}

		private ObservableCollection<ClassDetailInfoModel> _classDetailInfos;
		public  ObservableCollection<ClassDetailInfoModel> ClassDetailInfos
		{
			get => _classDetailInfos;
			set => SetProperty(ref _classDetailInfos, value);
		}

		private string _newDataType;
		public  string NewDataType
		{
			get => _newDataType; 
			set => SetProperty(ref _newDataType, value); 
		}

		private string												 SelectedClassName				{ get;		 set;  }  // CreateClassDetailInfos : Receive, SendCodingText : Using

		public  ObservableCollection<string>				 ClassAccessModifiers         { get;				 }
		public  ObservableCollection<string>			    AccessModifiers					{ get;		  	    }
		public  ObservableCollection<string>			    MemberDataTypes				   { get;		  	    } 
		public  ObservableCollection<string>			    MemberTypes					   { get;		  		 } 
		public  ObservableCollection<string>			    ClassTypes						   { get;		    	 }


		public  System.Windows.Threading.DispatcherTimer checkReadedFileTimer		   { get;		  set; }
		private string												 BaseClassName					   { get;		  set; }
		private object												 _lockObject;
		
		public  DelegateCommand<object>						 CreateCommand						{ get; private set; }
		public  DelegateCommand									 SendPriviewTextCommand			{ get; private set; }
		public  DelegateCommand								    AddDataTypeCommand				{ get; private set; }
		public  DelegateCommand									 ConfirmCommand					{ get; private set; }

		public CsvFileInfoViewModel(IEventAggregator eventAggregator)
		{			
			_eventAggregator					= eventAggregator;
			_classDetailInfoService			= new ClassDetailInfoService();
			_classInfoService					= new ClassInfoService();
			_settingTypeService				= new SettingTypeService();

			ClassAccessModifiers				= new ObservableCollection<string>();
			AccessModifiers					= new ObservableCollection<string>();
			MemberDataTypes				   = new ObservableCollection<string>();
			MemberTypes						   = new ObservableCollection<string>();
			ClassTypes						   = new ObservableCollection<string>();

			Initialize();
		}

		private void   CheckReadedFileTimerTick(object sender, EventArgs e)
		{
			lock(_lockObject)
			{
				if (_classDetailInfoService.GetCount() > 0)
				{
					_classInfos.Clear();
					var ReceivedClassNames = _classDetailInfoService.GetAll().Select(classDetailInfo => classDetailInfo.ClassName).Distinct();

					foreach (var ReceivedClassName in ReceivedClassNames)
					{
						if(IsBaseClass(ReceivedClassName) is true)
						{
							BaseClassName = ReceivedClassName;
						}

						_classInfos.Add(new ClassInfoModel
						{
							AccessModifier = "public"			,
							ClassType		= Constants.ClassTypeDefault,
							ClassName	   = ReceivedClassName
						});
					}

					checkReadedFileTimer.Stop();
				}
			}		
		}

		private void   OnclassInfosChanged(object sender, NotifyCollectionChangedEventArgs e)
			=> SequencingService.SetCollectionSequence(this._classInfos);

		private void   CreateClassDetailInfos(object classInfo)
		{
			var Receivedinfo = classInfo as ClassInfoModel;
			var emptyOrError = IsCreatingCompability(Receivedinfo);
			if (emptyOrError != string.Empty)
			{
				SendErrorLogging(emptyOrError);
				return;
			}
			
			SendErrorLogging(emptyOrError);

			var className		      = Receivedinfo.ClassName;
			SelectedClassName       = className;
			var allClassDetailInfos = _classDetailInfoService.GetAll();
			var selectedclassDetailInfos = allClassDetailInfos.Where(classDetailInfo => classDetailInfo.ClassName.Equals(className));
			if (_classDetailInfos.Count > 0) _classDetailInfos.Clear();

			foreach (var classDetailInfo in selectedclassDetailInfos)
			{
				_classDetailInfos.Add(classDetailInfo);
			}
		}

		private void   SendPriviewText()
		{
			var textResult              = string.Empty;
			var errorLogs					 = new List<string>();
			var convertingData          = new ConvertTo();
			var selectedClassInfo       = ClassInfos.Where (classInfo => classInfo.ClassName.Equals(SelectedClassName)).FirstOrDefault();
			var baseClassInfo				 = _classInfos.Where(o			=> o.ClassName.Equals(BaseClassName)				 ).FirstOrDefault();
			var detailedInfos				 = _classDetailInfoService.GetAll().ToList();
			
			CheckNewItems(ref detailedInfos);
		
			if (selectedClassInfo is null || detailedInfos.Count() == 0) return;

			SendErrorLogging(string.Empty);

			errorLogs = GetErrorLogs(convertingData,baseClassInfo, selectedClassInfo, detailedInfos);

			if (IsCompability(errorLogs) is true && errorLogs != null)
			{
				textResult = convertingData.Result();
				_eventAggregator.GetEvent<SendPreviewMessage>().Publish(textResult);	
			}
			convertingData.Reset();
		}

		private void CheckNewItems(ref List<ClassDetailInfoModel> detailedInfos)
		{
			var checkNewClassDetailInfos = _classDetailInfos;
			var selectedDetailedInfos = detailedInfos.Where(classInfo => classInfo.ClassName.Equals(SelectedClassName)).ToList();

			if (checkNewClassDetailInfos.Count != selectedDetailedInfos.Count)
			{
				for (int index = selectedDetailedInfos.Count; index < checkNewClassDetailInfos.Count; index++)
				{
					if (_classDetailInfoService.CanDoAdd(checkNewClassDetailInfos[index]))
					{
						detailedInfos.Add(checkNewClassDetailInfos[index]);
					}
				}
			}
		}

		private async void   SendResults()
		{
			var convertingData	 = new ConvertTo();
			var errorLogs			 = new List<string>();
			var resultlist			 = new List<object>();
			var textResult			 = string.Empty;
			var baseClassInfo		 = _classInfos.Where(o => o.ClassName.Equals(BaseClassName)).FirstOrDefault();
			var detailedInfos		 = _classDetailInfoService.GetAll().ToList();

			CheckNewItems(ref detailedInfos);

			if (ClassDetailInfos.Count == 0 || ClassInfos.Count == 0)
			{
				Message.InfoOKMessage(Message.ClassDetailInfosNotData);
				return;
			}

			SendErrorLogging(string.Empty);

		
				foreach (var classInfo in ClassInfos)
				{
					var reuslt = Task.Run(() => GetErrorLogs(convertingData, baseClassInfo, classInfo, detailedInfos));
					errorLogs = await reuslt;

					if (IsCompability(errorLogs) is true && errorLogs != null)
					{
						textResult = convertingData.Result();
						resultlist.Add(new { className = classInfo.ClassName, text = textResult });
						convertingData.Reset();
					}
					else
					{
						convertingData.Reset();
						return;
					}
				}

			ClearData();			
			_eventAggregator.GetEvent<SendSavingMessages>().Publish(resultlist);
		}

		private List<string> GetErrorLogs(ConvertTo								  convertingData	 , 
													 ClassInfoModel						  baseClassInfo	 , 
													 ClassInfoModel						  selectedClassInfo,
													 IEnumerable<ClassDetailInfoModel> detailedInfos	  )
		{
			var errorLogs = new List<string>();

			var emptyOrError = convertingData.Initialize(baseClassInfo, selectedClassInfo, detailedInfos);
			if (emptyOrError != string.Empty)
			{
				SendErrorLogging(emptyOrError);
				return null;
			}

			errorLogs.Add(convertingData.StartText()	    );
			errorLogs.Add(convertingData.ConstantsText()  );
			errorLogs.Add(convertingData.FieldsText()     );
			errorLogs.Add(convertingData.PropertiesText() );
			errorLogs.Add(convertingData.ConstructorText());
			errorLogs.Add(convertingData.MethodsText()    );
			convertingData.EndText();

			return errorLogs;
		}

		private void   AddDatatype()
		{
			var addingNewtype = _newDataType;
			if (IsDatatypeCompability(addingNewtype) is false) return;
			
			MemberDataTypes.Add(addingNewtype);
			NewDataType = string.Empty;
		}

		private void   SendErrorLogging(string errorLog)
		{
			_eventAggregator.GetEvent<SendLog>().Publish(errorLog);
		}

		private bool   IsCompability(List<string> errorLogs)
		{
			foreach (var errorLog in errorLogs)
			{
				if (errorLog != string.Empty)
				{
					SendErrorLogging(errorLog);
					return false;
				}
			}
			return true;
		}

		private string IsCreatingCompability(ClassInfoModel receivedinfo)
		{
			if (receivedinfo is null) return Message.ClassDetailInfosNotData;

			var result = string.Compare(receivedinfo.AccessModifier, "public"  , true) == 0 ||
							 string.Compare(receivedinfo.AccessModifier, "internal", true) == 0 ? true : false;

			if(result is false) return Message.WrongAccessModifier(receivedinfo.AccessModifier);

			return string.Empty;
		}

		private bool   IsDatatypeCompability(string addingNewtype)
		{
			if (string.IsNullOrWhiteSpace(addingNewtype) ||
				 ClassDetailInfos.Count == 0)
			{
				SendErrorLogging(Message.ClassDetailInfosNoExistOrNewTypeIsEmpty);
				return false;
			}
			else if (IsSpecialText(addingNewtype) is true)
			{
				SendErrorLogging(Message.DeleteSpecialText);
				return false;
			}
			else if (IsDuplicatedDataType(addingNewtype) is true)
			{
				SendErrorLogging(Message.AddingDataTypeDuplicated);
				return false;
			}

			return true;
		}

		private bool   IsSpecialText(string addingNewtype)
		{
			string str = @"[~!@\#$%^&*\()\=+|\\/:;?""']";
			var	 rex = new System.Text.RegularExpressions.Regex(str);
			return rex.IsMatch(addingNewtype);
		}

		private bool   IsDuplicatedDataType(string addingNewtype)
		{
			var doublicates = MemberDataTypes.Where(dataType => dataType.Equals(addingNewtype)).Count();

			return doublicates >= 1 ? true : false;
		}

		private bool   IsBaseClass(string receivedClassName)
		{
			var charList = receivedClassName.ToLower().ToCharArray();

			for (int i = 0; i < charList.Length; i++)
			{
				if (charList[i].Equals('b'))
				{
					var result = receivedClassName.Substring(i, 4).ToLower();
					if (result.Equals("base") == true)
					{
						return true;
					}
					break;
				}
			}
			return false;
		}

		private void   ClearData()
		{
			ClassInfos.Clear();
			_classInfos.Clear();
			_classDetailInfoService.Clear();
			ClassDetailInfos.Clear();
		}

		private void   Initialize()
		{
			InitializeCommand();

			InitializeData();

			InitializeTimer();
		}

		#region 종류에 맞는 Initialize로 구성

		private void   InitializeData()
		{
			var accessModifiers = _settingTypeService.GetAccessModifiers();
			var memberDataTypes = _settingTypeService.GetDataTypes();
			var memberTypes     = _settingTypeService.GetMemberTypes();
			var classTypes      = _settingTypeService.GetClassTypes();

			foreach (var accessModifier in accessModifiers)
			{
				AccessModifiers.Add(accessModifier);
			}

			foreach (var accessModifier in accessModifiers)
			{
				if(string.Compare(accessModifier, "internal", true) == 0 || string.Compare(accessModifier, "public", true) == 0)
					ClassAccessModifiers.Add(accessModifier);
			}

			foreach (var memberDataType in memberDataTypes)
			{
				MemberDataTypes.Add(memberDataType);
			}

			foreach (var memberType		 in memberTypes)
			{
				MemberTypes.Add(memberType);
			}

			foreach (var classType		 in classTypes)
			{
				ClassTypes.Add(classType);
			}

			_classInfos							 = new ObservableCollection<ClassInfoModel>();
			_classDetailInfos					 = new ObservableCollection<ClassDetailInfoModel>();
			_classInfos.CollectionChanged += OnclassInfosChanged;
		}

		private void   InitializeCommand()
		{
			CreateCommand						= new DelegateCommand<object>(CreateClassDetailInfos);
			SendPriviewTextCommand			= new DelegateCommand(SendPriviewText);
			AddDataTypeCommand				= new DelegateCommand(AddDatatype);
			ConfirmCommand						= new DelegateCommand(SendResults);
		}
						   
		private void   InitializeTimer()
		{
			checkReadedFileTimer			   = TimerManager.CheckReadedCsvFileTimer;
			checkReadedFileTimer.Tick     += CheckReadedFileTimerTick;
			checkReadedFileTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
			_lockObject						   = new object();
		}

		#endregion
	}
}
