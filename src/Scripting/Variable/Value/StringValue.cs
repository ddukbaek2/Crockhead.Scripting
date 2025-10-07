namespace Crockhead.Scripting
{
	/// <summary>
	/// 문자열 값.
	/// </summary>
	public sealed class StringValue : Value
	{
		/// <summary>
		/// 실제 값 프로퍼티.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// 값 종류 프로퍼티.
		/// </summary>
		public override ValueType Type => ValueType.String;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public StringValue(string value) : base()
		{
			Value = value;
		}

		/// <summary>
		/// 논리 변환.
		/// </summary>
		public override bool ToBoolean()
		{
			return !string.IsNullOrEmpty(Value);
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			//return base.ToString();
			return Value;
		}
	}
}