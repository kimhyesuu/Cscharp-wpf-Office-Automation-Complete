using Modules.Setting.Views;
using OfficeAutomation.Coding.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Modules.Setting
{
	public class SettingModule : IModule
	{
		private IRegionManager _regionManager;

		public SettingModule(IRegionManager regionManager)
		{
			this._regionManager = regionManager;
		}

		public void OnInitialized(IContainerProvider containerProvider)
		{
			_regionManager.RegisterViewWithRegion(RegionNames.SettingRegion, typeof(FileManagement));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{

		}
	}
}
