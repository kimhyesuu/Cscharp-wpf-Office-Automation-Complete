using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exe01_Ref
{
	class Program
	{
		static string field;

		static void Main()
		{
			field = "initial value";
			Console.WriteLine($"Before Modify1: {field}");

			// 값을 받고 변경은 안되네 
			Modify1(field, "new value for Modify1");
			Console.WriteLine($"After Modify1: {field}");

			// 참조하고 void 값을 받아
			Modify2(ref field, "new value for Modify2");
			Console.WriteLine($"After Modify2: {field}");

			Console.ReadLine();
		}

		static void Modify1(string storage, string value)
		{
			// This only changes the parameter
			storage = value;
		}

		static void Modify2(ref string storage, string value)
		{
			// This changes the variable that's been passed by reference,
			// e.g. a field
			storage = value;
		}
	}
}
