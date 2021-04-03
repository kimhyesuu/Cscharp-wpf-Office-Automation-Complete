using OfficeAutomation.Coding.Business.Services;

namespace OfficeAutomation.Coding.Business.Models
{
	public class ClassInfoModel : ModelBase, ISequencedObject
	{
		private int _sequenceNumber;
		private string _classType;

		public int SequenceNumber
		{
			get { return _sequenceNumber; }
			set { SetProperty(ref _sequenceNumber, value); }
		}

		public string ClassType
		{
			get { return _classType; }
			set { SetProperty(ref _classType, value); }
		}

		public ClassInfoModel() { }

	}
}
