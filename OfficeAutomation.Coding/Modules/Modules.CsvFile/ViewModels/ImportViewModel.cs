using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using OfficeAutomation.Coding.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Modules.CsvFile.ViewModels
{
	public class ImportViewModel : BindableBase
	{
		private readonly IClassService<ClassDetailInfoModel> _classDetailInfoService;

		public  DelegateCommand OpenFileDialogCommand { get; private set; }

		public ImportViewModel()
		{
			OpenFileDialogCommand   = new DelegateCommand(OpenCsvFile);
			_classDetailInfoService = new ClassDetailInfoService();
		}

		private void OpenCsvFile()
		{
			var			  selectedFilepath = string.Empty;
			var			  openFileDlg		 = new Microsoft.Win32.OpenFileDialog();
			CsvFileDialog CsvFileDlg		 = new CsvFileDialog(openFileDlg);

			CsvFileDlg.Initialize();
			selectedFilepath		= CsvFileDlg.GetDialogFilePath();

			if (IsCompatablitity(_classDetailInfoService, selectedFilepath) is false) return;

			var csvFileList		= CsvFileDlg.ReadCSV(selectedFilepath);
			if (csvFileList != null)
			{
				var ConvertedData = CSVToObjects(csvFileList);
				_classDetailInfoService.AddRange(ConvertedData);

				var classNames = GetClassNames(ConvertedData);
				//var viewModel = MovingView.GetCurrentView;
				//viewModel.SendClassNames(classNames);
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
			var result = Message.InfoMessage("저장안하시고 새로운 파일을 열 것입니까?");

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
					ClassName		= data[0]						      ,
					DataType		   = Constants.DataTypeDefault      ,
					MemberName		= data[1]						      ,
					MemberType		= Constants.MemberTypeDefault    ,
					Comment			= data[2]
				};
			}));
		}

		private List<string> GetClassNames(List<ClassDetailInfoModel> ClassDetailInfos)
		{
			var classNames = ClassDetailInfos.Select(o => o.ClassName).Distinct();
			var list = new List<string>();

			foreach (var className in classNames)
			{
				list.Add(className);
			}

			return list;
		}
	}	
}
