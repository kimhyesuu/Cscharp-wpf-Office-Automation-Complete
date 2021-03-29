namespace OfficeAutomation.Coding.Core
{
	using Prism.Events;
	using System;
	using System.Collections.Generic;

	public class SendCsvFileList : PubSubEvent<IEnumerable<object>>
	{
		public void Publish(object classInfoList)
		{
			throw new NotImplementedException();
		}
	}

	public class SendUpdatedList : PubSubEvent<string> { }
}
