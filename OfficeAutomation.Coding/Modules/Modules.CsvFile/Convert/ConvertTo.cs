using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Modules.CsvFile.Convert
{
	public class ConvertTo
	{
		private const int spaceCount = 4;

		private ClassInfoModel ClassInfo							 { get; set; }
		private List<ClassDetailInfoModel> ClassDetailInfos { get;      }
		private StringBuilder CodingTextResult					 { get;      }

		public ConvertTo()
		{
			ClassDetailInfos = new List<ClassDetailInfoModel>();
			CodingTextResult = new StringBuilder();
		}

		public string  Initialize(ClassInfoModel classInfo, IEnumerable<ClassDetailInfoModel> classDetailInfos)
		{
			var result = IsCompability(classDetailInfos);
			if (result != string.Empty)
			{
				return result;
			}

			ClassInfo = new ClassInfoModel()
			{
				SequenceNumber = classInfo.SequenceNumber,
				AccessModifier = classInfo.AccessModifier.Trim(),
				ClassType      = classInfo.ClassType.Trim(),
				ClassName      = ToCodingStyle(classInfo.ClassName)
			};

			foreach (var classDetailInfo in classDetailInfos)
			{
				ClassDetailInfos.Add(new ClassDetailInfoModel()
				{
					AccessModifier = classDetailInfo.AccessModifier.Trim(),
					MemberName = ToCodingStyle(classDetailInfo.MemberName),
					MemberType = classDetailInfo.MemberType.Trim(),
					DataType = classDetailInfo.DataType.Trim(),
					Comment = classDetailInfo.Comment is null ? string.Empty : classDetailInfo.Comment.Trim()
				});
			}

			return string.Empty;
		}

		public void    Reset()
		{
			ClassInfo = null;
			ClassDetailInfos.Clear();
			CodingTextResult.Clear();
		}
						   
		public string  Result()
		{
			// 로그 데이터가 있으면 로그로 전달하자 
			return CodingTextResult.ToString();
		}
						   
		public void    StartText()
		{
			var text = string.Empty;

			CodingTextResult.Append(string.Empty.PadRight(5));
			if (string.Compare(ClassInfo.AccessModifier, "public", true) == 0)
			{
				CodingTextResult.Append($"{ClassInfo.AccessModifier} " );
			}

			if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeDefault, true) == 0)
			{
				CodingTextResult.AppendLine($"class {ClassInfo.ClassName}");
			}
			else
			{
				CodingTextResult.AppendLine($"{ClassInfo.ClassType} class {ClassInfo.ClassName}");
			}

			CodingTextResult.AppendLine(string.Empty.PadRight(5) + "{");
		}
						   
		public void    EndText()
		{
			var text = string.Empty.PadRight(5) + "}";
			CodingTextResult.AppendLine(text);
			CodingTextResult.AppendLine();
			CodingTextResult.AppendLine();
			CodingTextResult.AppendLine();
		}
						   
		public string  ConstructorText()
		{
			var result = string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0 ? true : false;
			if (result is false)
			{
				CodingTextResult.AppendLine( string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"public {ClassInfo.ClassName}()");
				CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "{");
				CodingTextResult.AppendLine();
				CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "}");
				CodingTextResult.AppendLine();
			}
			return string.Empty;
		}
						   
		public string  FieldsText()
		{
			var errorResult = string.Empty;
			var fields = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Field, true) == 0);

			if (IsExist(fields) is false) return string.Empty;

			foreach (var classDetailInfo in fields)
			{
				// void 검사
				if (string.Compare(classDetailInfo.DataType, "void", true) == 0)
				{
					errorResult = $"void :{classDetailInfo.MemberName}가 Method Type이 아닙니다.";
					return errorResult;
				}

				if (string.Compare(ClassInfo.AccessModifier, "public", true) == 0)
				{
					CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"private ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.Append($"{classDetailInfo.DataType}  _{FirstCharToLower(classDetailInfo.MemberName)};");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					CodingTextResult.AppendLine($"  // {classDetailInfo.Comment}");
				}
				else
				{
					CodingTextResult.AppendLine();
				}
			}

			CodingTextResult.AppendLine();
			return string.Empty;
		}
						   
		public string  PropertiesText()
		{
			var errorResult = string.Empty;
			var Properties = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Property, true) == 0);

			if (IsExist(Properties) is false) return string.Empty;

			foreach (var classDetailInfo in Properties)
			{
				// void 검사
				if (string.Compare(classDetailInfo.DataType, "void", true) == 0)
				{
					errorResult = $"void :{classDetailInfo.MemberName}가 Method Type이 아닙니다.";
					return errorResult;
				}

				if (string.Compare(ClassInfo.AccessModifier, "public", true) == 0)
				{
					CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"private ");
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
					CodingTextResult.AppendLine($"  // {classDetailInfo.Comment}");
				}
				else
				{
					CodingTextResult.AppendLine();
				}
			}
			CodingTextResult.AppendLine();
			return string.Empty;
		}
						   
		public string  MethodsText()
		{
			var errorResult = string.Empty;
			var Methods = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Method, true) == 0);

			if (IsExist(Methods) is false) return string.Empty;

			foreach (var classDetailInfo in ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Method, true) == 0))
			{
				if (string.Compare(ClassInfo.AccessModifier, "public", true) == 0)
				{
					CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");
				}
				else
				{
					CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"private ");
				}

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					CodingTextResult.Append($"{ClassInfo.ClassType} ");
				}

				CodingTextResult.AppendLine($"{classDetailInfo.DataType} {classDetailInfo.MemberName}()");
				CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "{");

				// void 검사
				if (string.Compare(classDetailInfo.DataType, "void", true) == 0)
				{
					CodingTextResult.AppendLine();
				}
				else
				{
					var dataTypes = new List<string>() { "string", "bool", "byte", "char", "decimal", "double", "float", "int", "long", "sbyte", "short", "uint", "ulong", "ushort", };
					var flag = false;
					foreach (var dataType in dataTypes)
					{
						if (string.Compare(classDetailInfo.DataType, dataType, true) == 0)
						{
							flag = true;
						}
					}

					if (flag is false)
					{
						CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + $"var obj = new {classDetailInfo.DataType}();");
						CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + "return obj;");
					}
					else
					{
						CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + $"{classDetailInfo.DataType} obj; // {classDetailInfo.DataType}에 맞는 value 설정할 것");
						CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + "return obj;");
					}
				}

				CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "}");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					CodingTextResult.AppendLine($"  // {classDetailInfo.Comment}");
					CodingTextResult.AppendLine();
				}
				else
				{
					CodingTextResult.AppendLine();
					CodingTextResult.AppendLine();
				}
			}
			return string.Empty;
		}
						   
		public string  ConstantsText()
		{
			var errorResult = string.Empty;
			var constants = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.constant, true) == 0);

			if (IsExist(constants) is false) return string.Empty;

			foreach (var classDetailInfo in constants)
			{
				// void 검사
				if (string.Compare(classDetailInfo.DataType, "void", true) == 0)
				{
					errorResult = $"void :{classDetailInfo.MemberName}가 Method Type이 아닙니다.";
					return errorResult;
				}

				CodingTextResult.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} const ");

				var dataTypes = new List<string>() { "string", "bool", "byte", "char", "decimal", "double", "float", "int", "long", "sbyte", "short", "uint", "ulong", "ushort", };
				var flag = false;
				foreach (var dataType in dataTypes)
				{
					if (string.Compare(classDetailInfo.DataType, dataType, true) == 0)
					{
						flag = true;
					}
				}

				if (flag is true)
				{

					CodingTextResult.Append($"{classDetailInfo.DataType} {classDetailInfo.MemberName} =  ;");
				}
				else
				{
					return $"{classDetailInfo.MemberName}이 Constant Data Type이 아닙니다.";
				}

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					CodingTextResult.AppendLine($"  // {classDetailInfo.Comment}");
				}
				else
				{
					CodingTextResult.AppendLine();
				}
			}
			CodingTextResult.AppendLine();
			return string.Empty;
		}

		private string ToCodingStyle(string memberName)
		{
			var result = memberName.Trim();

			result = CanDivideFromWhiteSpace(result) is true  ? GetCodingStyle(result) : FirstCharToUpper(result); 

			return result;
		}

		private string GetCodingStyle(string name)
		{
			var result = string.Empty;
			var list = name.Split(' ');

			for (int i = 0; i < list.Length; i++)
			{
				result += FirstCharToUpper(list[i]); ;
			}

			return result;
		}

		private bool   CanDivideFromWhiteSpace(string name)
		{
			var list = name.ToCharArray();

			foreach (var charResult in list)
			{
				if (charResult == ' ')
				{
					return true;
				}
			}

			return false;
		}

		private string IsCompability(IEnumerable<ClassDetailInfoModel> classDetailInfos)
		{
			var duplicatedMemberName = CheckingDuplication(classDetailInfos);
			if (duplicatedMemberName != string.Empty) return duplicatedMemberName;

			var value = CheckingWhitespace(classDetailInfos);
			if (value != string.Empty) return value;

			var spacialText = ResultOfSpacialText(classDetailInfos);
			if (spacialText != string.Empty) return spacialText;

			return string.Empty;
		}

		private string ResultOfSpacialText(IEnumerable<ClassDetailInfoModel> classDetailInfos)
		{
			string str = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";

			foreach (var classDetailInfo in classDetailInfos)
			{
				var rex = new Regex(str);
				var result = rex.IsMatch(classDetailInfo.MemberName);

				if (result is true)
					return $"{classDetailInfo.MemberName}에 특수문자가 들어갔습니다. 제외특수문자 : " + @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
			}
			return string.Empty;
		}

		private string CheckingWhitespace(IEnumerable<ClassDetailInfoModel> classDetailInfos)
		{
			foreach (var classDetailInfo in classDetailInfos)
			{
				if (classDetailInfo.MemberName is null &&
					classDetailInfo.MemberType is null &&
					classDetailInfo.AccessModifier is null &&
					classDetailInfo.DataType is null)
				{
					return "Member 자제가 공란인 행이 있습니다.";
				}
				else if (classDetailInfo.AccessModifier is null)
				{
					return "Access Modifier이 공란인 행이 있습니다";
				}
				else if (classDetailInfo.DataType is null)
				{
					return "DataType이 공란인 행이 있습니다";
				}
				else if (classDetailInfo.MemberName is null)
				{
					return "Member Name이 공란인 행이 있습니다.";
				}
				else if (classDetailInfo.MemberType is null)
				{
					return "Member Type이 공란인 행이 있습니다";
				}
			}
			return string.Empty;
		}

		private string CheckingDuplication(IEnumerable<ClassDetailInfoModel> classDetailInfos)
		{
			var doublicates = classDetailInfos.GroupBy(classDetailInfo => classDetailInfo.MemberName)
									.Where(classDetailInfo => classDetailInfo.Count() > 1).Select(result => result.Key);

			if (doublicates.Any() == true)
			{
				var duplicatredMemberName = doublicates.FirstOrDefault();
				return $"Member Name에 {duplicatredMemberName}(이)가 중복되었습니다.";
			}

			return string.Empty;
		}

		private bool   IsExist(IEnumerable<ClassDetailInfoModel> memberTypes)
		{
			if (memberTypes.Count() == 0)
			{
				return false;
			}
			return true;
		}

		private string GetResultOfRemovingSpacialText(string result) => Regex.Replace(result, @"[^a-zA-Z0-9가-힣_]", "", RegexOptions.Singleline);

		private string FirstCharToUpper(string input) => input.First().ToString().ToUpper() + input.Substring(1);

		private string FirstCharToLower(string input) => input.First().ToString().ToLower() + input.Substring(1);

	}
}


