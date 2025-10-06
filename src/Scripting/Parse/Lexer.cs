namespace Crockhead.Scripting
{
	/// <summary>
	/// 렉서.
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
		private int m_Index;

		/// <summary>
		/// 행.
		/// </summary>
		private int m_Line;

		/// <summary>
		/// 열.
		/// </summary>
		private int m_Column;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Lexer(string source)
		{
			m_Source = source;
			m_Index = 0;
			m_Line = 1;
			m_Column = 1;
		}

		/// <summary>
		/// 해석.
		/// </summary>
		public Token Next()
		{
			SkipWhitespacesAndComments();

			if (m_Index >= m_Source.Length)
				return new Token(TokenType.EndOfFile, m_Line, m_Column, string.Empty);

			var ch = m_Source[m_Index];
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
			if (m_Index < m_Source.Length)
			{
				if (m_Source[m_Index] == '\n') { m_Line++; m_Column = 1; }
				else m_Column++;
				m_Index++;
			}
		}

		private char Peek(int offset = 0)
		{
			int i = m_Index + offset;
			return (i >= 0 && i < m_Source.Length) ? m_Source[i] : '\0';
		}

		private void SkipWhitespacesAndComments()
		{
			while (m_Index < m_Source.Length)
			{
				var c = Peek();
				if (char.IsWhiteSpace(c))
				{
					Advance();
					continue;
				}
				if (c == '/' && Peek(1) == '/')
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
			var start = m_Index;

			while (char.IsLetterOrDigit(Peek()) || Peek() == '_')
				Advance();

			var text = m_Source.Substring(start, m_Index - start);
			var type = (text == "function" || text == "var" || text == "return" || text == "true" || text == "false")
				? TokenType.Keyword : TokenType.Identifier;

			return new Token(type, startLine, startColumn, text);
		}

		private Token ReadNumber()
		{
			var startLine = m_Line;
			var startColumn = m_Column;
			var start = m_Index;
			var hasDot = false;

			while (true)
			{
				var c = Peek();
				if (char.IsDigit(c))
				{
					Advance();
				}
				else if (c == '.' && !hasDot)
				{
					hasDot = true; Advance();
				}
				else
				{
					break;
				}
			}

			var text = m_Source.Substring(start, m_Index - start);
			return new Token(TokenType.Number, startLine, startColumn, text);
		}

		private Token ReadString()
		{
			var startLine = m_Line;
			var startColumn = m_Column;
			var quote = Peek(); // '"'
			Advance(); // open quote

			var start = m_Index;
			while (Peek() != quote && Peek() != '\0')
				Advance();
			var text = m_Source.Substring(start, m_Index - start);
			if (Peek() == quote)
				Advance(); // close quote
			return new Token(TokenType.String, startLine, startColumn, text);
		}
	}
}