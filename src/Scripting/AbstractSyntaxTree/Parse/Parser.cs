using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 구문 분석기.
	/// <para>코드를 읽어서 처리 가능한 형태로 구조화.</para>
	/// </summary>
	public sealed class Parser
	{
		/// <summary>
		/// 어휘 분석기.
		/// </summary>
		private readonly Lexer m_Lexer;

		/// <summary>
		/// 코드에서 현재 처리 할 내용.
		/// </summary>
		private Token m_Token;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Parser(string source)
		{
			m_Lexer = new Lexer(source);
			m_Token = m_Lexer.Next();
		}

		/// <summary>
		/// 해석.
		/// </summary>
		public void Parse(out Dictionary<string, FunctionDefinition> functions)
		{
			functions = new Dictionary<string, FunctionDefinition>();
			while (m_Token.Type != TokenType.EndOfFile)
			{
				ExpectKeyword("function");
				string name = ExpectIdentifier();

				ExpectSymbol("(");
				var parameters = new List<string>();
				if (!IsSymbol(")"))
				{
					parameters.Add(ExpectIdentifier());
					while (IsSymbol(","))
					{
						Next();
						parameters.Add(ExpectIdentifier());
					}
				}
				ExpectSymbol(")");

				ExpectSymbol("{");
				var body = new List<IStatement>();
				while (!IsSymbol("}"))
				{
					body.Add(ParseStatement());
				}
				ExpectSymbol("}");

				functions[name] = new FunctionDefinition(name, parameters, body);
			}
		}

		/// <summary>
		/// 구문 해석.
		/// </summary>
		private IStatement ParseStatement()
		{
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "function")
			{
				Next();
				string name = ExpectIdentifier();

				ExpectSymbol("(");
				var parameters = new List<string>();
				if (!IsSymbol(")"))
				{
					parameters.Add(ExpectIdentifier());
					while (IsSymbol(","))
					{
						Next();
						parameters.Add(ExpectIdentifier());
					}
				}
				ExpectSymbol(")");

				ExpectSymbol("{");
				var body = new List<IStatement>();
				while (!IsSymbol("}"))
				{
					body.Add(ParseStatement());
				}
				ExpectSymbol("}");

				return new FunctionDeclarationStatement(name, parameters, body);
			}

			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "var")
			{
				Next();
				
				var name = ExpectIdentifier();
				var initializer = default(IExpression);
				if (IsSymbol("="))
				{
					Next();
					initializer = ParseExpression();
				}

				ExpectSymbol(";");
				return new VariableDeclarationStatement(name, initializer);
			}

			var expression = default(IExpression);
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "return")
			{
				Next();
				expression = default(IExpression);
				if (!IsSymbol(";")) expression = ParseExpression();
				ExpectSymbol(";");
				return new ReturnStatement(expression);
			}

			expression = ParseExpression();
			ExpectSymbol(";");
			return new ExpressionStatement(expression);
		}

		/// <summary>
		/// 표현식 해석.
		/// </summary>
		private IExpression ParseExpression()
		{
			var left = ParseAddSub();
			if (left is VariableExpression variableExpression && IsSymbol("="))
			{
				Next();
				var right = ParseExpression();
				return new AssignmentExpression(variableExpression.Name, right);
			}
			return left;
		}

		/// <summary>
		/// 연산자 더하기, 빼기 해석.
		/// </summary>
		private IExpression ParseAddSub()
		{
			var left = ParseMulDivMod();
			while (m_Token.Type == TokenType.Symbol && (m_Token.Value == "+" || m_Token.Value == "-"))
			{
				var @operator = m_Token.Value;
				Next();
				var right = ParseMulDivMod();
				left = new BinaryExpression(left, right, @operator);
			}
			return left;
		}

		/// <summary>
		/// 연산자 곱하기, 나누기, 나머지 해석.
		/// </summary>
		private IExpression ParseMulDivMod()
		{
			var left = ParsePrimary();
			while (m_Token.Type == TokenType.Symbol && (m_Token.Value == "*" || m_Token.Value == "/" || m_Token.Value == "%"))
			{
				var @operator = m_Token.Value;
				Next();
				var right = ParsePrimary();
				left = new BinaryExpression(left, right, @operator);
			}
			return left;
		}

		/// <summary>
		/// 연산자 해석.
		/// </summary>
		private IExpression ParsePrimary()
		{
			if (m_Token.Type == TokenType.Number)
			{
				var stringValue = m_Token.Value;
				Next();
				return new LiteralExpression(Variable.Number(Number.ParseDecimal(stringValue)));
			}

			if (m_Token.Type == TokenType.String)
			{
				var stringValue = m_Token.Value;
				Next();
				return new LiteralExpression(Variable.String(stringValue));
			}

			if (m_Token.Type == TokenType.Keyword && (m_Token.Value == "true" || m_Token.Value == "false"))
			{
				var booleanValue = m_Token.Value == "true";
				Next();
				return new LiteralExpression(Variable.Boolean(booleanValue));
			}

			if (IsSymbol("("))
			{
				Next();
				var innerExpression = ParseExpression();
				ExpectSymbol(")");
				return innerExpression;
			}

			if (m_Token.Type == TokenType.Identifier)
			{
				string name = m_Token.Value;
				Next();

				if (IsSymbol("("))
				{
					Next();
					var arguments = new List<IExpression>();
					if (!IsSymbol(")"))
					{
						arguments.Add(ParseExpression());
						while (IsSymbol(","))
						{
							Next();
							arguments.Add(ParseExpression());
						}
					}
					ExpectSymbol(")");
					return new CallExpression(name, arguments);
				}

				return new VariableExpression(name);
			}

			ThrowSyntax("식 시작 토큰이 유효하지 않습니다");
			return null; // 도달하지 않음
		}

		/// <summary>
		/// 다음 요소로 이동.
		/// </summary>
		private void Next()
		{
			m_Token = m_Lexer.Next();
		}

		/// <summary>
		/// 심볼 여부.
		/// </summary>
		private bool IsSymbol(string symbol)
		{
			return m_Token.Type == TokenType.Symbol && m_Token.Value == symbol;
		}

		/// <summary>
		/// 심볼 체크.
		/// </summary>
		private void ExpectSymbol(string symbol)
		{
			if (!IsSymbol(symbol))
				ThrowSyntax($"'{symbol}' 기호가 필요합니다");

			Next();
		}

		/// <summary>
		/// 키워드 체크.
		/// </summary>
		private void ExpectKeyword(string keyword)
		{
			if (!(m_Token.Type == TokenType.Keyword && m_Token.Value == keyword))
				ThrowSyntax($"키워드 '{keyword}' 가 필요합니다");

			Next();
		}

		/// <summary>
		/// 식별자 체크.
		/// </summary>
		private string ExpectIdentifier()
		{
			if (m_Token.Type != TokenType.Identifier)
				ThrowSyntax("식별자가 필요합니다");
			
			var value = m_Token.Value;
			Next();
			return value;
		}

		/// <summary>
		/// 문법 오류 예외 던지기.
		/// </summary>
		private void ThrowSyntax(string message)
		{
			throw new SyntaxException(m_Token.Line, m_Token.Column, $"{message}. 현재 토큰: '{m_Token.Value}'");
		}
	}
}