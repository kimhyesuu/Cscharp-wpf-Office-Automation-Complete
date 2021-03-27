using System.Collections.Generic;

namespace OfficeAutomation.Coding.Business.Services
{
	public interface IClassService<TModel>
	{
		IEnumerable<TModel> GetAll();
		bool Add(TModel model);
		bool Remove(TModel model);
		bool Search(List<TModel> models);
		bool Update(TModel model);
	}
}