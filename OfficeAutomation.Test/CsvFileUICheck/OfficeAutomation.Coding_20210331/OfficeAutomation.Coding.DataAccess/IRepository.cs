namespace OfficeAutomation.Coding.Repository
{
	using System.Collections.Generic;

	public interface IRepository<TModel>
	{
		IEnumerable<TModel> GetAll();
		bool Add(TModel model);
		bool Remove(TModel model);
		bool Search(List<TModel> models);
		bool Update(TModel model);
		bool Clear();
		int  Count();
	}
}