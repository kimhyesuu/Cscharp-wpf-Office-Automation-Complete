using System;
using System.Windows.Threading;

namespace OfficeAutomation.Coding.Business.Managers
{
	public static class TimerManager
	{
		public static DispatcherTimer CheckReadedCsvFileTimer { get; set; } = new DispatcherTimer();
	}
}
