using OfficeAutomation.Coding.Core;
using System.Collections.ObjectModel;

namespace OfficeAutomation.Coding.Business.Services
{
	public class SequencingService
	{	
		public static ObservableCollection<T> SetCollectionSequence<T>(ObservableCollection<T> targetCollection) where T : ISequencedObject
		{
			var sequenceNumber = 1;

			foreach (ISequencedObject sequencedObject in targetCollection)
			{
				sequencedObject.SequenceNumber = sequenceNumber;
				sequenceNumber++;
			}

			return targetCollection;
		}
	}
}
