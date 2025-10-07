using System;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 파서.
	/// </summary>
	public sealed class Parser
	{
		/// <summary>
		/// 구문 해석기.
		/// </summary>
		private readonly Lexer m_Lexer;

		/// <summary>
		/// 현재 해석 할 문자열 덩어리.
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
		public void Parse(ref Dictionary<string, FunctionDefinition> functions)
		{
			while (m_Token.Type != TokenType.EndOfFile)
			{
				if (m_Token.Type == TokenType.Keyword && m_Token.Value == "function")
				{
					Next();

					var name = ExpectIdentifier();
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
					var body = ParseBlock();
					functions[name] = new FunctionDefinition(name, parameters, body);
					continue;
				}
				throw new SyntaxException(m_Token.Line, m_Token.Column, "함수 정의 필요");
			}
		}

		/// <summary>
		/// 다음 구문 해석.
		/// </summary>
		private void Next()
		{
			m_Token = m_Lexer.Next();
		}

		/// <summary>
		/// 기호 여부.
		/// </summary>
		private bool IsSymbol(string symbol)
		{
			return m_Token.Type == TokenType.Symbol && m_Token.Value == symbol;
		}

		/// <summary>
		/// 식별자 검사.
		/// </summary>
		private string ExpectIdentifier()
		{
			if (m_Token.Type != TokenType.Identifier)
				throw new SyntaxException(m_Token.Line, m_Token.Column, "식별자 필요");
			string name = m_Token.Value;
			Next();
			return name;
		}

		/// <summary>
		/// 기호 검사.
		/// </summary>
		private void ExpectSymbol(string symbol)
		{
			if (m_Token.Type != TokenType.Symbol || m_Token.Value != symbol)
				throw new SyntaxException(m_Token.Line, m_Token.Column, $"'{symbol}' 필요");
			Next();
		}

		/// <summary>
		/// 키워드 검사.
		/// </summary>
		private void ExpectKeyword(string keyword)
		{
			if (m_Token.Type != TokenType.Keyword || m_Token.Value != keyword)
				throw new SyntaxException(m_Token.Line, m_Token.Column, $"'{keyword}' 필요");
			
			Next();
		}

		/// <summary>
		/// 구문 해석.
		/// </summary>
		private IStatement ParseStatement()
		{
			// if.
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "if")
				return ParseIfStatement();

			// for.
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "for")
				return ParseForStatement();

			// switch.
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "switch")
				return ParseSwitchStatement();

			// break.
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "break")
				return ParseBreak();

			// continue.
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "continue")
				return ParseContinue();

			var expression = default(IExpression);

			// var.
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "var")
			{
				Next();
				
				var name = ExpectIdentifier();
				expression = null;
				if (IsSymbol("="))
				{
					Next();
					expression = ParseExpression();
				}
				ExpectSymbol(";");
				return new VariableDeclarationStatement(name, expression);
			}

			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "return")
			{
				Next();
				
				expression = null;
				if (!IsSymbol(";"))
					expression = ParseExpression();
				ExpectSymbol(";");
				return new ReturnStatement(expression);
			}

			expression = ParseExpression();
			ExpectSymbol(";");
			return new ExpressionStatement(expression);
		}

		/// <summary>
		/// 조건 구문 해석.
		/// </summary>
		private IStatement ParseIfStatement()
		{
			ExpectKeyword("if");
			ExpectSymbol("(");
			var condition = ParseExpression();
			ExpectSymbol(")");
			var then = ParseBlock();
			var elseIfs = new List<(IExpression, List<IStatement>)>();
			var @else = default(List<IStatement>);

			while (m_Token.Type == TokenType.Keyword && m_Token.Value == "else")
			{
				Next();

				if (m_Token.Type == TokenType.Keyword && m_Token.Value == "if")
				{
					Next();

					ExpectSymbol("(");
					var elseIfCondition = ParseExpression();
					ExpectSymbol(")");
					var elseIfBody = ParseBlock();
					elseIfs.Add((elseIfCondition, elseIfBody));
				}
				else
				{
					@else = ParseBlock();
					break;
				}
			}

			return new IfStatement(condition, then, elseIfs, @else);
		}

		/// <summary>
		/// 반복 구문 해석.
		/// </summary>
		private IStatement ParseForStatement()
		{
			ExpectKeyword("for");
			ExpectSymbol("(");

			IStatement init = null;
			IExpression cond = null;
			IExpression post = null;

			if (!IsSymbol(";"))
			{
				if (m_Token.Type == TokenType.Keyword && m_Token.Value == "var")
				{
					Next();
					var name = ExpectIdentifier();
					IExpression initializerExpression = null;
					if (IsSymbol("="))
					{
						Next();
						initializerExpression = ParseExpression();
					}
					ExpectSymbol(";");
					init = new VariableDeclarationStatement(name, initializerExpression);
				}
				else
				{
					var e = ParseExpression();
					ExpectSymbol(";");
					init = new ExpressionStatement(e);
				}
			}
			else
			{
				ExpectSymbol(";");
			}

			if (!IsSymbol(";"))
			{
				cond = ParseExpression();
			}
			ExpectSymbol(";");

			if (!IsSymbol(")"))
			{
				post = ParseExpression();
			}
			ExpectSymbol(")");
			var body = ParseBlock();
			return new ForStatement(init, cond, post, body);
		}

		/// <summary>
		/// 분기 구문 해석.
		/// </summary>
		private IStatement ParseSwitchStatement()
		{
			ExpectKeyword("switch");
			ExpectSymbol("(");
			var expression = ParseExpression();
			ExpectSymbol(")");
			ExpectSymbol("{");

			var cases = new List<(IExpression, List<IStatement>)>();
			var defaultBody = default(List<IStatement>);

			while (!IsSymbol("}"))
			{
				if (m_Token.Type == TokenType.Keyword && m_Token.Value == "case")
				{
					Next();
					var label = ParseExpression();
					ExpectSymbol(":");
					var body = new List<IStatement>();
					while (!(m_Token.Type == TokenType.Keyword && (m_Token.Value == "case" || m_Token.Value == "default")) && !IsSymbol("}"))
					{
						if (IsSymbol(";")) { Next(); continue; }
						body.Add(ParseStatement());
					}
					cases.Add((label, body));
					continue;
				}
				if (m_Token.Type == TokenType.Keyword && m_Token.Value == "default")
				{
					Next();
					ExpectSymbol(":");
					defaultBody = new List<IStatement>();
					while (!IsSymbol("}"))
					{
						if (IsSymbol(";")) { Next(); continue; }
						defaultBody.Add(ParseStatement());
					}
					break;
				}
				throw new SyntaxException(m_Token.Line, m_Token.Column, "switch 문법 오류");
			}
			ExpectSymbol("}");
			return new SwitchStatement(expression, cases, defaultBody);
		}

		/// <summary>
		/// 탈출 해석.
		/// </summary>
		private IStatement ParseBreak()
		{
			ExpectKeyword("break");
			ExpectSymbol(";");
			return new BreakStatement();
		}

		/// <summary>
		/// 이어하기 해석.
		/// </summary>
		private IStatement ParseContinue()
		{
			ExpectKeyword("continue");
			ExpectSymbol(";");
			return new ContinueStatement();
		}

		/// <summary>
		/// 블록 해석.
		/// </summary>
		private List<IStatement> ParseBlock()
		{
			ExpectSymbol("{");
			var list = new List<IStatement>();
			while (!IsSymbol("}"))
			{
				list.Add(ParseStatement());
			}
			ExpectSymbol("}");
			return list;
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
		/// 더하기 빼기 해석.
		/// </summary>
		private IExpression ParseAddSub()
		{
			var left = ParseMulDivMod();
			while (m_Token.Type == TokenType.Symbol && (m_Token.Value == "+" || m_Token.Value == "-"))
			{
				var op = m_Token.Value;
				Next();
				var right = ParseMulDivMod();
				left = new BinaryExpression(left, right, op);
			}
			return left;
		}

		/// <summary>
		/// 곱하기 나누기 나머지 해석.
		/// </summary>
		private IExpression ParseMulDivMod()
		{
			var left = ParsePrimary();
			while (m_Token.Type == TokenType.Symbol && (m_Token.Value == "*" || m_Token.Value == "/" || m_Token.Value == "%"))
			{
				var op = m_Token.Value;
				Next();
				var right = ParsePrimary();
				left = new BinaryExpression(left, right, op);
			}
			return left;
		}

		/// <summary>
		/// 기본 해석.
		/// </summary>
		private IExpression ParsePrimary()
		{
			if (m_Token.Type == TokenType.Number)
			{
				var text = m_Token.Value;
				Next();
				return new LiteralExpression(Variable.Number(Number.Parse(text)));
			}

			if (m_Token.Type == TokenType.String)
			{
				var text = m_Token.Value;
				Next();
				return new LiteralExpression(Variable.String(text));
			}

			if (m_Token.Type == TokenType.Keyword && (m_Token.Value == "true" || m_Token.Value == "false"))
			{
				var value = m_Token.Value == "true";
				Next();
				return new LiteralExpression(Variable.Boolean(value));
			}

			if (IsSymbol("("))
			{
				Next();
				var expression = ParseExpression();
				ExpectSymbol(")");
				return expression;
			}

			if (m_Token.Type == TokenType.Identifier)
			{
				var name = m_Token.Value;
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

			throw new SyntaxException(m_Token.Line, m_Token.Column, "유효하지 않은 표현식");
		}
	}
}