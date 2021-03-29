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
		private			  IEventAggregator						  _eventAggregator;

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

		public  ObservableCollection<string>			    AccessModifiers				   { get;		  		  } 
		public  ObservableCollection<string>			    MemberDataTypes				   { get;		  		  } 
		public  ObservableCollection<string>			    MemberTypes					   { get;		  		  } 
		public  ObservableCollection<string>			    ClassTypes						   { get;		  	  	  } 
																											   				  
		public  System.Windows.Threading.DispatcherTimer checkReadedFileTimer		   { get;		    set;}
		private object												 _lockObject;

		public  DelegateCommand									 SendConvertedMessageCommand  { get; private set; }
		public  DelegateCommand<object>						 SendSelectedClassInfoCommand { get; private set; }


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

		private void CheckReadedFileTimerTick(object sender, System.EventArgs e)
		{
			lock(_lockObject)
			{
				if (_classDetailInfoService.GetCount() > 0)
				{
					// 다시 클릭했을 때 첫번째만 터지게해야대나 							
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

		private void SendClassInfo(object classInfo)
		{
			var Receivedinfo = classInfo as ClassInfoModel;
			if (Receivedinfo.ClassName is null) return;


			var className = Receivedinfo.ClassName;
			var selectedclassDetailInfos = _classDetailInfoService.GetAll().Where(classDetailInfo => classDetailInfo.ClassName == className);

			if (_classDetailInfos.Count > 0) _classDetailInfos.Clear();

			foreach (var classDetailInfo in selectedclassDetailInfos)
			{
				_classDetailInfos.Add(classDetailInfo);
			}
		}

		private void SendMessage()
		{
			string convertedCsvToCodingmessage = string.Empty;

			// 만들기 

			_eventAggregator.GetEvent<SendConvertedMessage>().Publish(convertedCsvToCodingmessage);
		}

		private void Initialize()
		{
			InitializeCommand();

			InitializeData();

			InitializeTimer();
		}

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
			SendSelectedClassInfoCommand  = new DelegateCommand<object>(SendClassInfo);
			SendConvertedMessageCommand   = new DelegateCommand(SendMessage);
		}

	

		private void InitializeTimer()
		{
			checkReadedFileTimer			   = TimerManager.CheckReadedCsvFileTimer;
			checkReadedFileTimer.Tick     += CheckReadedFileTimerTick;
			checkReadedFileTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
			_lockObject						   = new object();
			checkReadedFileTimer.Start();
		}

		//아직 안씀
		private void InitializeClassInfo()
		{
				_classInfos = new ObservableCollection<ClassInfoModel>();
		}
	}
}
