using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 파서.
	/// </summary>
	public sealed class Parser
	{
		/// <summary>
		/// 렉서.
		/// </summary>
		private readonly Lexer m_Lexer;

		/// <summary>
		/// 토큰.
		/// </summary>
		private Token m_Current;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Parser(string source)
		{
			m_Lexer = new Lexer(source);
			m_Current = m_Lexer.Next();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="functions"></param>
		public void ParseAllFunctions(out Dictionary<string, FunctionDefinition> functions)
		{
			functions = new Dictionary<string, FunctionDefinition>();
			while (m_Current.Type != TokenType.EndOfFile)
			{
				ExpectKeyword("function");
				string name = ExpectIdentifier();

				ExpectSymbol("(");
				var parameters = new List<string>();
				if (!IsSymbol(")"))
				{
					parameters.Add(ExpectIdentifier());
					while (IsSymbol(",")) { Next(); parameters.Add(ExpectIdentifier()); }
				}
				ExpectSymbol(")");

				ExpectSymbol("{");
				var body = new List<IStatement>();
				while (!IsSymbol("}"))
					body.Add(ParseStatement());
				ExpectSymbol("}");

				functions[name] = new FunctionDefinition(name, parameters, body);
			}
		}

		private IStatement ParseStatement()
		{
			if (m_Current.Type == TokenType.Keyword && m_Current.Value == "function")
			{
				Next();
				string name = ExpectIdentifier();

				ExpectSymbol("(");
				var parameters = new List<string>();
				if (!IsSymbol(")"))
				{
					parameters.Add(ExpectIdentifier());
					while (IsSymbol(",")) { Next(); parameters.Add(ExpectIdentifier()); }
				}
				ExpectSymbol(")");

				ExpectSymbol("{");
				var body = new List<IStatement>();
				while (!IsSymbol("}"))
					body.Add(ParseStatement());
				ExpectSymbol("}");

				return new FunctionDeclarationStatement(name, parameters, body);
			}

			if (m_Current.Type == TokenType.Keyword && m_Current.Value == "var")
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
			if (m_Current.Type == TokenType.Keyword && m_Current.Value == "return")
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

		private IExpression ParseAddSub()
		{
			var expression = ParseMulDivMod();
			while (m_Current.Type == TokenType.Symbol && (m_Current.Value == "+" || m_Current.Value == "-"))
			{
				var @operator = m_Current.Value;
				Next();
				var right = ParseMulDivMod();
				expression = new BinaryExpression(expression, right, @operator);
			}
			return expression;
		}

		private IExpression ParseMulDivMod()
		{
			var expression = ParsePrimary();
			while (m_Current.Type == TokenType.Symbol && (m_Current.Value == "*" || m_Current.Value == "/" || m_Current.Value == "%"))
			{
				var @operator = m_Current.Value;
				Next();
				var right = ParsePrimary();
				expression = new BinaryExpression(expression, right, @operator);
			}
			return expression;
		}

		private IExpression ParsePrimary()
		{
			if (m_Current.Type == TokenType.Number)
			{
				string text = m_Current.Value;
				Next();
				return new LiteralExpression(Value.Number(BigRational.ParseDecimal(text)));
			}

			if (m_Current.Type == TokenType.String)
			{
				string text = m_Current.Value; Next();
				return new LiteralExpression(Value.String(text));
			}

			if (m_Current.Type == TokenType.Keyword && (m_Current.Value == "true" || m_Current.Value == "false"))
			{
				bool booleanValue = m_Current.Value == "true"; Next();
				return new LiteralExpression(Value.Boolean(booleanValue));
			}

			if (IsSymbol("("))
			{
				Next();
				var inner = ParseExpression();
				ExpectSymbol(")");
				return inner;
			}

			if (m_Current.Type == TokenType.Identifier)
			{
				string name = m_Current.Value; Next();

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

		private void Next()
		{
			m_Current = m_Lexer.Next();
		}

		private bool IsSymbol(string symbol)
		{
			return m_Current.Type == TokenType.Symbol && m_Current.Value == symbol;
		}

		private void ExpectSymbol(string symbol)
		{
			if (!IsSymbol(symbol))
				ThrowSyntax($"'{symbol}' 기호가 필요합니다");

			Next();
		}

		private void ExpectKeyword(string keyword)
		{
			if (!(m_Current.Type == TokenType.Keyword && m_Current.Value == keyword))
				ThrowSyntax($"키워드 '{keyword}' 가 필요합니다");

			Next();
		}

		private string ExpectIdentifier()
		{
			if (m_Current.Type != TokenType.Identifier)
				ThrowSyntax("식별자가 필요합니다");
			
			var value = m_Current.Value;
			Next();
			return value;
		}

		private void ThrowSyntax(string message)
		{
			throw new SyntaxException(m_Current.Line, m_Current.Column, $"{message}. 현재 토큰: '{m_Current.Value}'");
		}
	}
}