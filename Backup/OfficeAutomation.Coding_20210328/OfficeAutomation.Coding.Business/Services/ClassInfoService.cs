using OfficeAutomation.Coding.Business.Models;
using OfficeAutomation.Coding.Repository;
using System;
using System.Collections.Generic;

namespace OfficeAutomation.Coding.Business.Services
{
	public class ClassInfoService : IClassService<ClassInfoModel>
	{
		private readonly IRepository<ClassInfoModel> _repository;

		public ClassInfoService()
		{
			_repository = Singleton.GetCurrentClassInfos;
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
			foreach (var model in models)
			{
				_repository.Add(model);
			}
		}

		public bool CanDoRemove(ClassInfoModel model)
		{
			_repository.Remove(model);
			return true;
		}

		public bool CanDoSearch(List<ClassInfoModel> models)
		{
			throw new NotImplementedException();
		}

		public bool CanDoUpdate(ClassInfoModel model)
		{
			throw new NotImplementedException();
		}

		public bool CanDoClear()
		{
			if(_repository is null)
			{
				return false;
			}

			_repository.Clear();
			return true;
		}
	}
}
