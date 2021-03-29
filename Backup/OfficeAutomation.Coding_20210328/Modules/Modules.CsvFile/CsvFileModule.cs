using Modules.CsvFile.Views;
using OfficeAutomation.Coding.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.CsvFile
{
	public class CsvFileModule : IModule
	{
		private IRegionManager _regionManager;

		public CsvFileModule(IRegionManager regionManager)
		{
			this._regionManager = regionManager;
		}

		public void OnInitialized(IContainerProvider containerProvider)
		{
			_regionManager.RegisterViewWithRegion(RegionNames.WriteContentRegion, typeof(CsvFileInfo));
			_regionManager.RegisterViewWithRegion(RegionNames.SettingRegion, typeof(Import));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{

		}
	}
}
