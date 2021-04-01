﻿using Modules.CsvFile.Convert;
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
			get => _newDataType; 
			set => SetProperty(ref _newDataType, value); 
		}

		private string												 SelectedClassName				{ get;			set; }  // CreateClassDetailInfos : Receive, SendCodingText : Using

		public  ObservableCollection<string>				 ClassAccessModifiers         { get;				  }
		public  ObservableCollection<string>			    AccessModifiers					{ get;		  	     }
		public  ObservableCollection<string>			    MemberDataTypes				   { get;		  	     } 
		public  ObservableCollection<string>			    MemberTypes					   { get;		  		  } 
		public  ObservableCollection<string>			    ClassTypes						   { get;		    	  } 
																											   				  
		public  System.Windows.Threading.DispatcherTimer checkReadedFileTimer		   { get;		   set; }
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
						_classInfos.Add(new ClassInfoModel
						{
							SequenceNumber = 0,
							AccessModifier = "public"			,
							ClassType		= Constants.ClassTypeDefault,
							ClassName	   = ReceivedClassName
						});
					}
					checkReadedFileTimer.Stop();
				}
			}		
		}
						   
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

			var className					  = Receivedinfo.ClassName;
			SelectedClassName = className;
			var selectedclassDetailInfos = _classDetailInfoService.GetAll().Where(classDetailInfo => classDetailInfo.ClassName == className);
	
			if (_classDetailInfos.Count > 0) _classDetailInfos.Clear();

			foreach (var classDetailInfo in selectedclassDetailInfos)
			{
				_classDetailInfos.Add(classDetailInfo);
			}
		}
						   
		private void   SendPriviewText()
		{
			var textResult          = string.Empty;
			var errorLogs           = new List<string>();
			var convertingData      = new ConvertTo();
			var selectedClassInfo   = ClassInfos.Where(classInfo => string.Compare(classInfo.ClassName ,SelectedClassName,true) == 0).FirstOrDefault();
			var DetailedInfosToThis = ClassDetailInfos.Where(classDetailInfo => classDetailInfo.ClassName == selectedClassInfo.ClassName);

			if (selectedClassInfo is null || DetailedInfosToThis.Count() == 0) return;

			var emptyOrError = convertingData.Initialize(selectedClassInfo, DetailedInfosToThis);
			if (emptyOrError != string.Empty)
			{
				SendErrorLogging(emptyOrError);
				return;
			}
			SendErrorLogging(emptyOrError);

			convertingData.StartText();

			errorLogs.Add(convertingData.ConstantsText()  );
			errorLogs.Add(convertingData.FieldsText()	    );
			errorLogs.Add(convertingData.PropertiesText() );
			errorLogs.Add(convertingData.ConstructorText());
			errorLogs.Add(convertingData.MethodsText()    );

			convertingData.EndText();

			if (IsCompability(errorLogs) is true)
			{
				textResult = convertingData.Result();
				_eventAggregator.GetEvent<SendConvertedMessage>().Publish(textResult);	
			}
			convertingData.Reset();
		}
						   
		private void   SendResults()
		{
			throw new NotImplementedException();
		}
						   
		private void   AddDatatype()
		{		
			if (string.IsNullOrWhiteSpace(_newDataType) ||
				 ClassDetailInfos.Count == 0)
			{
				SendErrorLogging("클래스 세부 사항 목록이 없거나 새로운 타입이 빈 공란입니다.");
				return;
			}
			else if (IsSpecialText(_newDataType) is true)
			{
				SendErrorLogging("특수문자 제거해주세요.(제외 특수문자 : <, >)");
				return;
			}

			MemberDataTypes.Add(NewDataType);
			_newDataType = string.Empty;
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
			if (receivedinfo is null) return "클래스 데이터가 없습니다.";

			var result = string.Compare(receivedinfo.AccessModifier, "public"  , true) == 0 ||
							 string.Compare(receivedinfo.AccessModifier, "internal", true) == 0 ? true : false;

			if(result is false) return $"AccessModifier {receivedinfo.AccessModifier}(이)가 올바르지 않습니다.";

			return string.Empty;
		}

		private bool   IsSpecialText(string txt)
		{
			// string str = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";		
			string str = @"[~!@\#$%^&*\()\=+|\\/:;?""']";
			var	 rex = new System.Text.RegularExpressions.Regex(str);
			return rex.IsMatch(txt);
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

			_classInfos			= new ObservableCollection<ClassInfoModel>();
			_classDetailInfos = new ObservableCollection<ClassDetailInfoModel>();
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
