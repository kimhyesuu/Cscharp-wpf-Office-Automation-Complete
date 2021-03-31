using System.Collections.Generic;

namespace OfficeAutomation.Coding.Business.Services
{
	public interface ISettingTypeService
	{
		List<string> GetAccessModifiers();
		List<string> GetDataTypes();
		List<string> GetClassTypes();
		List<string> GetMemberTypes();		
	}
}