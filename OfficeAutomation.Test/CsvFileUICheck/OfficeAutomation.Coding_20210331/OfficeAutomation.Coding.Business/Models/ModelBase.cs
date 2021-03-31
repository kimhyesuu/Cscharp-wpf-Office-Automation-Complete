using OfficeAutomation.Coding.Business.Services;
using Prism.Mvvm;

namespace OfficeAutomation.Coding.Business.Models
{
	public class ModelBase : BindableBase, ISequencedObject
	{
		private int		_sequenceNumber;
		private string _accessModifier;
		private string _className;

		public int SequenceNumber
		{
			get { return _sequenceNumber; }
			set { SetProperty(ref _sequenceNumber, value); }
		}

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
