namespace Crockhead.Scripting
{
	/// <summary>
	/// 논리 값.
	/// </summary>
	public sealed class BooleanValue : Value
	{
		/// <summary>
		/// 실제 값 프로퍼티.
		/// </summary>
		public bool Value { get; }

		/// <summary>
		/// 값 종류 프로퍼티.
		/// </summary>
		public override ValueType Type => ValueType.Boolean;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public BooleanValue(bool value) : base()
		{
			Value = value;
		}

		/// <summary>
		/// 논리 변환.
		/// </summary>
		public override bool ToBoolean()
		{
			return Value;
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			//return base.ToString();
			return Value ? "true" : "false";
		}
	}
}