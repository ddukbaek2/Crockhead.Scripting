namespace Crockhead.Scripting
{
	/// <summary>
	/// 토큰.
	/// <para>스크립트의 최소 의미 단위 블럭.</para>
	/// </summary>
	public readonly struct Token
	{
		/// <summary>
		/// 토큰 종류 프로퍼티.
		/// </summary>
		public TokenType Type { get; }

		/// <summary>
		/// 파일 경로 프로퍼티.
		/// </summary>
		public string FilePath { get; }

		/// <summary>
		/// 줄 번호 프로퍼티.
		/// </summary>
		public int Line { get; }

		/// <summary>
		/// 열 위치 프로퍼티.
		/// </summary>
		public int Column { get; }

		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public string Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Token(TokenType type, string filePath, int line, int column, string value)
		{
			Type = type;
			FilePath = filePath;
			Line = line;
			Column = column;
			Value = value;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Token(TokenType type, int line, int column, string value)
		{
			Type = type;
			FilePath = string.Empty;
			Line = line;
			Column = column;
			Value = value;
		}
	}
}