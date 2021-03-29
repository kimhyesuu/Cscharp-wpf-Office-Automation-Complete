using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Coding.Business.Models
{
	public class ClassInfoModel : ModelBase
	{
		private string _classType;

		public string ClassType
		{
			get { return _classType; }
			set { SetProperty(ref _classType, value); }
		}

		public ClassInfoModel() { }

		//public string ClassType { get; set; }
	}
}
