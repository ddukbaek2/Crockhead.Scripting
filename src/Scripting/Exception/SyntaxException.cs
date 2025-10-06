using System;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 문법 오류로 인한 예외.
	/// </summary>
	public sealed class SyntaxException : Exception
	{
		/// <summary>
		/// 파일 경로.
		/// </summary>
		public string FilePath { get; }

		/// <summary>
		/// 줄 번호.
		/// </summary>
		public int Line { get; }

		/// <summary>
		/// 열 위치.
		/// </summary>
		public int Column { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SyntaxException(string filePath, int line, int column, string message) : base(message)
		{
			FilePath = filePath;
			Line = line;
			Column = column;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SyntaxException(int line, int column, string message) : this(string.Empty, line, column, message)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SyntaxException(string message) : this(string.Empty, -1, -1, message)
		{
		}
	}
}