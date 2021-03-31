using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules.CsvFile.Convert
{
	public class ConvertTo
	{
		private ClassInfoModel					 ClassInfo					{ get; set; }
		private List<ClassDetailInfoModel>   ClassDetailInfos		   { get; set; }
		private StringBuilder			       CodingTextResult			{ get; set; }
		private const int cnt = 6;
		public		  ConvertTo()
		{
			ClassDetailInfos = new List<ClassDetailInfoModel>();
			CodingTextResult = new StringBuilder();
		}
		
		public void   Initialize(ClassInfoModel classInfo, IEnumerable<ClassDetailInfoModel> classDetailInfos)
		{			
			ClassInfo = new ClassInfoModel()
			{
				SequenceNumber = classInfo.SequenceNumber,
				AccessModifier = classInfo.AccessModifier,
				ClassType		= classInfo.ClassType,
				ClassName		= classInfo.ClassName
			};

			foreach (var classDetailInfo in classDetailInfos)
			{
				ClassDetailInfos.Add(new ClassDetailInfoModel()
				{
					SequenceNumber = classDetailInfo.SequenceNumber,
					AccessModifier = classDetailInfo.AccessModifier,
					MemberName = classDetailInfo.MemberName,
					MemberType = classDetailInfo.MemberType,
					DataType = classDetailInfo.DataType,
					Comment = classDetailInfo.Comment,
				});

			}
		}

		public void   Reset()
		{
			ClassInfo = null;
			ClassDetailInfos.Clear();
			CodingTextResult.Clear();
		}

		public string Result()
		{
			return CodingTextResult.ToString();
		}
		 
		public void	  StartText()
		{
			var text = string.Empty;

			if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeDefault,true) == 0)
			{
				text = "\t" + $"{ClassInfo.AccessModifier} class {ClassInfo.ClassName}";
			}
			else
			{
				text = "\t" + $"{ClassInfo.AccessModifier} {ClassInfo.ClassType} {ClassInfo.ClassName}";
			}

			CodingTextResult.AppendLine(text);
			CodingTextResult.AppendLine("\t{");
		}
						  
		public void	  EndText()
		{
			var text = "\t}";
			CodingTextResult.AppendLine(text);
			CodingTextResult.AppendLine();
			CodingTextResult.AppendLine();
			CodingTextResult.AppendLine();
		}

		public void	  FieldsText()
		{
			foreach (var ClassDetailInfo in ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Field, true) == 0))
			{
				if(string.Compare(ClassInfo.AccessModifier, Constants.AccessModifierDefault, true) == 0)
				{
					CodingTextResult.Append("\t".PadRight(cnt) + $"{ClassInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append("\t".PadRight(cnt) + $"{ClassDetailInfo.AccessModifier} ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.Append($"{ClassDetailInfo.DataType} {ClassDetailInfo.MemberName};");
				CodingTextResult.AppendLine($"// {ClassDetailInfo.Comment}");
			}
			CodingTextResult.AppendLine();
		}

		public void   MethodsText()
		{
			foreach (var ClassDetailInfo in ClassDetailInfos.Where(o => string.Compare(o.MemberType,Constants.Method,true) == 0))
			{
				if (string.Compare(ClassInfo.AccessModifier, Constants.AccessModifierDefault, true) == 0)
				{
					CodingTextResult.Append("\t".PadRight(cnt) + $"{ClassInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append("\t".PadRight(cnt) + $"{ClassDetailInfo.AccessModifier} ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.AppendLine($"{ClassDetailInfo.DataType} {ClassDetailInfo.MemberName}()");
				CodingTextResult.AppendLine("\t".PadRight(cnt) + "{");
				CodingTextResult.AppendLine();
				CodingTextResult.Append("\t".PadRight(cnt) + "}");
				CodingTextResult.AppendLine($"// {ClassDetailInfo.Comment}");
				CodingTextResult.AppendLine();
			}
		}		

		public void   ConstructorText()
		{
			var result = string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0 ? true : false;
			if (result is false)
			{
				CodingTextResult.AppendLine("\t".PadRight(cnt) + $"public {ClassInfo.ClassName}()");
				CodingTextResult.AppendLine("\t".PadRight(cnt) + "{");
				CodingTextResult.AppendLine();
				CodingTextResult.AppendLine("\t".PadRight(cnt) + "}");
				CodingTextResult.AppendLine();

			}
		}
	
		public void	  PropertiesText()
		{
			foreach (var ClassDetailInfo in ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Property, true) == 0))
			{
				if (string.Compare(ClassInfo.AccessModifier, Constants.AccessModifierDefault, true) == 0)
				{
					CodingTextResult.Append("\t".PadRight(cnt) + $"{ClassInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append("\t".PadRight(cnt) + $"{ClassDetailInfo.AccessModifier} ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.Append($"{ClassDetailInfo.DataType} {ClassDetailInfo.MemberName} ");
				CodingTextResult.Append("{ ");
				CodingTextResult.Append("get; set;");
				CodingTextResult.Append(" }");
				CodingTextResult.AppendLine($"// {ClassDetailInfo.Comment}");
			}
			CodingTextResult.AppendLine();
		}
	}
}
