using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Objects
{
	public class ClassSetting
	{
		public const string Class				= "class";

		public static int? SequentialNumber { get; set; }
		public string		 AccessModifier   { get; set; }
		public string		 ClassType        { get; set; }
		public string		 ClassName        { get; set; }
		public static bool IsDelete         { get; set; }
		public string	    Comment				{ get; set; }

		public ClassSetting()
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
