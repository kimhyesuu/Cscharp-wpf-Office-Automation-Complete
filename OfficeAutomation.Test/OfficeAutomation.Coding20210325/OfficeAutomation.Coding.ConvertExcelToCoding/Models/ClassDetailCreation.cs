using OfficeAutomation.Coding.ConvertExcelToCoding.Base;

namespace OfficeAutomation.Coding.ConvertExcelToCoding.Models
{
	public class ClassDetailCreation : ObservableBase
	{
		private int? sequentialNumber;
		public int? SequentialNumber
		{
			get { return sequentialNumber; }
			set { SetProperty(ref sequentialNumber, value); }
		}

		private string accessModifier;
		public string AccessModifier
		{
			get { return accessModifier; }
			set { SetProperty(ref accessModifier, value); }
		}

		private string dataType;
		public string DataType
		{
			get { return dataType; }
			set { SetProperty(ref dataType, value); }
		}

		private string memberName;
		public string MemberName
		{
			get { return memberName; }
			set { SetProperty(ref memberName, value); }
		}

		private string memberType;
		public string MemberType
		{
			get { return memberType; }
			set { SetProperty(ref memberType, value); }
		}
	}
}
