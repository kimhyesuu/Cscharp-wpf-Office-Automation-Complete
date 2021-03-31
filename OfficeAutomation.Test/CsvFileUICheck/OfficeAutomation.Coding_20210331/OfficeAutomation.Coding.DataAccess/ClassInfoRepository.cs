using System;
using System.Collections.Generic;
using System.Linq;

namespace OfficeAutomation.Coding.Repository
{
	public class ClassInfoRepository<T> : IRepository<T>
	{
		private static List<T> ClassInfos { get; set; } = new List<T>();

		public bool Add(T model)
		{
			ClassInfos.Add(model);
			return true;
		}

		public IEnumerable<T> GetAll()
		{
			return ClassInfos;
		}

		public bool Remove(T model)
		{
			ClassInfos.Remove(model);
			return true;
		}

		public bool Clear()
		{
			ClassInfos.Clear();
			return true;
		}

		public bool Search(List<T> models)
		{
			throw new NotImplementedException();
		}

		public bool Update(T model)
		{
			throw new NotImplementedException();
		}

		public int Count()
		{
			return ClassInfos.Count();
		}
	}
}
