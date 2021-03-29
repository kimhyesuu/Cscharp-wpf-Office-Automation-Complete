using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Sample.BackgroundWorkerExe
{
	public class MainViewModel  
	{
		private readonly BackgroundWorker _worker = new BackgroundWorker();

		public MainViewModel()
		{
			_worker.DoWork += WorkerOnDoWork;
			_worker.WorkerReportsProgress = true;
			_worker.ProgressChanged += WorkerOnProgressChanged;
		}

		private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs e)
		{
		}

		private void WorkerOnDoWork(object sender, DoWorkEventArgs e)
		{
		}
		// ProgressbarStartCommand
	}
}
