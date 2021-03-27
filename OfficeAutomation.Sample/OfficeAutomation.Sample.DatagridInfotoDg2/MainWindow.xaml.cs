using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OfficeAutomation.Sample.DatagridInfotoDg2
{
	public partial class MainWindow : Window
	{
		private const string CsvFolder = "\\OfficeAutomation.File";
		private const string CsvAndAllFileFilter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

		private string PreviousClassName { get; set; }
		private string CurrentClassName { get; set; }

		private ObservableCollection<ClassInfo> ClassInfos { get; }
		private ObservableCollection<ClassDetailInfo> ClassDetailInfos { get; set; }
		//private List<ClassInfo> ClassInfoRepository { get; set; }
		private List<ClassDetailInfo> ClassDetailInfoRepository { get; set; }
		private ICommand TestButtonCommand { get;  set; }


		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;
			ClassInfos = new ObservableCollection<ClassInfo>();

			//ClassInfoRepository = new List<ClassInfo>();
			ClassDetailInfoRepository = new List<ClassDetailInfo>();
			ClassDetailInfos = new ObservableCollection<ClassDetailInfo>();
			TestButtonCommand = new RelayCommand((o) => TestGetClassName(o));
		}

		private void TestGetClassName(object o)
		{
			if(ClassDetailInfos != null)
			{
				ClassDetailInfos = null;
				ClassDetailInfos = new ObservableCollection<ClassDetailInfo>();
			}
			var classInfo = o as ClassInfo;
			var selectedclassDetailInfos = ClassDetailInfoRepository;

			foreach (var selectedclassDetailInfo in selectedclassDetailInfos)
			{
				if(classInfo.ClassName == selectedclassDetailInfo.ClassName)
				{
					ClassDetailInfos.Add(new ClassDetailInfo()
					{
						ClassName = selectedclassDetailInfo.ClassName,
						AccessModifier = selectedclassDetailInfo.AccessModifier,
						DataType = selectedclassDetailInfo.DataType,
						MemberName = selectedclassDetailInfo.MemberName,
						MemberType = selectedclassDetailInfo.MemberType,
						Comment = selectedclassDetailInfo.Comment
					});
				}
			}

			Datagrid2.ItemsSource = ClassDetailInfos;

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var openFileDlg = new Microsoft.Win32.OpenFileDialog();

			openFileDlg.InitialDirectory = GetCsvPath();
			openFileDlg.Filter = CsvAndAllFileFilter;

			var result = openFileDlg.ShowDialog();

			if (result.HasValue is true && result.Value is true)
			{
				var selectedFilepath = openFileDlg.FileName;
				lbFilePath.Content = openFileDlg.InitialDirectory.ToString();
				ClassDetailInfoRepository = new List<ClassDetailInfo>(ReadCSV(selectedFilepath));
				Datagrid1.ItemsSource = ClassInfos;
			}
		}

		private string GetCsvPath()
		{
			string path = string.Empty;

			string workingDirectory = Environment.CurrentDirectory;
			path = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName + CsvFolder;

			return path;
		}

		public IEnumerable<ClassDetailInfo> ReadCSV(string fileName)
		{
			string[] lines = File.ReadAllLines(System.IO.Path.ChangeExtension(fileName, ".csv"));
			int ClassInfoCount = 1;

			return lines.Select(line =>
			{
				string[] data = line.Split(',');
				// null 값 혹은 string.Empty 넣자 
				CurrentClassName = data[0];

				if (string.Compare(CurrentClassName, PreviousClassName, true) != 0 &&
					 string.IsNullOrEmpty(data[0]) is false)
				{
					PreviousClassName = CurrentClassName;
					// Add로 하자 
					// ClassInfo
					ClassInfos.Add(new ClassInfo()
					{
						SequentialNumber = ClassInfoCount.ToString("00"),
						AccessModifier = "Public",
						ClassType = string.Empty,
						ClassName = data[0]
					});

					ClassInfoCount++;
				}

				return new ClassDetailInfo()
				{
					ClassName = data[0],
					AccessModifier = "private",
					DataType = "int",
					MemberName = data[1],
					MemberType = string.Empty,
					Comment = data[2]
				};
			});
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (ClassDetailInfos != null)
			{
				ClassDetailInfos = null;
				ClassDetailInfos = new ObservableCollection<ClassDetailInfo>();
			}
			//var classInfo = o as ClassInfo;
			// Ditinct로 줄이자 Create로 부시자 
			var selectedclassInfo = ClassInfos.Select(o => o.ClassName).Distinct();
			var selectedclassDetailInfos = ClassDetailInfoRepository;

			foreach (var selectedclassDetailInfo in selectedclassDetailInfos)
			{
				if (selectedclassInfo.FirstOrDefault() == selectedclassDetailInfo.ClassName)
				{
					ClassDetailInfos.Add(new ClassDetailInfo()
					{
						ClassName = selectedclassDetailInfo.ClassName,
						AccessModifier = selectedclassDetailInfo.AccessModifier,
						DataType = selectedclassDetailInfo.DataType,
						MemberName = selectedclassDetailInfo.MemberName,
						MemberType = selectedclassDetailInfo.MemberType,
						Comment = selectedclassDetailInfo.Comment
					});
				}
			}

			Datagrid2.ItemsSource = ClassDetailInfos;

		}
	}

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


	public class ClassInfo : ObservableBase
	{
		public const string Class = "class";

		private string sequentialNumber;
		public string SequentialNumber
		{
			get { return sequentialNumber; }
			set { SetProperty(ref sequentialNumber, value); }
		}

		private string accessModifier;
		public string AccessModifier
		{
			get { return accessModifier; }
			set { SetProperty(ref accessModifier, value); }
		}

		private string classType;
		public string ClassType
		{
			get { return classType; }
			set { SetProperty(ref classType, value); }
		}

		private string className;
		public string ClassName
		{
			get { return className; }
			set { SetProperty(ref className, value); }
		}

		//public string AccessModifier { get; set; }
		//public string ClassType { get; set; }
		//public string ClassName { get; set; }
	}


	public class ClassDetailInfo
	{
		public string ClassName { get; set; }
		public string AccessModifier { get; set; }
		public string DataType { get; set; }
		public string MemberName { get; set; }
		public string MemberType { get; set; }
		public string Comment { get; set; }
	}
}
