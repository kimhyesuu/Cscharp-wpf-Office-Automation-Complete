using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Repository;
using System;
using System.Collections.Generic;

namespace OfficeAutomation.Coding.Business.Services
{
	public class ClassDetailInfoService : IClassService<ClassDetailInfoModel>
	{
		private readonly IRepository<ClassDetailInfoModel> _repository;

		public ClassDetailInfoService()
		{
			_repository = Singleton.GetCurrentClassDetailInfos;
		}

		public IEnumerable<ClassDetailInfoModel> GetAll()
		{

			return _repository.GetAll();
		}

		public bool CanDoAdd(ClassDetailInfoModel model)
		{
			_repository.Add(model);
			return true;
		}

		public void AddRange(List<ClassDetailInfoModel> models)
		{
			foreach(var model in models)
			{
				_repository.Add(model);
			}
		}

		public bool CanDoList()
		{
			if(_repository.Count() == 0)
			{

			}

			return true;
		}

	

		public bool CanDoRemove(ClassDetailInfoModel model)
		{
			_repository.Remove(model);
			return true;
		}

		public void Clear()
		{
			_repository.Clear();
		}

		public bool CanDoSearch(List<ClassDetailInfoModel> models)
		{
			//여기서 값이 있는지 확인을 해야겠네 
			throw new NotImplementedException();
		}

		public bool CanDoUpdate(ClassDetailInfoModel model)
		{
			throw new NotImplementedException();
		}

		public int GetCount()
		{
			return _repository.Count();
		}
	}
}
