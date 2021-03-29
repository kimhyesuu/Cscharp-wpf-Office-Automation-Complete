using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeAutomation.Coding.Repository
{
	public class ClassDetailInfoRepository<T> : IRepository<T>
	{
		private static List<T> ClassDetailInfos { get; set; } = new List<T>();

		public bool Add(T model)
		{
			ClassDetailInfos.Add(model);
			return true;
		}

		public IEnumerable<T> GetAll()
		{
			return ClassDetailInfos;
		}

		public bool Remove(T model)
		{
			ClassDetailInfos.Remove(model);
			return true;
		}

		public bool Clear()
		{
			ClassDetailInfos.Clear();
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
	}
}
