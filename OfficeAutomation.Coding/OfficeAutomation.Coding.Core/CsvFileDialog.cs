using Microsoft.Win32;
using System;
using System.IO;

namespace OfficeAutomation.Coding.Core
{
	public class CsvFileDialog 
	{
		public static string  CsvPath			 { get; set; }
		public OpenFileDialog OpenFileDialog { get; set; }

		public CsvFileDialog(OpenFileDialog openFileDialog)
		{
			OpenFileDialog = openFileDialog;
		}

		public void Initialize()
		{
			OpenFileDialog.InitialDirectory = GetCsvPath();
			CsvPath								  = OpenFileDialog.InitialDirectory.ToString();
			OpenFileDialog.Filter			  = Constants.CsvAndAllFileFilter;
		}

		public string GetDialogFilePath()
		{
			var result			   = OpenFileDialog.ShowDialog();
			var selectedFilepath = string.Empty;

			if (result.HasValue is true && result.Value is true)
			{
				selectedFilepath = OpenFileDialog.FileName;
				return selectedFilepath;
			}

			return selectedFilepath;
		}

		public string[] ReadCSV(string fileName)
		{
			string[] list = null;

			if (File.Exists(fileName) is true)
			{
				list = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv"));
				return list;
			}

			return list;
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
