using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Repository;

namespace OfficeAutomation.Coding.Business.Services
{
	public sealed class Singleton
	{
		private static ClassInfoRepository<ClassInfoModel> _classInfoIntances = null;

		public  static ClassInfoRepository<ClassInfoModel> GetCurrentClassInfos
		{
			get
			{
				if (_classInfoIntances is null)
					_classInfoIntances = new ClassInfoRepository<ClassInfoModel>();
				return _classInfoIntances;
			}
		}

		private static ClassDetailInfoRepository<ClassDetailInfoModel> _classDetailInfoIntances = null;

		public  static ClassDetailInfoRepository<ClassDetailInfoModel> GetCurrentClassDetailInfos
		{
			get
			{
				if (_classDetailInfoIntances is null)
					_classDetailInfoIntances = new ClassDetailInfoRepository<ClassDetailInfoModel>();
				return _classDetailInfoIntances;
			}
		}
	}
}
