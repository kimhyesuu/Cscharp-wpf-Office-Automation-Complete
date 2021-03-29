using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		private readonly IClassService<ClassInfoModel> _classInfoService;
		//public static	  System.Windows.Threading.DispatcherTimer CheckReadedCsvFileTimer { get; set; }
		private			  IEventAggregator				  _eventAggregator;

		public CsvFileInfoViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator		   = eventAggregator;
			_classDetailInfoService = new ClassDetailInfoService();
			_classInfoService			= new ClassInfoService();
			_classDetailInfos		   = new ObservableCollection<ClassDetailInfoModel>();
			_classInfos					= new ObservableCollection<ClassInfoModel>();
			// 여기서 갖고 오자 

			// 다시 열 때 문제가 있다. TimerManager를 만들자 
			//CheckReadedCsvFileTimer = new System.Windows.Threading.DispatcherTimer();
			//CheckReadedCsvFileTimer.Tick += CheckReadedCsvFileTimerTick;
			//CheckReadedCsvFileTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
			//CheckReadedCsvFileTimer.Start();
		}

		private void CheckReadedCsvFileTimerTick(object sender, EventArgs e)
		{	  
			if (_classDetailInfoService.GetCount() > 0)
			{
				var ReceivedClassNames = GetClassNames(_classDetailInfoService.GetAll());

				foreach(var ReceivedClassName in ReceivedClassNames)
				{
					_classInfos.Add(new ClassInfoModel
					{
						ClassName = ReceivedClassName
					});
				}
				//CheckReadedCsvFileTimer.Stop();
			}		
		}

		public static void SendCsvFile(List<ClassDetailInfoModel> convertedData)
		{

		}

		private List<string> GetClassNames(IEnumerable<ClassDetailInfoModel> ClassDetailInfos)
		{
			var classNames = ClassDetailInfos.Select(o => o.ClassName).Distinct();
			var list = new List<string>();

			foreach (var className in classNames)
			{
				list.Add(className);
			}

			return list;
		}
	}
}
