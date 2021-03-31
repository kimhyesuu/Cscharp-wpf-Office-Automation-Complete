using Modules.CsvFile.Convert;
using OfficeAutomation.Coding.Business.Managers;
using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using OfficeAutomation.Coding.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Modules.CsvFile.ViewModels
{
	public class CsvFileInfoViewModel : BindableBase
	{
		private readonly IClassService<ClassDetailInfoModel> _classDetailInfoService;
		private readonly IClassService<ClassInfoModel>		  _classInfoService;
		private readonly ISettingTypeService					  _settingTypeService;
		private readonly IEventAggregator						  _eventAggregator;

		private ObservableCollection<ClassInfoModel> _classInfos;
		public  ObservableCollection<ClassInfoModel> ClassInfos
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
			get { return _newDataType; }
			set { SetProperty(ref _newDataType, value); }
		}

		private string												 SelectedClassName				{ get;			set; }  // CreateClassDetailInfos : Receive, SendCodingText : Using

		public  ObservableCollection<string>			    AccessModifiers					{ get;		  	     }
		public  ObservableCollection<string>			    MemberDataTypes				   { get;		  	     } 
		public  ObservableCollection<string>			    MemberTypes					   { get;		  		  } 
		public  ObservableCollection<string>			    ClassTypes						   { get;		    	  } 
																											   				  
		public  System.Windows.Threading.DispatcherTimer checkReadedFileTimer		   { get;		   set; }
		private object												 _lockObject;
		
		public  DelegateCommand<object>						 CreateCommand						{ get; private set; }
		public  DelegateCommand									 SendPriviewTextCommand			{ get; private set; }
		public  DelegateCommand								    AddDataTypeCommand				{ get; private set; }

		public CsvFileInfoViewModel(IEventAggregator eventAggregator)
		{			
			_eventAggregator					= eventAggregator;
			_classDetailInfoService			= new ClassDetailInfoService();
			_classInfoService					= new ClassInfoService();
			_settingTypeService				= new SettingTypeService();

			AccessModifiers					= new ObservableCollection<string>();
			MemberDataTypes				   = new ObservableCollection<string>();
			MemberTypes						   = new ObservableCollection<string>();
			ClassTypes						   = new ObservableCollection<string>();

			Initialize();
		}

		private void CheckReadedFileTimerTick(object sender, EventArgs e)
		{
			lock(_lockObject)
			{
				if (_classDetailInfoService.GetCount() > 0)
				{
					_classInfos.Clear();
					var ReceivedClassNames = _classDetailInfoService.GetAll().Select(classDetailInfo => classDetailInfo.ClassName).Distinct();

					foreach (var ReceivedClassName in ReceivedClassNames)
					{
						_classInfos.Add(new ClassInfoModel
						{
							SequenceNumber = 0,
							AccessModifier = Constants.AccessModifierDefault,
							ClassType		= Constants.ClassTypeDefault,
							ClassName	   = ReceivedClassName
						});
					}
					checkReadedFileTimer.Stop();
				}
			}		
		}

		private void CreateClassDetailInfos(object classInfo)
		{
			var Receivedinfo = classInfo as ClassInfoModel;
			if (Receivedinfo is null)		  return;

			var className					  = Receivedinfo.ClassName;
			SelectedClassName = className;
			var selectedclassDetailInfos = _classDetailInfoService.GetAll().Where(classDetailInfo => classDetailInfo.ClassName == className);
	
			if (_classDetailInfos.Count > 0) _classDetailInfos.Clear();

			foreach (var classDetailInfo in selectedclassDetailInfos)
			{
				_classDetailInfos.Add(classDetailInfo);
			}
		}

		private void SendCodingText()
		{
			// 유효성 검사하고 
			var convertingData = new ConvertTo();
			var selectedClassInfo = ClassInfos.Where(classInfo => string.Compare(classInfo.ClassName ,SelectedClassName,true) == 0).First();
			var classDetailInfos = ClassDetailInfos;

			var DetailedInfosToThis = classDetailInfos.Where(classDetailInfo => classDetailInfo.ClassName == selectedClassInfo.ClassName);
			convertingData.Initialize(selectedClassInfo, DetailedInfosToThis);
			convertingData.StartText();
			convertingData.FieldsText();
			convertingData.PropertiesText();
			convertingData.ConstructorText();
			convertingData.MethodsText();
			convertingData.EndText();
			var stringResult = convertingData.Result();
			// 로그 던진다.
			// 가는 방향이 다르게 하면되겠네
			// true를 던지면 
			_eventAggregator.GetEvent<SendConvertedMessage>().Publish(stringResult);

			convertingData.Reset();	
		}

		private void AddDatatype()
		{
			// 1 정합성 검사
			if (string.IsNullOrWhiteSpace(_newDataType)) return;

			MemberDataTypes.Add(NewDataType);
			_newDataType = string.Empty;
		}

		private void Initialize()
		{
			InitializeCommand();

			InitializeData();

			InitializeTimer();
		}

		#region 종류에 맞는 Initialize로 구성

		private void InitializeData()
		{
			foreach (var AccessModifier in _settingTypeService.GetAccessModifiers())
			{
				AccessModifiers.Add(AccessModifier);
			}

			foreach (var MemberDataType in _settingTypeService.GetDataTypes())
			{
				MemberDataTypes.Add(MemberDataType);
			}

			foreach (var MemberType in _settingTypeService.GetMemberTypes())
			{
				MemberTypes.Add(MemberType);
			}

			foreach (var ClassType in _settingTypeService.GetClassTypes())
			{
				ClassTypes.Add(ClassType);
			}

			_classInfos			= new ObservableCollection<ClassInfoModel>();
			_classDetailInfos = new ObservableCollection<ClassDetailInfoModel>();
		}

		private void InitializeCommand()
		{
			CreateCommand						= new DelegateCommand<object>(CreateClassDetailInfos);
			SendPriviewTextCommand			= new DelegateCommand(SendCodingText);
			AddDataTypeCommand				= new DelegateCommand(AddDatatype);
		}

		private void InitializeTimer()
		{
			checkReadedFileTimer			   = TimerManager.CheckReadedCsvFileTimer;
			checkReadedFileTimer.Tick     += CheckReadedFileTimerTick;
			checkReadedFileTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
			_lockObject						   = new object();
		}

		#endregion
	}
}
