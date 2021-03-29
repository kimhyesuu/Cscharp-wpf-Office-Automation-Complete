namespace OfficeAutomation.Coding.Core
{
	using Prism.Events;

	public class SendCsvFileList : PubSubEvent<string[]>			{ }

	public class SendConvertedMessage : PubSubEvent<string>		{ }
}
