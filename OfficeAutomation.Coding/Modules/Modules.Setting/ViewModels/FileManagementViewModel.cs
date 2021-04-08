using OfficeAutomation.Coding.Business.Managers;
using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using OfficeAutomation.Coding.Core;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace Modules.Setting.ViewModels
{
	public class FileManagementViewModel : BindableBase
	{
		private readonly IClassService<ClassDetailInfoModel> _classDetailInfoService;

		private List<object>    ExportList			{ get;			set; }
		public  DelegateCommand ImportCommand     { get; private set; }
		public  DelegateCommand ExportCommand     { get; private set; }
		public  DelegateCommand OpenFolderCommand { get; private set; }
		
		public FileManagementViewModel(IEventAggregator eventAggregator)
		{
			eventAggregator.GetEvent<SendSavingMessages>().Subscribe(MessagesReceived);
			_classDetailInfoService = new ClassDetailInfoService();
			ImportCommand				= new DelegateCommand(OpenCsvFile);
			ExportCommand				= new DelegateCommand(ExportCoding);
			OpenFolderCommand			= new DelegateCommand(OpenFolder);
		}

		private void ExportCoding()
		{
			var className = string.Empty;
			var text		  = string.Empty;
			var messageBoxResult = MessageBox.Show("Export하시겠습니까?" , "EXPORT", MessageBoxButton.OKCancel);

			if (messageBoxResult == MessageBoxResult.Cancel) return;

			foreach (var exportText in ExportList)
			{
				var results = exportText.GetType().GetProperties();
				className   = results[0].GetValue(exportText, null).ToString();
				text        = results[1].GetValue(exportText, null).ToString();
				FileManager.CreateTxtFile(className.ToString());
				FileManager.WriteTXT(text.ToString());
			}

			Message.InfoOKMessage("성공적으로 저장했습니다.");
		}

		private void OpenFolder()
		{
			var path = FileManager.CsvPath;

			if(path != null)
			{
				Process.Start(path);
			}
		}

		private void OpenCsvFile()
		{
			var selectedFilepath     = string.Empty;
			var openFileDlg		    = new Microsoft.Win32.OpenFileDialog();
			FileManager CsvFileDlg   = new FileManager(openFileDlg);

			CsvFileDlg.Initialize();
			selectedFilepath			 = CsvFileDlg.GetDialogFilePath();

			if (IsCompatablitity(_classDetailInfoService, selectedFilepath) is false) return;

			var csvFileList			 = CsvFileDlg.ReadCSV(selectedFilepath);
			if (csvFileList != null)
			{
				var ConvertedData		 = CSVToObjects(csvFileList);
				_classDetailInfoService.AddRange(ConvertedData);
				TimerManager.CheckReadedCsvFileTimer.Start();
			}
		}

		private bool IsCompatablitity(IClassService<ClassDetailInfoModel> classDetailInfoService, string selectedFilepath)
		{
			if (selectedFilepath == string.Empty)
			{
				return false;
			}

			if (classDetailInfoService.GetCount() > 0)
			{
				if (IsOPenNewFileWithoutSaving() is true)
				{
					_classDetailInfoService.Clear();
					return true;
				}
				else
				{
					return false;
				}
			}

			return true;
		}

		private bool IsOPenNewFileWithoutSaving() => Message.InfoMessage("변환 데이터를 저장안하십니까?") == MessageBoxResult.OK ? true : false;

		private List<ClassDetailInfoModel> CSVToObjects(string[] lines)
		{
			return new List<ClassDetailInfoModel>(lines.Select(line =>
			{
				string[] data = line.Split(Constants.Comma);

				return new ClassDetailInfoModel()
				{
					AccessModifier = Constants.AccessModifierDefault,
					ClassName		= data[0]								,
					DataType			= Constants.DataTypeDefault		,
					MemberName		= data[1]								,
					MemberType		= Constants.MemberTypeDefault		,
					Comment			= data[2]								,
				};
			}));
		}

		private void MessagesReceived(List<object> convertedResults)
		{
			if (ExportList != null) ExportList.Clear();
			else							ExportList = new List<object>();

			foreach (var convertedResult in convertedResults)
			{
				ExportList.Add(convertedResult);
			}
		}
	}
}
