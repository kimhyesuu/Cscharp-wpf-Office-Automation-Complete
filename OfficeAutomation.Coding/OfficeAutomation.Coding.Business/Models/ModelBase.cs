using OfficeAutomation.Coding.Business.Services;
using Prism.Mvvm;

namespace OfficeAutomation.Coding.Business.Models
{
	public class ModelBase : BindableBase 
	{
		private string _accessModifier;
		private string _className;

		public string AccessModifier
		{
			get { return _accessModifier; }
			set { SetProperty(ref _accessModifier, value); }
		}

		public string ClassName
		{
			get { return _className; }
			set { SetProperty(ref _className, value); }
		}

		public ModelBase() { }
	
	}
}
