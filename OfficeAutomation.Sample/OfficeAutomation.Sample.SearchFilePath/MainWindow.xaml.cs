using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace OfficeAutomation.Sample.SearchFilePath
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var openFileDlg = new OpenFileDialog();
			openFileDlg.InitialDirectory = GetIoFilePath(); //csv의 이름이 필요하다.
			openFileDlg.Filter = "*.csv|";
			lbPath.Content = openFileDlg.InitialDirectory.ToString(); 
		}

		private string GetIoFilePath()
		{
			string path = string.Empty;

			string workingDirectory = Environment.CurrentDirectory;
			path = Directory.GetParent(workingDirectory).Parent.Parent.FullName; //  + "\\IO_File"

			return path;
		}
	}
}
