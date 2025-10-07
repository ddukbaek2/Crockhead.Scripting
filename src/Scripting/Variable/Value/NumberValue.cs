namespace Crockhead.Scripting
{
	/// <summary>
	/// 숫자 값.
	/// </summary>
	public sealed class NumberValue : Value
	{
		/// <summary>
		/// 실제 값 프로퍼티.
		/// </summary>
		public Number Value { get; }

		/// <summary>
		/// 값 종류 프로퍼티.
		/// </summary>
		public override ValueType Type => ValueType.Number;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NumberValue(Number value) : base()
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
			//return base.ToString();
			return Value.ToDisplayString();
		}
	}
}