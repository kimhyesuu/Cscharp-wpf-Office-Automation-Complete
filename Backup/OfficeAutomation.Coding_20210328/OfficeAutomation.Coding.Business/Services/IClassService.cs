using System.Collections.Generic;

namespace OfficeAutomation.Coding.Business.Services
{
	public interface IClassService<TModel>
	{
		IEnumerable<TModel> GetAll();
		bool CanDoAdd(TModel model);
		void AddRange(List<TModel> models);
		bool CanDoRemove(TModel model);
		bool CanDoSearch(List<TModel> models);
		bool CanDoUpdate(TModel model);
		bool CanDoClear();
	}
}