namespace OfficeAutomation.Coding.Business.Models
{
	public class ClassDetailInfoModel : ModelBase
	{
		private string _dataType;
		private string _memberName;
		private string _memberType;
		private string _comment;

		public string DataType
		{
			get { return _dataType; }
			set { SetProperty(ref _dataType, value); }
		}

		public string MemberName
		{
			get { return _memberName; }
			set { SetProperty(ref _memberName, value); }
		}

		public string MemberType
		{
			get { return _memberType; }
			set { SetProperty(ref _memberType, value); }
		}

		public string Comment
		{
			get { return _comment; }
			set { SetProperty(ref _comment, value); }
		}

		public ClassDetailInfoModel() { }

	
		 //public string DataType	     { get; set; }
		 //public string MemberName     { get; set; }
		 //public string MemberType     { get; set; }
		 //public string Comment		  { get; set; }
	}
}
