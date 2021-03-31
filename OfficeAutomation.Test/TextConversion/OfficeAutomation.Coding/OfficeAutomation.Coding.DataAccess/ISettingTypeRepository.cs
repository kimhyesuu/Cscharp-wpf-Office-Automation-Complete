using System.Collections.Generic;

namespace OfficeAutomation.Coding.Repository
{
	public interface ISettingTypeRepository
	{
		List<string> AccessModifiers { get; }
		List<string> ClassTypes      { get; }
		List<string> DataTypes       { get; }
		List<string> MemberTypes     { get; }
	}
}