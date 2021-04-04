using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Modules.CsvFile.Convert
{
	public enum Inheritance
	{
		None   ,
		Parent ,
		Child  ,
	}

	public class ConvertTo
	{
		private const int spaceCount = 4;

		private  Inheritance				         InheritanceFlag		{ get; set; }
		private  ClassInfoModel			         BaseClassInfo		   { get; set; }
		private	ClassInfoModel			         ClassInfo			   { get; set; }

		private  List<ClassDetailInfoModel>    AllClassDetailInfos  { get; set; }
		private  List<ClassDetailInfoModel>    BaseClassDetailInfos { get; set; }
		private  List<ClassDetailInfoModel>    ClassDetailInfos     { get; set; }
		private  StringBuilder					   CodingTextResult	   { get;      }

		public ConvertTo()
		{
			ClassDetailInfos	   = new List<ClassDetailInfoModel>();
			AllClassDetailInfos  = new List<ClassDetailInfoModel>();
			BaseClassDetailInfos = new List<ClassDetailInfoModel>();
			CodingTextResult	   = new StringBuilder();
		}

		private Inheritance ParentOrChild(ClassInfoModel baseClassInfo, ClassInfoModel classInfo)
		{
			if (baseClassInfo.ClassType.Equals(Constants.ClassTypeStatic) is false &&
			    baseClassInfo.ClassType.Equals(ClassTypeSealed)			  is false &&
				 baseClassInfo.ClassName.Equals(classInfo.ClassName)		  is true)
			{
				return Inheritance.Parent;
			}		
				return Inheritance.Child;
		}

		// 완료
		internal string Initialize(ClassInfoModel baseClassInfo, ClassInfoModel selectedClassInfo, IEnumerable<ClassDetailInfoModel> detailedInfos)
		{
			var allClassDetailedInfos  = detailedInfos.ToList();
			if (detailedInfos is null) return "데이터가 없습니다.";
			else								SaveAllData(allClassDetailedInfos);	

			var receivedClassInfo	   = selectedClassInfo;
			var selectedDetailedInfos  = allClassDetailedInfos.Where(o => o.ClassName.Equals(receivedClassInfo.ClassName)     is true).ToList();

			var receivedBaseClassInfo  = baseClassInfo;
			var baseClassDetailedInfos = allClassDetailedInfos.Where(o => o.ClassName.Equals(receivedBaseClassInfo.ClassName) is true).ToList();

			var result = IsCompability(selectedDetailedInfos);
			if (result != string.Empty)
			{
				return result;
			}

			if (IsDataEixst(receivedBaseClassInfo, baseClassDetailedInfos, receivedClassInfo, selectedDetailedInfos) is true)
			{
				InitializeSelectedClass(receivedClassInfo, selectedDetailedInfos);
				InitializeBase(baseClassInfo, baseClassDetailedInfos);
				InheritanceFlag = ParentOrChild(baseClassInfo, receivedClassInfo);
			}
			else
			{
				InitializeSelectedClass(receivedClassInfo, selectedDetailedInfos.ToList());
				InheritanceFlag = Inheritance.None;
			}
			return string.Empty;
		}

		private void SaveAllData(List<ClassDetailInfoModel> detailedInfos)
		{
			AllClassDetailInfos.Clear();

			foreach (var detailedInfo in detailedInfos)
			{
				AllClassDetailInfos.Add(new ClassDetailInfoModel()
				{
					ClassName		= detailedInfo.ClassName.Trim(),
					AccessModifier = detailedInfo.AccessModifier.Trim(),
					MemberName     = ToCodingStyle(detailedInfo.MemberName),
					MemberType     = detailedInfo.MemberType.Trim(),
					DataType       = detailedInfo.DataType.Trim(),
					Comment        = detailedInfo.Comment is null ? string.Empty : detailedInfo.Comment.Trim()
				});
			}
		}

		// 완료	
		public void    Reset()
		{
			ClassInfo = null;
			ClassDetailInfos.Clear();
			CodingTextResult.Clear();
		}
					   
		public string  Result()
		{
			return CodingTextResult.ToString();
		}

		#region Start End Constructor Text
		public  string StartText()
		{
			var errorResult          = string.Empty;
			var startTextBuilder     = CodingTextResult;

			var flag			          = InheritanceFlag;
			var baseClassInfo 		 = BaseClassInfo;
			
			CodingTextResult.Append(string.Empty.PadRight(5));

			switch (flag)
			{
				case Inheritance.None:   errorResult = WriteNoneStartText  (ref startTextBuilder,					 ClassInfo);	  break;
				case Inheritance.Parent: errorResult = WriteParentStartText(ref startTextBuilder,					 ClassInfo);     break;
				case Inheritance.Child:  errorResult = WriteChildStartText (ref startTextBuilder, baseClassInfo, ClassInfo);	  break;
			}

			startTextBuilder.AppendLine(string.Empty.PadRight(5) + "{");
			return errorResult;
		}

		public  void   EndText()
		{
			var text = string.Empty.PadRight(5) + "}";
			CodingTextResult.AppendLine(text);
			CodingTextResult.AppendLine();
		}

		private string WriteChildStartText(ref StringBuilder startTextBuilder, ClassInfoModel baseClassInfo, ClassInfoModel classInfo)
		{
			if (BaseClassInfo.AccessModifier.Equals("public") is true)
			{
				startTextBuilder.Append($"{ClassInfo.AccessModifier} ");
			}
			else if (ClassInfo.AccessModifier.Equals("public") is false)
			{
				startTextBuilder.Append($"{ClassInfo.AccessModifier} ");
			}
			else
			{
				return "상속한 클래스 한정자가 public이 아닙니다";
			}

			if (ClassInfo.ClassType.Equals(Constants.ClassTypeDefault) is false)
			{
				startTextBuilder.Append($"{ClassInfo.ClassType} ");
			}

			if (baseClassInfo.ClassType.Equals(Constants.ClassTypeStatic) is false &&
				 baseClassInfo.ClassType.Equals(ClassTypeSealed) is false &&
				 baseClassInfo.ClassName.Equals(classInfo.ClassName) is false)
			{
				startTextBuilder.AppendLine($"class {ClassInfo.ClassName} : {baseClassInfo.ClassName}");
			}
			else
			{
				startTextBuilder.AppendLine($"class {ClassInfo.ClassName}");
			}

			return string.Empty;
		}

		private string WriteParentStartText(ref StringBuilder startTextBuilder, ClassInfoModel baseClassInfo)
		{
			if (baseClassInfo.AccessModifier.Equals("public") is true)
			{
				startTextBuilder.Append($"{BaseClassInfo.AccessModifier} ");
			}

			// class Type
			if (ClassInfo.ClassType.Equals(Constants.ClassTypeDefault) is false)
			{
				startTextBuilder.Append($"{ClassInfo.ClassType} ");
			}

			startTextBuilder.AppendLine($"class {ClassInfo.ClassName}");

			return string.Empty;
		}

		private string WriteNoneStartText(ref StringBuilder startTextBuilder, ClassInfoModel classInfo)
		{
			startTextBuilder.Append($"{ClassInfo.AccessModifier} ");

			if (ClassInfo.ClassType.Equals(Constants.ClassTypeDefault) is false)
			{
				startTextBuilder.Append($"{ClassInfo.ClassType} ");
			}
			startTextBuilder.AppendLine($"class {ClassInfo.ClassName}");
			return string.Empty;
		}

		public  string ConstructorText()
		{
			if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
			{
				return string.Empty;
			}
			
			CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"public {ClassInfo.ClassName}()");
			CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "{");
			CodingTextResult.AppendLine();
			CodingTextResult.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "}");
			CodingTextResult.AppendLine();
			return string.Empty;
		}
		#endregion

		#region FieldsText
		public const string DataTypeVoid = "void";
		public static string MessageThisTypeIsNotVoidType(string memberName) =>  $"void :{memberName}가 Method Type이 아닙니다.";

		public string  FieldsText()
		{
			var errorResult			 = string.Empty;
			var fieldsTextBuilder    = CodingTextResult;
			var flag						 = InheritanceFlag;
			var baseClassDetailInfos = BaseClassDetailInfos;
			var selectedClassInfo    = ClassInfo;
			var fields		          = ClassDetailInfos.Where(o => o.MemberType.ToLower().Equals(Constants.Field) is true);			 
			var voidOrNull				 = VoidType(fields); 

			if (IsExist(fields) is false) return string.Empty;

			if (voidOrNull      != null ) return MessageThisTypeIsNotVoidType(voidOrNull.MemberName); 

			switch (flag)
			{
				case Inheritance.None	: errorResult = WriteFieldsText		 (ref fieldsTextBuilder,							  fields); break;
				case Inheritance.Parent : errorResult = WriteParentFieldsText(ref fieldsTextBuilder, selectedClassInfo   , fields); break; 
				case Inheritance.Child  : errorResult = WriteChildFieldsText (ref fieldsTextBuilder, baseClassDetailInfos, fields); break;
			}

			return errorResult;
		}

		private string WriteFieldsText		(ref StringBuilder fieldsTextBuilder, IEnumerable<ClassDetailInfoModel> fields)
		{
			foreach (var classDetailInfo in fields)
			{
				WriteFieldText(ref fieldsTextBuilder, classDetailInfo);				
			}
			fieldsTextBuilder.AppendLine();
			return string.Empty;
		}

		private string WriteChildFieldsText	(ref  StringBuilder                    fieldsTextBuilder    ,
															  IEnumerable<ClassDetailInfoModel> baseClassDetailInfos ,
															  IEnumerable<ClassDetailInfoModel> fields					)
		{
			foreach (var classDetailInfo in fields)
			{
				fieldsTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount)
					+ $"{classDetailInfo.AccessModifier} ");
	
				fieldsTextBuilder.Append($"{classDetailInfo.DataType} _{FirstCharToLower(classDetailInfo.MemberName)};");

				if (classDetailInfo.Comment != string.Empty)
				{
					fieldsTextBuilder.Append($" //{classDetailInfo.Comment} ");
				}

				var flag = false;
				foreach (var baseClassDetailInfo in baseClassDetailInfos)
				{
					if (classDetailInfo.MemberName.Equals(baseClassDetailInfo.MemberName))
					{
						flag = true;
						fieldsTextBuilder.AppendLine("<use parent class>");
						break;
					}
				}

				if(flag is false)
				{
					fieldsTextBuilder.AppendLine();
				}
			}
			fieldsTextBuilder.AppendLine();
			return string.Empty;
		}

		private string WriteParentFieldsText(ref StringBuilder						   fieldsTextBuilder    , 
															  ClassInfoModel						   selectedClassInfo    , 
															  IEnumerable<ClassDetailInfoModel> fields				   )
		{
			var allClassDetailInfos = AllClassDetailInfos.Where(o => o.ClassName.Equals(BaseClassInfo.ClassName) is false);

			foreach(var ClassDetailInfo in fields)
			{
				fieldsTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) 
					+ $"{ClassDetailInfo.AccessModifier} ");

				fieldsTextBuilder.Append($"{ClassDetailInfo.DataType} _{FirstCharToLower(ClassDetailInfo.MemberName)};");

				if (ClassDetailInfo.Comment != string.Empty)
				{
					fieldsTextBuilder.Append($" //{ClassDetailInfo.Comment} ");
				}

				var flag = false;
				foreach (var allClassDetailInfo in allClassDetailInfos)
				{
					if (ClassDetailInfo.MemberName.Equals(allClassDetailInfo.MemberName))
					{
						flag = true;
						fieldsTextBuilder.AppendLine("<use child class>");
						break;
					}
				}

				if (flag is false)
				{
					fieldsTextBuilder.AppendLine();
				}
			}

			fieldsTextBuilder.AppendLine();
			return string.Empty;
		}

		private void   WriteFieldText			(ref StringBuilder fieldTextBuilder, ClassDetailInfoModel classDetailInfo)
		{
			fieldTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount)
				+ $"{classDetailInfo.AccessModifier} {classDetailInfo.DataType} _{FirstCharToLower(classDetailInfo.MemberName)};");

			if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
			{
				fieldTextBuilder.AppendLine($" // {classDetailInfo.Comment}");
			}
			else
			{
				fieldTextBuilder.AppendLine();
			}
		}
		#endregion

		#region PropertiesText

		public  string PropertiesText()
		{
			var errorResult		     = string.Empty;
			var propertiesTextBuilder = CodingTextResult;
			var flag						  = InheritanceFlag;
			var baseClassDetailInfos  = BaseClassDetailInfos;
			var selectedClassInfo     = ClassInfo;
			var Properties				  = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Property, true) == 0);
			var voidOrNull				  = VoidType(Properties); 

			if (IsExist(Properties) is false) return string.Empty;

			if (voidOrNull			   != null ) return MessageThisTypeIsNotVoidType(voidOrNull.MemberName);

			switch (flag)
			{
				case Inheritance.None  : errorResult = WritePropertiesText		 (ref propertiesTextBuilder,							   Properties); break;
				case Inheritance.Parent: errorResult = WriteParentPropertiesText(ref propertiesTextBuilder, selectedClassInfo   , Properties); break;
				case Inheritance.Child : errorResult = WriteChildPropertiesText (ref propertiesTextBuilder, baseClassDetailInfos, Properties); break;
			}

			propertiesTextBuilder.AppendLine();
			return errorResult;
		}

		public const string ClassTypeAbstract = "abstract";
		private string WritePropertiesText		 (ref StringBuilder propertiesTextBuilder, IEnumerable<ClassDetailInfoModel> properties)
		{
			foreach (var classDetailInfo in properties)
			{
				propertiesTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");

				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					propertiesTextBuilder.Append($"{ClassInfo.ClassType} ");
				}
				else if (string.Compare(ClassInfo.ClassType, ClassTypeAbstract, true) == 0)
				{
					propertiesTextBuilder.Append($"{ClassInfo.ClassType} ");
				}

				propertiesTextBuilder.Append($"{classDetailInfo.DataType} {classDetailInfo.MemberName} ");
				propertiesTextBuilder.Append("{ ");
				propertiesTextBuilder.Append("get; set;");
				propertiesTextBuilder.Append(" }");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					propertiesTextBuilder.AppendLine($"  // {classDetailInfo.Comment}");
				}
				else
				{
					propertiesTextBuilder.AppendLine();
				}
			}

			propertiesTextBuilder.AppendLine();
			return string.Empty;
		}
															 
		private string WriteChildPropertiesText (ref StringBuilder							propertiesTextBuilder , 
																   List<ClassDetailInfoModel>			baseClassDetailInfos  ,
																   IEnumerable<ClassDetailInfoModel> properties				 )
		{
			// 타입 
			foreach (var classDetailInfo in properties)
			{
				propertiesTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");

				// ClassInfo
				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					propertiesTextBuilder.Append($"{ClassInfo.ClassType} ");
				}
				else if (string.Compare(ClassInfo.ClassType, ClassTypeAbstract, true) == 0)
				{
					propertiesTextBuilder.Append($"{ClassInfo.ClassType} ");
				}
				else
				{
					foreach (var baseClassDetailInfo in baseClassDetailInfos)
					{
						if (classDetailInfo.MemberName.Equals(baseClassDetailInfo.MemberName))
						{
							if(classDetailInfo.MemberType.Equals(baseClassDetailInfo.MemberType) is true)
							{
								propertiesTextBuilder.Append("ovrride ");
							}
							else
							{
								return $"base class의 {classDetailInfo.MemberName}(와)과 타입이 다릅니다.";
							}
							break;
						}
					}
				}

				propertiesTextBuilder.Append($"{classDetailInfo.DataType} {classDetailInfo.MemberName} ");
				propertiesTextBuilder.Append("{ ");
				propertiesTextBuilder.Append("get; set;");
				propertiesTextBuilder.Append(" }");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					propertiesTextBuilder.AppendLine($"  // {classDetailInfo.Comment}");
				}
				else
				{
					propertiesTextBuilder.AppendLine();
				}
			}

			return string.Empty;
		}

		private string WriteParentPropertiesText(ref StringBuilder							 propertiesTextBuilder , 
																	ClassInfoModel							 selectedClassInfo	  , 
																	IEnumerable<ClassDetailInfoModel> properties				  )
		{
			var allClassDetailInfos = AllClassDetailInfos.Where(o => o.ClassName.Equals(BaseClassInfo.ClassName) is false);

			foreach (var classDetailInfo in properties)
			{
				propertiesTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");

				// ClassInfo
				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					propertiesTextBuilder.Append($"{ClassInfo.ClassType} ");
				}
				else if (string.Compare(ClassInfo.ClassType, ClassTypeAbstract, true) == 0)
				{
					propertiesTextBuilder.Append($"{ClassInfo.ClassType} ");
				}
				else
				{
					foreach (var allClassDetailInfo in allClassDetailInfos)
					{
						if (classDetailInfo.MemberName.Equals(allClassDetailInfo.MemberName))
						{
							propertiesTextBuilder.Append("virtual ");
							break;
						}
					}
				}

				propertiesTextBuilder.Append($"{classDetailInfo.DataType} {classDetailInfo.MemberName} ");
				propertiesTextBuilder.Append("{ ");
				propertiesTextBuilder.Append("get; set;");
				propertiesTextBuilder.Append(" }");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					propertiesTextBuilder.AppendLine($"  // {classDetailInfo.Comment}");
				}
				else
				{
					propertiesTextBuilder.AppendLine();
				}
			}

			return string.Empty;
		}
		#endregion

		#region MethodsText

		public string MethodsText()
		{
			var errorResult		    = string.Empty;
			var methodsTextBuilder   = CodingTextResult;
			var flag					    = InheritanceFlag;
			var baseClassDetailInfos = BaseClassDetailInfos;
			var selectedClassInfo    = ClassInfo;
			var Methods				    = ClassDetailInfos.Where(o => string.Compare(o.MemberType, Constants.Method, true) == 0);

			if (IsExist(Methods) is false) return string.Empty;

			switch (flag)
			{
				case Inheritance.None  : errorResult = WriteMethodsText			  (ref methodsTextBuilder, Methods);  break;
				case Inheritance.Parent: errorResult = WriteBaseParentMethodsText(ref methodsTextBuilder, selectedClassInfo,  Methods);  break;
				case Inheritance.Child : errorResult = WriteBaseChildMethodsText (ref methodsTextBuilder, baseClassDetailInfos, Methods);  break;
			}

			return errorResult;
		}

		private string WriteBaseChildMethodsText(ref StringBuilder                     methodsTextBuilder	 , 
																	List<ClassDetailInfoModel>        baseClassDetailInfos , 
																	IEnumerable<ClassDetailInfoModel> methods					 )
		{
			throw new NotImplementedException();
		}

		private string WriteBaseParentMethodsText(ref StringBuilder							  methodsTextBuilder , 
																	 ClassInfoModel						  selectedClassInfo  , 
																	 IEnumerable<ClassDetailInfoModel> methods			   )
		{
			throw new NotImplementedException();
		}

		private string WriteMethodsText(ref StringBuilder methodsTextBuilder, IEnumerable<ClassDetailInfoModel> methods)
		{
			foreach(var classDetailInfo in methods)
			{
				methodsTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + $"{classDetailInfo.AccessModifier} ");
				
				if (string.Compare(ClassInfo.ClassType, Constants.ClassTypeStatic, true) == 0)
				{
					methodsTextBuilder.Append($"{ClassInfo.ClassType} ");
				}

				// override
				// 여기서 가져와서 
				methodsTextBuilder.AppendLine($"{classDetailInfo.DataType} {classDetailInfo.MemberName}()");
				methodsTextBuilder.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "{");

				// void 검사
				if (string.Compare(classDetailInfo.DataType, DataTypeVoid, true) == 0)
				{
					methodsTextBuilder.AppendLine();
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

					// override면 다르게 해야대

					if (flag is false)
					{
						methodsTextBuilder.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + $"var obj = new {classDetailInfo.DataType}();");
						methodsTextBuilder.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + "return obj;");
					}
					else
					{
						methodsTextBuilder.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + $"{classDetailInfo.DataType} obj; // {classDetailInfo.DataType}에 맞는 value 설정할 것");
						methodsTextBuilder.AppendLine(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount + 3) + "return obj;");
					}
				}

				methodsTextBuilder.Append(string.Empty.PadRight(5) + string.Empty.PadRight(spaceCount) + "}");

				// Comment 검사
				if (string.IsNullOrWhiteSpace(classDetailInfo.Comment) == false)
				{
					methodsTextBuilder.AppendLine($" // {classDetailInfo.Comment}");
					methodsTextBuilder.AppendLine();
				}
				else
				{
					methodsTextBuilder.AppendLine();
					methodsTextBuilder.AppendLine();
				}

				methodsTextBuilder.AppendLine();
			}

			return string.Empty;
		}

		#endregion

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

		public string  ToCodingStyle(string memberName)
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

		public const string SpecialText = @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
		public static string MessageMemberHaveSpecialText(string member) => $"{member}에 특수문자가 들어갔습니다. 제외특수문자 : " + @"[~!@\#$%^&*\()\=+|\\/:;?""<>']";
		private string ResultOfSpacialText(IEnumerable<ClassDetailInfoModel> classDetailInfos)
		{
			string str = SpecialText;

			foreach (var classDetailInfo in classDetailInfos)
			{
				var rex = new Regex(str);
				var result = rex.IsMatch(classDetailInfo.MemberName);

				if (result is true)
					return MessageMemberHaveSpecialText(classDetailInfo.MemberName);
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

		private bool   IsDataEixst(ClassInfoModel baseClassInfo, List<ClassDetailInfoModel> baseClassDetailInfos, ClassInfoModel classInfo, List<ClassDetailInfoModel> classDetailInfos)
		{
			if(classInfo	  != null			   &&
				baseClassInfo != null		      &&
				classDetailInfos.Count       > 0 &&					 
				baseClassDetailInfos.Count   > 0  )
			{
				ClassInfo = null;
				ClassDetailInfos.Clear();
				BaseClassInfo = null;
				BaseClassDetailInfos.Clear();

				return true;
			}
			else if (classInfo != null && classDetailInfos.Count > 0)
			{
				ClassInfo = null;
				ClassDetailInfos.Clear();
				return false;
			}	
			return false;
		}

		public const string ClassTypeSealed = "sealed";

		private string InitializeSelectedClass(ClassInfoModel classInfo,     List<ClassDetailInfoModel> classDetailInfos)
		{
			ClassInfo = new ClassInfoModel()
			{
				SequenceNumber = classInfo.SequenceNumber,
				AccessModifier = classInfo.AccessModifier.Trim(),
				ClassType = classInfo.ClassType.Trim(),
				ClassName = ToCodingStyle(classInfo.ClassName)
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

		private string InitializeBase			  (ClassInfoModel baseClassInfo, List<ClassDetailInfoModel> baseClassDetailInfos)
		{		
			BaseClassInfo = new ClassInfoModel()
			{
				SequenceNumber = baseClassInfo.SequenceNumber,
				AccessModifier = baseClassInfo.AccessModifier.Trim(),
				ClassType = baseClassInfo.ClassType.Trim(),
				ClassName = ToCodingStyle(baseClassInfo.ClassName.Trim())
			};

			foreach (var baseClassDetailInfo in baseClassDetailInfos)
			{
				BaseClassDetailInfos.Add(new ClassDetailInfoModel()
				{
					ClassName = baseClassDetailInfo.ClassName.Trim(),
					AccessModifier = baseClassDetailInfo.AccessModifier.Trim(),
					MemberName = baseClassDetailInfo.MemberName.Trim(),
					MemberType = ToCodingStyle(baseClassDetailInfo.MemberType.Trim()),
					DataType = baseClassDetailInfo.DataType.Trim(),
					Comment = baseClassDetailInfo.Comment is null ? string.Empty : baseClassDetailInfo.Comment.Trim()				   
				});
			}

			return string.Empty;
		}

		private ClassDetailInfoModel VoidType(IEnumerable<ClassDetailInfoModel> membertypes) => membertypes.Where(o => o.DataType.Equals("void") is true).FirstOrDefault();

		private string GetResultOfRemovingSpacialText(string result)								 => Regex.Replace(result, @"[^a-zA-Z0-9가-힣_]", "", RegexOptions.Singleline);
																														 
		private string FirstCharToUpper(string input)													 => input.First().ToString().ToUpper() + input.Substring(1);
																														 
		private string FirstCharToLower(string input)													 => input.First().ToString().ToLower() + input.Substring(1);

	}
}


