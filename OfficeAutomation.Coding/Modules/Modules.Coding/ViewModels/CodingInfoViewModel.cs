using OfficeAutomation.Coding.Core;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace Modules.Coding.ViewModels
{
	public class CodingInfoViewModel : BindableBase
	{
		public CodingInfoViewModel(IEventAggregator eventAggregator)
		{
			eventAggregator.GetEvent<SendConvertedMessage>().Subscribe(MessageReceived);
		}

		private void MessageReceived(string paremeter)
		{
			throw new NotImplementedException();
		}
	}
}
