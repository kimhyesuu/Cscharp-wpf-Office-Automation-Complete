using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassTypes
{
	public  class animal
	{
	
		public abstract virtual int MyProperty { get; set; }
		protected internal virtual uint Aacs()
		{
			uint obj = 1; // uint에 맞는 value 설정
			return obj;
		}

	}

	public abstract class animalxs : animal
	{

		protected internal virtual uint Aacs()
		{
			uint obj = 1; // uint에 맞는 value 설정
			return obj;
		}

	}

	//"none"				  ,
	//"abstract"          ,
	//"partial"           ,
	//"sealed"            ,
	//"static"				  ,	

	//static
	// 부모 클래스와 자식 클래스의 한정자는 같아야한다.
	// sealed는 상속 금지 즉 sealed 클래스는 다른 클래스에 적용이 안됩니다. 
	// 
	// base 클래스가 있을 때 

	// 한정자 
	// 1. 부모 클래스가 internal이면, 자식클래스도 동일하게 하고 보내기 
	// 2. 부모 클래스가 public이면, 자식 클래스는 둘다 사용 가능 

	// 클래스 타입
	// 0. static이면 따로 놀게 해야된다.   tpye이 sealed, static이면 상속 안됨 cando로 보내자  
	// 1. sealed는 상속을 해줄수 없다, 부모가 sealed면, 그냥 지나가고 
	// 2. 부모가 sealed가 아니고 static이 아니면 다 가능 
	// 3. 

	// 필드 
	// 프로퍼티 
	// 0. base고 
	// 1. 부모랑 member 이름이 같은게 있으면,
	// 2. 타입이 같은지 확인
	// 3.  

	// 메서드 

	internal abstract class fd
	{
		private virtual int MyProperty { get; set; }
		public  int ss;
		protected internal string asd;
		private List<int> fdas;

		public fd()
		{

		}

		public virtual int ssdf()
		{
			return 1;
		}

		//public fd(int a)
		//{
		//	ss = a;
		//}
	}
	// base 와 같은게 있으면 
	// 한정자가 달라도 된다. data type이 달라도 된다. 
	// data type 다르면 주석 달기 
	// 멤버 타입이 다르면 return 해주기 
	// int 
	internal class Dewwq : fd
	{
		// base 클래스와 같은 이름의 필드 모음
		private List<int> fdas;
		public string asd;
		public override int MyProperty { get; set; }
													  // 
		private string ss()
		{
			return "fd";
		}


		// 아님
		private string fieldName;
		public string PropertyName
		{
			get { return fieldName; }
			set { SetProperty(ref fieldName, value); }
		}

		private void SetProperty(ref string fieldName, string value)
		{
		
		}

		public override int ssdf()
		{
		  var o = base.ssdf();
		  return o;
		}

		//public Dewwq(int a) : base(a)
		//{

		//}
	}



	public class Program
	{
		static void Main(string[] args)
		{
			Dewwq df = new Dewwq();
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



	public sealed class Cat : animal
	{
		private const decimal Cs = 2;

		private int FeedKinds;  // good
		private int Pain;  // good
		private int Kinds;  // good

		public Cat()
		{

		}

		protected internal override uint Aacs()
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
}
