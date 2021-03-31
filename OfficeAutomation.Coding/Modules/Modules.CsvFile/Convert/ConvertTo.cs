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
		private const int spaceCount = 6;

		private ClassInfoModel					 ClassInfo					{ get; set; }
		private List<ClassDetailInfoModel>   ClassDetailInfos		   { get; set; }
		private StringBuilder			       CodingTextResult			{ get; set; }

		public ConvertTo()
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
				ClassType = classInfo.ClassType,
				ClassName = classInfo.ClassName
			};

			foreach (var classDetailInfo in classDetailInfos)
			{
				ClassDetailInfos.Add(new ClassDetailInfoModel()
				{
					SequenceNumber = classDetailInfo.SequenceNumber,
					AccessModifier = classDetailInfo.AccessModifier,
					MemberName	   = classDetailInfo.MemberName	  ,
					MemberType	   = classDetailInfo.MemberType    ,
					DataType       = classDetailInfo.DataType      ,
					Comment        = classDetailInfo.Comment       ,
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
			// 로그 데이터가 있으면 로그로 전달하자 
			return CodingTextResult.ToString();
		}

		public void	  StartText()
		{
			var text = string.Empty;

			if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeDefault, true) == 0)
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

		public string ConstructorText()
		{
			var result = string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0 ? true : false;
			if (result is false)
			{
				CodingTextResult.AppendLine("\t".PadRight(spaceCount) + $"public {ClassInfo.ClassName}()");
				CodingTextResult.AppendLine("\t".PadRight(spaceCount) + "{");
				CodingTextResult.AppendLine();
				CodingTextResult.AppendLine("\t".PadRight(spaceCount) + "}");
				CodingTextResult.AppendLine();
			}
			return string.Empty;
		}

		public string FieldsText()
		{
			var errorResult = string.Empty;
			var fields = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Field, true) == 0);

			if(IsExist(fields) is false) return string.Empty;

			foreach (var classDetailInfo in fields)
			{
				// void 검사
				if (string.Compare(classDetailInfo.DataType, "void" , true) == 0)
				{
				  	errorResult = $"void :{classDetailInfo.MemberName}가 Method Type이 아닙니다.";
					return errorResult;
				}

				if (string.Compare(ClassInfo.AccessModifier, Constants.AccessModifierDefault, true) == 0)
				{
					CodingTextResult.Append("\t".PadRight(spaceCount) + $"{ClassInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append("\t".PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.Append($"{classDetailInfo.DataType} {classDetailInfo.MemberName};");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					CodingTextResult.AppendLine($"// {classDetailInfo.Comment}");
				}
				else
				{
					CodingTextResult.AppendLine();
				}
			}

			CodingTextResult.AppendLine();
			return string.Empty;
		}

		public string PropertiesText()
		{
			var errorResult = string.Empty;
			var Properties  = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Property, true) == 0);

			if (IsExist(Properties) is false) return string.Empty;

			foreach (var classDetailInfo in Properties)
			{
				// void 검사
				if (string.Compare(classDetailInfo.DataType, "void", true) == 0)
				{
					errorResult = $"void :{classDetailInfo.MemberName}가 Method Type이 아닙니다.";
					return errorResult;
				}

				if (string.Compare(ClassInfo.AccessModifier, Constants.AccessModifierDefault, true) == 0)
				{
					CodingTextResult.Append("\t".PadRight(spaceCount) + $"{ClassInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append("\t".PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.Append($"{classDetailInfo.DataType} {classDetailInfo.MemberName} ");
				CodingTextResult.Append("{ ");
				CodingTextResult.Append("get; set;");
				CodingTextResult.Append(" }");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					CodingTextResult.AppendLine($"// {classDetailInfo.Comment}");
				}
				else
				{
					CodingTextResult.AppendLine();
				}
			}
			CodingTextResult.AppendLine();
			return string.Empty;
		}
			  
		public string MethodsText()
		{
			var errorResult = string.Empty;
			var Methods = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Method, true) == 0);

			if (IsExist(Methods) is false) return string.Empty;

			foreach (var classDetailInfo in ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Method, true) == 0))
			{
				if (string.Compare(ClassInfo.AccessModifier, Constants.AccessModifierDefault, true) == 0)
				{
					CodingTextResult.Append("\t".PadRight(spaceCount) + $"{ClassInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append("\t".PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.AppendLine($"{classDetailInfo.DataType} {classDetailInfo.MemberName}()");
				CodingTextResult.AppendLine("\t".PadRight(spaceCount) + "{");
				CodingTextResult.AppendLine();
				CodingTextResult.Append("\t".PadRight(spaceCount) + "}");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					CodingTextResult.AppendLine($"// {classDetailInfo.Comment}");
				}
				else
				{
					CodingTextResult.AppendLine();
				}
			}
			CodingTextResult.AppendLine();
			return string.Empty;
		}

		private bool IsExist(IEnumerable<ClassDetailInfoModel> memberTypes)
		{
			if (memberTypes.Count() == 0)
			{
				return false;
			}
			return true;
		}
	}
}
