namespace Crockhead.Scripting
{
	/// <summary>
	/// 값의 종류.
	/// </summary>
	public enum ValueType
	{
		/// <summary>
		/// 없음.
		/// </summary>
		Null,

		/// <summary>
		/// 논리.
		/// </summary>
		Boolean,

		/// <summary>
		/// 숫자. (정수/실수)
		/// </summary>
		Number,

		/// <summary>
		/// 문자열.
		/// </summary>
		String,

		/// <summary>
		/// 함수.
		/// </summary>
		Function,

		/// <summary>
		/// 구조체 타입.
		/// </summary>
		StructType,
		/// <summary>
		/// 구조체 인스턴스.
		/// </summary>
		Struct,
	}
}
