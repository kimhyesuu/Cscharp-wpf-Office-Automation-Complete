using OfficeAutomation.Coding.Repository;
using System.Collections.Generic;

namespace OfficeAutomation.Coding.Business.Services
{
	public class SettingTypeService : ISettingTypeService
	{
		private ISettingTypeRepository _settingTypeRepository;

		public SettingTypeService()
		{
			_settingTypeRepository = new SettingTypeRepository();
		}

		public List<string> GetAccessModifiers()
		{
			return _settingTypeRepository.AccessModifiers;
		}

		public List<string> GetDataTypes()
		{
			return _settingTypeRepository.DataTypes;
		}

		public List<string> GetClassTypes()
		{
			return _settingTypeRepository.ClassTypes;
		}

		public List<string> GetMemberTypes()
		{
			return _settingTypeRepository.MemberTypes;
		}
	}
}
