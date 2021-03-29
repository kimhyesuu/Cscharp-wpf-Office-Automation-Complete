using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.CsvFile.ViewModels
{
	public class CsvFileInfoViewModel : BindableBase
	{
		private readonly IClassService<ClassInfoModel> _classDetailInfoService;
		private readonly IClassService<ClassInfoModel> _classInfoService;

		private			  IEventAggregator				  _eventAggregator;

		public CsvFileInfoViewModel(IEventAggregator eventAggregator)
		{
			_eventAggregator		   = eventAggregator;
			_classDetailInfoService = new ClassDetailInfoService();
			_classInfoService			= new ClassInfoService();


			// 타이머 사용하고 
			// 그 값이 
		}
	}
}
