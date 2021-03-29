using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Sample.Arc
{
	public class Model : ObservableBase
	{
		private string name;
		public string Name
		{
			get { return name; }
			set { Set(ref name, value); }
		}

		private int age;
		public int Age
		{
			get { return age; }
			set { Set(ref age, value); }
		}

		private int income;
		public int Income
		{
			get { return income; }
			set { Set(ref income, value); }
		}

		private string className;
		public string ClassName
		{
			get { return className; }
			set { Set(ref className, value); }
		}
	}
}

