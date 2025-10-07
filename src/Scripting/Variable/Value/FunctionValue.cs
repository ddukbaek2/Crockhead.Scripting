namespace Crockhead.Scripting
{
	/// <summary>
	/// 함수 값.
	/// </summary>
	public sealed class FunctionValue : Value
	{
		/// <summary>
		/// 실제 값 프로퍼티.
		/// </summary>
		public Function Value { get; }

		/// <summary>
		/// 값 종류 프로퍼티.
		/// </summary>
		public override ValueType Type => ValueType.Function;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FunctionValue(Function value) : base()
		{
			Value = value;
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