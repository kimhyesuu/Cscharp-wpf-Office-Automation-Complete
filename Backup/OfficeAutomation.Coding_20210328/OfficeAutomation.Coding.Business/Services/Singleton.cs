using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Coding.Business.Services
{
	public sealed class Singleton
	{
		private static ClassInfoRepository<ClassInfoModel> _classInfoIntances = null;

		public static ClassInfoRepository<ClassInfoModel> GetCurrentClassInfos
		{
			get
			{
				if (_classInfoIntances is null)
					_classInfoIntances = new ClassInfoRepository<ClassInfoModel>();
				return _classInfoIntances;
			}
		}

		private static ClassDetailInfoRepository<ClassInfoModel> _classDetailInfoIntances = null;

		public static ClassDetailInfoRepository<ClassInfoModel> GetCurrentClassDetailInfos
		{
			get
			{
				if (_classDetailInfoIntances is null)
					_classDetailInfoIntances = new ClassDetailInfoRepository<ClassInfoModel>();
				return _classDetailInfoIntances;
			}
		}
	}
}
