using Prism.Mvvm;

namespace OfficeAutomation.Coding.Business.Models
{
	public class ModelBase : BindableBase
	{
		public int	  SequenceNumber { get; set; }
		public string AccessModifier { get; set; }
		public string ClassName		  { get; set; }
	}
}
