using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Business.Services;
using OfficeAutomation.Coding.Core;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Modules.CsvFile.ViewModels
{
	public class ImportViewModel : BindableBase
	{
		private readonly IClassService<ClassInfoModel> _classDetailInfoService;

		public  DelegateCommand OpenFileDialogCommand { get; private set; }

		public ImportViewModel()
		{
			OpenFileDialogCommand   = new DelegateCommand(OpenCsvFile);
			_classDetailInfoService = new ClassDetailInfoService();
		}

		private void OpenCsvFile()
		{
			// _classDetailInfoService 값이 있는지 판단 하기 
			
			var			  selectedFilepath = string.Empty;
			var			  openFileDlg		 = new Microsoft.Win32.OpenFileDialog();
			CsvFileDialog CsvFileDlg		 = new CsvFileDialog(openFileDlg);

			CsvFileDlg.Initialize();

			selectedFilepath		= CsvFileDlg.GetDialogFilePath();
			var csvFileList		= CsvFileDlg.ReadCSV(selectedFilepath);

			if(csvFileList is null) return;

			_classDetailInfoService.AddRange(CSVToObjects(csvFileList));
		}

		public List<ClassInfoModel> CSVToObjects(string[] lines)
		{
			return new List<ClassInfoModel>(lines.Select(line =>
			{
				string[] data = line.Split(Constants.Comma);

				return new ClassInfoModel()
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
	}	
}
