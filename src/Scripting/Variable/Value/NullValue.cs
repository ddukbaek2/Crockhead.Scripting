namespace Crockhead.Scripting
{
	/// <summary>
	/// 널 값.
	/// </summary>
	public sealed class NullValue : Value
	{
		/// <summary>
		/// 공유 변수.
		/// </summary>
		public static readonly NullValue SharedInstance = new NullValue();

		/// <summary>
		/// 실제 값 프로퍼티.
		/// </summary>
		public NullValue Value => NullValue.SharedInstance;

		/// <summary>
		/// 값 종류 프로퍼티.
		/// </summary>
		public override ValueType Type => ValueType.Null;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NullValue() : base()
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
			return base.ToString();
		}
	}
}