using System.Windows;

namespace OfficeAutomation.Coding.Core
{
	public static class Message
	{
		public static string MemberNotHaveAllData							 => "Member 자체가 공란인 행이 있습니다."	;
		public static string MemberNotHaveAccessModifier				 => "Access Modifier이 공란인 행이 있습니다";
		public static string MemberNotHaveDataType						 => "DataType이 공란인 행이 있습니다";
		public static string MemberNotHaveMemberName						 => "Member Name이 공란인 행이 있습니다.";
		public static string MemberNotHaveMemberType						 => "Member Type이 공란인 행이 있습니다" ;
		public static string ClassDetailInfosNotData						 => "클래스 데이터가 없습니다.";
		public static string BaseClassIsNotPublicAccessModifier		 => "상속한 클래스 한정자가 public이 아닙니다";
		public static string ClassDetailInfosNoExistOrNewTypeIsEmpty => "클래스 세부 사항 목록이 없거나 새로운 타입이 빈 공란입니다.";
		public static string DeleteSpecialText								 => "특수문자 제거해주세요.(제외한 특수문자 : <, >)";
		public static string AddingDataTypeDuplicated					 => "추가할 Data Type이 중복되었습니다.";

		public static string ThisMemberTypeIsNotVoidType(string memberName)			 => $"void :{memberName}가 Method Type이 아닙니다.";
																											 
		public static string MemberHaveSpecialText(string member)						 => $"{member}에 특수문자가 들어갔습니다.";

		public static string DuplicatedMemberNameExist(string duplicatedMemberName) => $"Member Name에 {duplicatedMemberName}(이)가 중복되었습니다.";

		public static string WrongAccessModifier(string accessModifier)				 => $"AccessModifier {accessModifier}(이)가 올바르지 않습니다.";

		public static string DifferentMemberType(string baseMember, string selectedMember, string selectedClassName)
		=> $"{baseMember}(base class)와 {selectedMember}({selectedClassName} class)(와)과 타입이 다릅니다.";

		public static MessageBoxResult InfoMessage  (string Message)
		{
			var result = MessageBox.Show(Message,"정보",MessageBoxButton.OKCancel);

			if(result == MessageBoxResult.OK)
			{
				return MessageBoxResult.OK;
			}

			return MessageBoxResult.No;
		}

		public static void				 InfoOKMessage(string Message)
		{
			MessageBox.Show(Message, "정보", MessageBoxButton.OK);
		}

		public static MessageBoxResult InfoOkCancelMessage(string Message)
		{
			var result = MessageBox.Show(Message, "정보", MessageBoxButton.OKCancel);

			if (result == MessageBoxResult.OK)
			{
				return MessageBoxResult.OK;
			}

			return MessageBoxResult.No;
		}

	}
}
