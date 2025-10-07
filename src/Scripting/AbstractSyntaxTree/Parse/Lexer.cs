namespace Crockhead.Scripting
{
	/// <summary>
	/// 어휘 분석기.
	/// </summary>
	public sealed class Lexer
	{
		/// <summary>
		/// 스크립트.
		/// </summary>
		private readonly string m_Source;

		/// <summary>
		/// 위치.
		/// </summary>
		private int m_Position;

		/// <summary>
		/// 행. (줄)
		/// </summary>
		private int m_Line;

		/// <summary>
		/// 열. (현재 줄에서의 커서)
		/// </summary>
		private int m_Column;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Lexer(string source)
		{
			m_Source = source;
			m_Position = 0;
			m_Line = 1;
			m_Column = 1;
		}

		/// <summary>
		/// 다음 해석.
		/// </summary>
		public Token Next()
		{
			SkipWhitespacesAndComments();

			if (m_Position >= m_Source.Length)
				return new Token(TokenType.EndOfFile, m_Line, m_Column, string.Empty);

			var ch = m_Source[m_Position];
			if (char.IsLetter(ch) || ch == '_')
				return ReadIdentifier();
			if (char.IsDigit(ch))
				return ReadNumber();
			if (ch == '"')
				return ReadString(); // 쌍따옴표만 허용.
			if (ch == '\'')
				throw new SyntaxException(m_Line, m_Column, "문자열은 쌍따옴표(\")만 사용할 수 있습니다.");

			var token = new Token(TokenType.Symbol, m_Line, m_Column, ch.ToString());
			Advance();
			return token;
		}

		private void Advance()
		{
			if (m_Position < m_Source.Length)
			{
				if (m_Source[m_Position] == '\n') { m_Line++; m_Column = 1; }
				else m_Column++;
				m_Position++;
			}
		}

		private char Peek(int offset = 0)
		{
			var index = m_Position + offset;
			return (index >= 0 && index < m_Source.Length) ? m_Source[index] : '\0';
		}

		private void SkipWhitespacesAndComments()
		{
			while (m_Position < m_Source.Length)
			{
				var ch = Peek();
				if (char.IsWhiteSpace(ch))
				{
					Advance();
					continue;
				}
				if (ch == '/' && Peek(1) == '/')
				{
					Advance();
					Advance();
					while (Peek() != '\n' && Peek() != '\0')
						Advance();
					continue;
				}
				break;
			}
		}

		private Token ReadIdentifier()
		{
			var startLine = m_Line;
			var startColumn = m_Column;
			var start = m_Position;

			while (char.IsLetterOrDigit(Peek()) || Peek() == '_')
				Advance();

			var text = m_Source.Substring(start, m_Position - start);
			var type = (text == "function" || text == "var" || text == "return" || text == "true" || text == "false")
				? TokenType.Keyword : TokenType.Identifier;

			return new Token(type, startLine, startColumn, text);
		}

		private Token ReadNumber()
		{
			var startLine = m_Line;
			var startColumn = m_Column;
			var start = m_Position;
			var hasDot = false;

			while (true)
			{
				var ch = Peek();
				if (char.IsDigit(ch))
				{
					Advance();
				}
				else if (ch == '.' && !hasDot)
				{
					hasDot = true; Advance();
				}
				else
				{
					break;
				}
			}

			var text = m_Source.Substring(start, m_Position - start);
			return new Token(TokenType.Number, startLine, startColumn, text);
		}

		private Token ReadString()
		{
			var startLine = m_Line;
			var startColumn = m_Column;
			var quote = Peek(); // '"'
			Advance(); // open quote

			var start = m_Position;
			while (Peek() != quote && Peek() != '\0')
				Advance();
			var text = m_Source.Substring(start, m_Position - start);
			if (Peek() == quote)
				Advance(); // close quote
			return new Token(TokenType.String, startLine, startColumn, text);
		}
	}
}