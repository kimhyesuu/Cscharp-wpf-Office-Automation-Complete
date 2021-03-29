using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Repository;
using System;
using System.Collections.Generic;

namespace OfficeAutomation.Coding.Business.Services
{
	public class ClassDetailInfoService : IClassService<ClassInfoModel>
	{
		private readonly IRepository<ClassInfoModel> _repository;

		public ClassDetailInfoService()
		{
			_repository = Singleton.GetCurrentClassDetailInfos;
		}

		public IEnumerable<ClassInfoModel> GetAll()
		{
			return _repository.GetAll();
		}

		public bool CanDoAdd(ClassInfoModel model)
		{
			_repository.Add(model);
			return true;
		}

		public void AddRange(List<ClassInfoModel> models)
		{
			foreach(var model in models)
			{
				_repository.Add(model);
			}
		}

		public bool CanDoRemove(ClassInfoModel model)
		{
			_repository.Remove(model);
			return true;
		}

		public bool CanDoClear()
		{
			_repository.Clear();
			return true;
		}

		public bool CanDoSearch(List<ClassInfoModel> models)
		{
			//여기서 값이 있는지 확인을 해야겠네 
			throw new NotImplementedException();
		}

		public bool CanDoUpdate(ClassInfoModel model)
		{
			throw new NotImplementedException();
		}


	}
}
