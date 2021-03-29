namespace OfficeAutomation.Coding.Core
{
	using Prism.Events;
	using System.Collections.Generic;

	public class SendCsvFileList : PubSubEvent<IEnumerable<object>> { }

	public class SendUpdatedList : PubSubEvent<string> { }
}
