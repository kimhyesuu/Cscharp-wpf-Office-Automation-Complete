using OfficeAutomation.Sample.BackgroundWorkerExe;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace OfficeAutomation.Sample.WorkerExe01
{
	public partial class MainWindow : Window
	{
		private readonly BackgroundWorker _worker = new BackgroundWorker();

		public MainWindow()
		{
			InitializeComponent();

			_worker.DoWork += WorkerOnDoWork;
			_worker.WorkerReportsProgress = true;
			_worker.ProgressChanged += WorkerOnProgressChanged;
		}

		private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			pg.Value = e.ProgressPercentage;
		}

		private void WorkerOnDoWork(object sender, DoWorkEventArgs e)
		{
			for (int i = 0; i <= 100; i++)
			{
				Thread.Sleep(50);
				_worker.ReportProgress(i);
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_worker.RunWorkerAsync();
		}
	}
}
