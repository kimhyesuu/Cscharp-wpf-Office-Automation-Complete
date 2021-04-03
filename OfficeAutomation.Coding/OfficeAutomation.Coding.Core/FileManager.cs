using Microsoft.Win32;
using System;
using System.IO;
using System.Text;

namespace OfficeAutomation.Coding.Core
{
	public class FileManager 
	{
		public static string  CsvPath			 { get; set; }
		public static string  TxtFile			 { get; set; }
		public static string  TxtFolderName	 { get; set; } 
		public OpenFileDialog OpenFileDialog { get; set; }

		public FileManager(OpenFileDialog openFileDialog)
		{
			OpenFileDialog = openFileDialog;
		}

		public void	    Initialize()
		{
			OpenFileDialog.InitialDirectory = GetCsvPath();
			CsvPath								  = OpenFileDialog.InitialDirectory.ToString();
			OpenFileDialog.Filter			  = Constants.CsvAndAllFileFilter;
		}
					    
		public string   GetDialogFilePath()
		{
			var result			   = OpenFileDialog.ShowDialog();
			var selectedFilepath = string.Empty;

			if (result.HasValue is true && result.Value is true)
			{
				selectedFilepath = OpenFileDialog.FileName;
				SaveTxtFolderName(selectedFilepath);
				return selectedFilepath;
			}

			return selectedFilepath;
		}

		public string[] ReadCSV(string fileName)
		{
			string[] list = null;

			if (File.Exists(fileName) is true)
			{
				list = File.ReadAllLines(Path.ChangeExtension(fileName, ".csv"));
				return list;
			}

			return list;
		}

		private string   GetCsvPath()
		{
			string path = string.Empty;

			string workingDirectory = Environment.CurrentDirectory;
			path = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + Constants.CsvFolder;

			return path;
		}

		private void     SaveTxtFolderName(string selectedFilepath)
		{
			var fullName = selectedFilepath.Substring(selectedFilepath.LastIndexOf('\\') + 1);
			TxtFolderName = fullName.Substring(0, fullName.LastIndexOf('.'));
		}

		public static void CreateTxtFile(string fileName)
		{
			var resultFolderPath = CsvPath + Constants.txtFolder;
			if (Directory.Exists(resultFolderPath) == false)
			{
				Directory.CreateDirectory(resultFolderPath);
			}

			var TxtPath = resultFolderPath + $"\\{TxtFolderName}";
			if (Directory.Exists(TxtPath) == false)
			{
				Directory.CreateDirectory(TxtPath);
			}

			TxtFile = Path.Combine(TxtPath, fileName + Constants.TXT);
			using (FileStream fs = File.Create(TxtFile)) { };
		}

		public static void WriteTXT(string text)
		{
			using (FileStream file = new FileStream(TxtFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
			{
				using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
				{
					writer.Write(text);
				}
			}

		}
	}
}