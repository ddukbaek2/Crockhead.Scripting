namespace Crockhead.Scripting
{
	/// <summary>
	/// 토큰 타입.
	/// </summary>
	public enum TokenType
	{
		/// <summary>
		/// 식별자.
		/// </summary>
		Identifier,

		/// <summary>
		/// 숫자.
		/// </summary>
		Number,

		/// <summary>
		/// 문자열.
		/// </summary>
		String,

		/// <summary>
		/// 심볼.
		/// </summary>
		Symbol,

		/// <summary>
		/// 키워드.
		/// </summary>
		Keyword,

		/// <summary>
		/// 파일의 끝.
		/// </summary>
		EndOfFile,
	}
}