using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OfficeAutomation.Coding.Utility.Commons
{
	public class ObservableObject : INotifyPropertyChanged
	{
		protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
		{
			if (IsEqual(field, default(T)) == false|| field.Equals(newValue) == true)
			{
				return false;
			}

			field = newValue;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private bool IsEqual<T>(T field, T newValue)
		{
			return EqualityComparer<T>.Default.Equals(field, newValue);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
