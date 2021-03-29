using Modules.Coding;
using Modules.CsvFile;
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
			//containerRegistry.RegisterDialog<CodingInfo, CodingInfoViewModel>();
			//containerRegistry.RegisterDialog<ProductInfo, ProductInfoViewModel>();

			//containerRegistry.RegisterDialogWindow<RegistryDialogWindow>();
		}

		protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
		{
			base.ConfigureModuleCatalog(moduleCatalog);
			moduleCatalog.AddModule<CodingModule>();
			moduleCatalog.AddModule<CsvFileModule>();
		}
	}
}
