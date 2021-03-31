using OfficeAutomation.Coding.Core;
using Prism.Events;
using Prism.Mvvm;

namespace Modules.Coding.ViewModels
{
	public class CodingInfoViewModel : BindableBase
	{
		private string _result;
		public  string Result
		{
			get { return _result; }
			set { SetProperty(ref _result, value); }
		}

		public CodingInfoViewModel(IEventAggregator eventAggregator)
		{
			eventAggregator.GetEvent<SendConvertedMessage>().Subscribe(MessageReceived);
		}

		private void MessageReceived(string paremeters)
		{
			Result = paremeters;
		}
	}
}
