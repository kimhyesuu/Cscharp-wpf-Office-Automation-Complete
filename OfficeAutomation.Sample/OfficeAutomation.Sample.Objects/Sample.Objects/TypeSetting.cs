using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Objects
{
	public class TypeSetting
	{
		public List<string> AccessModifiers { get; }
		public List<string> ClassTypes		{ get; }
		public List<string> DataTypes			{ get; }
		public List<string> MemberTypes		{ get; }

		public TypeSetting()
		{
			AccessModifiers = new List<string>()
			{
				"private"           ,
				"public"            ,
				"protected"         ,
				"internal"          ,
				"protected internal",
				"private protected"
			};

			ClassTypes = new List<string>()
			{
				"Abstract"			  ,
				"Partial"			  ,
				"Sealed"				  ,
				"Static"				 
			};

			DataTypes = new List<string>()
			{
				"<Write Object>"	  ,
				"int"					  ,
				"uint"				  ,
				"long"				  ,				
				"decimal"			  ,
				"double"				  ,
				"float"				  ,
				"long"				  ,
				"ulong"				  ,
				"short"				  ,
				"ushort"				  ,
				"bool"				  ,
				"byte"				  ,
				"sbyte"				  ,
				"char"				  ,
				"string"  
			};

			MemberTypes = new List<string>()
			{
				"field"				   ,
				"Constant"			   ,
				"Property"			   ,
				"Method"				   ,
				"Event"				   ,
				"Constructor"		   ,
			};
		}
	}
}
