using OfficeAutomation.Coding.ConvertExcelToCoding.Base;
using System.Text;

namespace OfficeAutomation.Coding.ConvertExcelToCoding.Models
{
	public class ClassCreation : ObservableBase
	{
		public const string Class = "class";

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

		private string classType;
		public string ClassType
		{
			get { return classType; }
			set { SetProperty(ref classType, value); }
		}

		private string className;
		public string ClassName
		{
			get { return className; }
			set { SetProperty(ref className, value); }
		}

		public static bool IsDelete { get; set; }

		public ClassCreation()
		{
			IsDelete = false;
		}

		public string GetClassStartText()
		{
			var sb = new StringBuilder();

			sb.Append($"{AccessModifier} {Class} {ClassName}");
			sb.Append("{");
			return sb.ToString();
		}

		public string GetClassEndText()
		{
			return "}";
		}
	}
}
