namespace Crockhead.Scripting
{
	/// <summary>
	/// 함수.
	/// </summary>
	public sealed class Function : Object
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public UserFunction Value { get; }

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public override string Type => "Function";

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Function() : base()
		{
			Value = null;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Function(UserFunction value) : base()
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
			return "<Function>";
		}
	}
}