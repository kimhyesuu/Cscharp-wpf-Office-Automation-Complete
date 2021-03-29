using OfficeAutomation.Coding.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Coding.Business.Services
{
	public class SequencingService
	{	
		public static ObservableCollection<T> SetCollectionSequence<T>(ObservableCollection<T> targetCollection) where T : ISequencedObject
		{
			// Initialize
			var sequenceNumber = 1;

			// Resequence
			foreach (ISequencedObject sequencedObject in targetCollection)
			{
				sequencedObject.SequenceNumber = sequenceNumber;
				sequenceNumber++;
			}

			// Set return value
			return targetCollection;
		}
	}
}
