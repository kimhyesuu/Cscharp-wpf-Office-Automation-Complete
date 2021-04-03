using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write
{
	class Program
	{
		static void Main(string[] args)
		{
			if (File.Exists("d:\\kim.txt") is false)
			{
				File.Create("d:\\kim.txt");
			}
		}

		//public static async Task ExampleAsync()
		//{
		//	string path = string.Empty;
		//	var d = new FileStream(path, FileMode.Append);
		//	string text =
		//		 "A class is the most powerful data type in C#. Like a structure, " +
		//		 "a class defines the data and behavior of the data type. ";

		//	await d.CopyToAsync(text); (, text);
		//}
	}
}
