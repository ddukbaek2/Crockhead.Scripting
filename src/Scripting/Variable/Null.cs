namespace Crockhead.Scripting
{
	/// <summary>
	/// 널.
	/// </summary>
	public sealed class Null : Object
	{
		/// <summary>
		/// 전역 변수.
		/// </summary>
		public static readonly Null Instance = new Null();

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public override string Type => "Null";

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Null() : base()
		{
		}

		/// <summary>
		/// 논리 변환.
		/// </summary>
		public override bool ToBoolean()
		{
			return false;
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			return "Null";
		}
	}
}