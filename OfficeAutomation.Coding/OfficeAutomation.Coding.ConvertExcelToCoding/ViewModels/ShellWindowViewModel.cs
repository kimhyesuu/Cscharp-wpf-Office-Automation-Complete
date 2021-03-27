using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Coding.ConvertExcelToCoding.ViewModels
{
	public class ShellWindowViewModel : BindableBase
	{
		public ShellWindowViewModel()
		{
			
		}

		public string MainTitle
		{
			get => "Coding Automation System - Convert Excel Content List to Coding Terms";			
		}
	}
}
