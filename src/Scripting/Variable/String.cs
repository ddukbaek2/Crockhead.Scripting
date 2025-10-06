namespace Crockhead.Scripting
{
	/// <summary>
	/// 문자열.
	/// </summary>
	public sealed class String : Object
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public override string Type => "String";

		/// <summary>
		/// 생성됨.
		/// </summary>
		public String() : base()
		{
			Value = string.Empty;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public String(string value) : base()
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
			//return "<String>";
			return Value;
		}
	}
}