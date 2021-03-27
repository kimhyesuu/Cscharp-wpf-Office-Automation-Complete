using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OfficeAutomation.Sample.Arc
{
	public abstract class ObservableBase : INotifyPropertyChanged
	{
		public void Set<TValue>(ref TValue field, TValue newValue,
									[CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<TValue>.Default.Equals(field, default(TValue)) || !field.Equals(newValue))

			{
				field = newValue;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}