namespace OfficeAutomation.Coding.Core
{
	using Prism.Events;
	using System.Collections.Generic;

	public class SendLog				  : PubSubEvent<string>		   { }
																			  
	public class SendPreviewMessage : PubSubEvent<string>		   { }

	public class SendSavingMessages : PubSubEvent<List<object>> { }

}

