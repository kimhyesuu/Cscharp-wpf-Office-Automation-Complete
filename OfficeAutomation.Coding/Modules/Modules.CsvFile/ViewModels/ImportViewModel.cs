using OfficeAutomation.Coding.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;

namespace Modules.CsvFile.ViewModels
{
	public class ImportViewModel : BindableBase
	{
		public DelegateCommand OpenFileDialogCommand { get; private set; }

		public ImportViewModel()
		{
			OpenFileDialogCommand = new DelegateCommand(OpenCsvFile);
		}

		private void OpenCsvFile()
		{
			var openFileDlg = new Microsoft.Win32.OpenFileDialog();

			openFileDlg.InitialDirectory = GetCsvPath();
			openFileDlg.Filter = Constants.CsvAndAllFileFilter;

			var result = openFileDlg.ShowDialog();

			if (result.HasValue is true && result.Value is true)
			{
				var selectedFilepath = openFileDlg.FileName;
				// Detail 먼저 받고 
				// Class Info만드자 
				//ClassDetailInfoRepository = new List<ClassDetailInfo>(ReadCSV(selectedFilepath));
			}
		}

		private string GetCsvPath()
		{
			string path = string.Empty;

			string workingDirectory = Environment.CurrentDirectory;
			path = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + Constants.CsvFolder;

			return path;
		}
	}

	
}
