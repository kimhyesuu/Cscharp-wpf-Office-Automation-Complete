using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Modules.CsvFile.ViewModels
{
	public class CsvFileInfoViewModel : BindableBase
	{
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

		private readonly IClassService<ClassDetailInfoModel> _classDetailInfoService;
		private readonly IClassService<ClassInfoModel>		  _classInfoService;
		private			  IEventAggregator						  _eventAggregator;

		public CsvFileInfoViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator		   = eventAggregator;
			_classDetailInfoService = new ClassDetailInfoService();
			_classInfoService			= new ClassInfoService();
			_classDetailInfos		   = new ObservableCollection<ClassDetailInfoModel>();
			_classInfos					= new ObservableCollection<ClassInfoModel>();
			//로딩할 떄 콤보박스 내용을 넣어야한다.
		}

		//private void CheckReadedCsvFileTimerTick(object sender, EventArgs e)
		//{	  
		//	if (_classDetailInfoService.GetCount() > 0)
		//	{
		//		var ReceivedClassNames = GetClassNames(_classDetailInfoService.GetAll());

		//		foreach(var ReceivedClassName in ReceivedClassNames)
		//		{
		//			_classInfos.Add(new ClassInfoModel
		//			{
		//				ClassName = ReceivedClassName
		//			});
		//		}
		//		//CheckReadedCsvFileTimer.Stop();
		//	}		
		//}

		//public void SendClassNames(List<string> classNames)
		//{
		//	var CsvFileInfo = new CsvFileInfoViewModel();
		//	CsvFileInfo.AddClassInfo(classNames);
		//}

		public void ReceivedClassNames(List<string> classNames)
		{
			foreach (var className in classNames)
			{
				_classInfos.Add(new ClassInfoModel
				{
					ClassName = className
				});
			}
		}
	}
}
