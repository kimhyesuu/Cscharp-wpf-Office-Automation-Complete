using Modules.Coding;
using Modules.CsvFile;
using Modules.Log;
using Modules.Setting;
using OfficeAutomation.Coding.ConvertExcelToCoding.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace OfficeAutomation.Coding.ConvertExcelToCoding
{
	public partial class App : PrismApplication
	{
		protected override Window CreateShell()
		{
			return Container.Resolve<ShellWindow>();
		}

		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			base.ConfigureModuleCatalog(moduleCatalog);
			moduleCatalog.AddModule<CodingModule>();
			moduleCatalog.AddModule<CsvFileModule>();
			moduleCatalog.AddModule<SettingModule>();
			moduleCatalog.AddModule<LogModule>();
		}
	}
}
