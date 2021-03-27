using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Objects
{
	public class ConstantVariables
	{
		public const string AccessTypePrivate = "private";
		public const string AccessTypeProtected = "protected";
		public const string AccessTypePrivateProtected = "private protected";


		public enum MemberTypes
		{
			None,
			Field,
			Constant,
		}

		public static string GetResultType(string accessModifiersType)
		{
			string result = string.Empty;

			var AccessModifiers = new List<string>()
			{
				"private",
				"public",
				"protected",
				"internal",
				"protected internal",
				"private protected"
			};

			foreach(var AccessModifier in AccessModifiers)
			{
				if(AccessModifier == accessModifiersType)
				{
					result = accessModifiersType;
				}
			}

			return result;
		
		}
	}
}
