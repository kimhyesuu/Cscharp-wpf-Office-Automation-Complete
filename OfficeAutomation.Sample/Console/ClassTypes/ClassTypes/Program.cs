using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTypes
{
	public abstract class Cat
	{
		public Cat()
		{

		}

		private int FeedKinds()
		{
			int obj; // int에 맞는 value 설정
			return obj;
		}  // good

		private int Pain()
		{
			int obj; // int에 맞는 value 설정
			return obj;
		}  // good

		private int Kinds()
		{
			int obj; // int에 맞는 value 설정
			return obj;
		}  // good

		public long Cs()
		{
			long obj; // long에 맞는 value 설정
			return obj;
		}

		protected double Sa()
		{
			double obj; // double에 맞는 value 설정
			return obj;
		}

		protected long As()
		{
			long obj; // long에 맞는 value 설정
			return obj;
		}

	}

	public class Cat
	{
		private const decimal Cs =  2;

		private int FeedKinds;  // good
		private int Pain;  // good
		private int Kinds;  // good

		public Cat()
		{

		}

		protected internal uint Aacs()
		{
			uint obj = 1; // uint에 맞는 value 설정
			return obj;
		}

	}

	public partial class PartialClass
	{
	}

	public abstract class AbstractClass
	{
		internal int a = 1;
		private int z = 1;
		protected int s = 1;
		protected internal int zx = 1;
	}

	public static class StaticClass
	{
		internal static int a = 1;
		private static int z = 1;

		// static 에는 protected를 포함이 안된다.
		protected static int s = 1; 
		protected internal static int zx = 1;
	}

	public sealed class SealedClass
	{
		internal int a = 1;
		private int z = 1;
		protected int s = 1;
		protected internal int zx = 1;
	}

 
   internal sealed class Dddxs
	{
		internal int a = 1;
		private int z = 1;
		protected int s = 1;
		protected internal int zx = 1;
	}

	internal abstract class Dddaacs
	{
		internal int a = 1;
		private int z = 1;
		protected int s = 1;
		protected internal int zx = 1;
	}

	internal partial class Ddda
	{
		internal int a = 1;
		private int z = 1;
		protected int s = 1;
		protected internal int zx = 1;
	}

	internal class Dddaaaa
	{
		internal int a = 1;
		public int z = 1;
		protected int s = 1;
		protected internal int zx = 1;
	}

	internal static class Ddd1
	{

	}

	class ff
	{

	}

	public class Program
	{
		static void Main(string[] args)
		{
			string str = "f d";
			
			string temp = "f d".Trim();

			if (string.IsNullOrWhiteSpace(str))
			{
				Console.WriteLine("웅");
			}
			Console.WriteLine(temp);

			Console.WriteLine("아니");
			Console.Read();
		}

		private int test()
		{
			int  i	 = 1;
			byte b	 = 1;
			decimal d = 1;
			double dd = 1.1;
			float f = 1.1f;

			char c = ' ';
			string a; 
			return i;
		}
	}
}
