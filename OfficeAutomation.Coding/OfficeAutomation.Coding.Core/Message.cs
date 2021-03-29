using System.Windows;

namespace OfficeAutomation.Coding.Core
{
	public static class Message
	{
		public static MessageBoxResult InfoMessage(string Message)
		{
			var result = MessageBox.Show(Message,"정보",MessageBoxButton.OKCancel);

			if(result == MessageBoxResult.OK)
			{
				return MessageBoxResult.OK;
			}
			return MessageBoxResult.No;
		}
	}
}
