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

namespace Modules.Setting.ViewModels
{
	public class FileManagementViewModel : BindableBase
	{
		private readonly IClassService<ClassDetailInfoModel> _classDetailInfoService;

		public DelegateCommand ImportCommand     { get; private set; }
		public DelegateCommand ExportCommand     { get; private set; }
		public DelegateCommand OpenFolderCommand { get; private set; }
		
		public FileManagementViewModel(IEventAggregator eventAggregator)
		{
			eventAggregator.GetEvent<SendSavingMessages>().Subscribe(MessagesReceived);
			_classDetailInfoService = new ClassDetailInfoService();
			ImportCommand				= new DelegateCommand(OpenCsvFile);
			OpenFolderCommand			= new DelegateCommand(OpenFolder);
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
			FileManager CsvFileDlg = new FileManager(openFileDlg);

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

		private bool IsOPenNewFileWithoutSaving()
		{
			var result = Message.InfoMessage("변환 데이터를 저장안하십니까?");

			if (result == System.Windows.MessageBoxResult.OK)
			{
				return true;
			}

			return false;
		}

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
			var className = string.Empty;
			var text = string.Empty;
			foreach (var convertedResult in convertedResults)
			{
				var results = convertedResult.GetType().GetProperties();
				className = results[0].GetValue(convertedResult, null).ToString();
				text		 = results[1].GetValue(convertedResult, null).ToString();
				FileManager.CreateTxtFile(className.ToString());
				FileManager.WriteTXT(text.ToString());
			}

			Message.InfoOKMessage("성공적으로 저장했습니다.");
		}
	}
}
