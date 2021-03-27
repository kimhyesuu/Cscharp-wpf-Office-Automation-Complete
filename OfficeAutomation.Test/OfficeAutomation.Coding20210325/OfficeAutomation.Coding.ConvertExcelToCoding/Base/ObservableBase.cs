using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OfficeAutomation.Coding.ConvertExcelToCoding.Base
{
	public abstract class ObservableBase : INotifyPropertyChanged
	{
		public void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(field, default(T)) || !field.Equals(newValue))
			{
				field = newValue;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
