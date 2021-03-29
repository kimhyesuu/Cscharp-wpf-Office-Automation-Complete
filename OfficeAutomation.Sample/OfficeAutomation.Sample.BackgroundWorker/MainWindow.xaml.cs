using System.Windows;

namespace OfficeAutomation.Sample.BackgroundWorkerExe
{
	public partial class MainWindow : Window
	{
		public MainWindow(MainViewModel mainViewModel)
		{
			InitializeComponent();
			this.DataContext = mainViewModel;
		}
	}
}
