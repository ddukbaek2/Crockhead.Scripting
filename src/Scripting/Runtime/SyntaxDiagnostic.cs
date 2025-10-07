namespace Crockhead.Scripting
{
	/// <summary>
	/// 문법 진단 객체.
	/// </summary>
	public sealed class SyntaxDiagnostic
	{
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
		/// 메시지 프로퍼티.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SyntaxDiagnostic(string filePath, int line, int column, string message)
		{
			FilePath = filePath;
			Line = line;
			Column = column;
			Message = message;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SyntaxDiagnostic(int line, int column, string message) : this(string.Empty, line, column, message)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SyntaxDiagnostic(string message) : this(string.Empty, -1, -1, message)
		{
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			return $"{Line}:{Column} {Message}";
		}
	}
}
