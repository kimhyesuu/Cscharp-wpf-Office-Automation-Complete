using Modules.Coding.Views;
using OfficeAutomation.Coding.Core;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Coding
{
	public class CodingModule : IModule
	{
		private IRegionManager _regionManager;

		public CodingModule(IRegionManager regionManager)
		{
			this._regionManager = regionManager;
		}

		public void OnInitialized(IContainerProvider containerProvider)
		{
			_regionManager.RegisterViewWithRegion(RegionNames.ResultContentRegion, typeof(CodingInfo));
		}

		public void RegisterTypes(IContainerRegistry containerRegistry)
		{

		}
	}
}
