namespace Crockhead.Scripting
{
	/// <summary>
	/// 논리.
	/// </summary>
	public sealed class Boolean : Object
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public bool Value { get; }

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public override string Type => "Boolean";

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Boolean() : base()
		{
			Value = false;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Boolean(bool value) : base()
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
			//return "<Boolean>";
			return Value ? "true" : "false";
		}
	}
}