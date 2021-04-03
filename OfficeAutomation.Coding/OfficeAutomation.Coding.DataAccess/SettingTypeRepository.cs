using System.Collections.Generic;

namespace OfficeAutomation.Coding.Repository
{
	public class SettingTypeRepository : ISettingTypeRepository
	{
		public List<string> AccessModifiers      { get; }
		public List<string> ClassTypes		     { get; }
		public List<string> DataTypes			     { get; }
		public List<string> MemberTypes		     { get; }

		public SettingTypeRepository()
		{
			AccessModifiers = new List<string>()
			{
				"private"           ,
				"public"            ,
				"protected"         ,
				"internal"          ,
				"protected internal",
				"private protected" ,
			};

			ClassTypes		 = new List<string>()
			{
				"none"				  ,
				"abstract"          ,
				"partial"           ,
				"sealed"            ,
				"static"				  ,				
			};
								 
			DataTypes	    = new List<string>()
			{
				"void"               ,
				"int"               ,
				"uint"              ,
				"long"              ,
				"decimal"           ,
				"double"            ,
				"float"             ,
				"long"              ,
				"ulong"             ,
				"short"             ,
				"ushort"            ,
				"bool"              ,
				"byte"              ,
				"sbyte"             ,
				"char"              ,
				"string"				  ,
			};
								 
			MemberTypes     = new List<string>()
			{
				"field"              ,
				"Constant"           ,
				"Property"           ,
				"Method"             ,
			};
		}
	}
}
