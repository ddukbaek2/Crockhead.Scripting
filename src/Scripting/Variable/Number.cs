namespace Crockhead.Scripting
{
	/// <summary>
	/// 임의 정밀도를 가진 유리수.
	/// </summary>
	public sealed class Number : Object
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public BigRational Value { get; }

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public override string Type => "Number";

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Number() : base()
		{
			Value = new BigRational();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Number(BigRational value) : base()
		{
			Value = value;
		}

		/// <summary>
		/// 논리 변환.
		/// </summary>
		public override bool ToBoolean()
		{
			return !Value.IsZero;
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			//return "<Number>";
			return Value.ToDisplayString();
		}
	}
}