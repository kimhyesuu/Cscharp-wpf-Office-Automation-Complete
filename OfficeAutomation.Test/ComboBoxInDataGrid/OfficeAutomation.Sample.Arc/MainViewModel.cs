using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfficeAutomation.Sample.Arc
{
	public class RelayCommand : ICommand
	{
		readonly Action<object> _execute;
		readonly Predicate<object> _canExecute;

		public RelayCommand(Action<object> execute)
				: this(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null ? true : _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}

	public class MainViewModel
	{

		public ICommand SendSelectedClassInfoCommand { get; set; }

		public MainViewModel()
		{
			SendSelectedClassInfoCommand = new RelayCommand((o) => Send(o));
			Mock();
		}

		private void Send(object o)
		{
			throw new NotImplementedException();
		}

		private void Mock()
		{
			for (int i = 20; i < 120; i++)
			{
				AgeRange.Add(i);
			}

			for (int i = 0; i < 100; i++)
			{
				IncomeRange.Add(10000 + 10 * i);
			}

			for (int i = 0; i < 100; i++)
			{
				People.Add(new Model
				{
					Name = $"Person {i}",
					Age = 85,
					Income = 10000,
					ClassName = "Good"
					// Default값을 넣어줘야겠네
					//Age = AgeRange[rand.Next(0, AgeRange.Count - 1)],
					//Income = IncomeRange[rand.Next(0, IncomeRange.Count - 1)]
				});
			}

			Names = new ObservableCollection<string>(People.Select(x => x.Name));
		}	

		private Random rand = new Random();

		public ObservableCollection<Model> People { get; }
			 = new ObservableCollection<Model>();

		public ObservableCollection<int> AgeRange { get; }
			 = new ObservableCollection<int>();

		public ObservableCollection<int> IncomeRange { get; }
			 = new ObservableCollection<int>();

		public ObservableCollection<string> Names { get; private set; }
	}
}
