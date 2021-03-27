using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Objects
{
	public class ClassInfo
	{
		public const string Class = "class";

		public static int? SequentialNumber { get; set; }
		public string AccessModifier { get; set; }
		public string ClassType { get; set; }
		public string ClassName { get; set; }
		public string Comment { get; set; }
		public string MemberDataType { get; set; }
		public string MemberName { get; set; }
		public string MemberType { get; set; }
	}
}
