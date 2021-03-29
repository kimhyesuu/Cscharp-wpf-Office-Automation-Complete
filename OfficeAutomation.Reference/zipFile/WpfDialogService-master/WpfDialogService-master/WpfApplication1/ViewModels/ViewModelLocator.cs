using Ninject;

namespace WpfApplication1.ViewModels
{
    public class ViewModelLocator
    {
		public MainWindowViewModel MainWindowViewModel { get; set; } = null;

		public ViewModelLocator()
        {
            this.MainWindowViewModel =
               IoC.Container.Instance.Kernel.Get<MainWindowViewModel>();
        }
    }
}
