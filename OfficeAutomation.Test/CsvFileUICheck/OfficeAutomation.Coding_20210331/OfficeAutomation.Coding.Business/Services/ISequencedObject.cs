using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Coding.Business.Services
{
	public interface ISequencedObject
	{
		int SequenceNumber { get; set; }
	}
}
