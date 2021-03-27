using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Objects
{
	public sealed class TypeSettingSingleton
	{
		private static TypeSetting _typeSetting = null;

		public static TypeSetting GetTypeSettings
		{
			get
			{
				if (_typeSetting is null)
					_typeSetting = new TypeSetting();
				return _typeSetting;
			}
		}

		public TypeSettingSingleton()
		{

		}
	}
}
