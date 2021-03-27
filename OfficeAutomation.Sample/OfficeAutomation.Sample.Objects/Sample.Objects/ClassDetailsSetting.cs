using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Objects
{	
	public class ClassDetailsSetting
	{
		public static int?  SequentialNumber { get; set; }
		public string		  AccessModifier   { get; set; }
		public string		  DataType		    { get; set; }
		public string		  MemberName	    { get; set; }
		public string		  MemberType	    { get; set; }
		public string		  DeleteMember	    { get; set; }
		public bool			  IsDelete			 { get; set; }

		public ClassDetailsSetting()
		{
			IsDelete = false;
		}
	}
}
