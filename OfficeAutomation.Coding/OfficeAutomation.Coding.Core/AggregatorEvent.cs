namespace OfficeAutomation.Coding.Core
{
	using Prism.Events;

	public class SendLog					 : PubSubEvent<string>	   { }

	public class SendConvertedMessage : PubSubEvent<string>		{ }

}
