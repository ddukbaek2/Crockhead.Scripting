using Crockhead.Core;
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

				// [추가] 선택적 타입 명시: var name : Type = expr;
				var type = string.Empty;
				if (IsSymbol(":"))
				{
					Next();
					type = ExpectIdentifier();
				}

				IExpression initializer = null;
				if (IsSymbol("="))
				{
					Next();
					initializer = ParseExpression();
				}

				ExpectSymbol(";");

				// [추가] 타입 명시가 있는 경우 TypedVariableDeclarationStatement 사용
				if (!string.IsNullOrEmpty(type))
					return new TypedVariableDeclarationStatement(name, type, initializer);

				return new VariableDeclarationStatement(name, initializer);
			}

			// return.
			if (m_Token.Type == TokenType.Keyword && m_Token.Value == "return")
			{
				Next();

				expression = null;
				if (!IsSymbol(";"))
					expression = ParseExpression();
				ExpectSymbol(";");
				return new ReturnStatement(expression);
			}

			// 일반 표현식 문.
			expression = ParseExpression();

			// [추가] 좌변이 멤버 접근일 때 대입 지원 (obj.field = expr)
			if (IsSymbol("="))
			{
				if (expression is VariableExpression variableExpression)
				{
					Next();
					var right = ParseExpression();
					ExpectSymbol(";");
					return new ExpressionStatement(new AssignmentExpression(variableExpression.Name, right));
				}
				if (expression is MemberAccessExpression memberAccess)
				{
					Next();
					var right = ParseExpression();
					ExpectSymbol(";");
					return new ExpressionStatement(new MemberAssignmentExpression(memberAccess, right));
				}
			}

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
			if (IsSymbol("=")) {
				if (left is VariableExpression variableExpression) {
				Next();
				var right = ParseExpression();
				return new AssignmentExpression(variableExpression.Name, right); }
				if (left is MemberAccessExpression memberAccess) { Next(); var right = ParseExpression(); return new MemberAssignmentExpression(memberAccess, right); }
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
			var left = ParsePostfix();
			while (m_Token.Type == TokenType.Symbol && (m_Token.Value == "*" || m_Token.Value == "/" || m_Token.Value == "%"))
			{
				var op = m_Token.Value;
				Next();
				var right = ParsePostfix();
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

		/// <summary>
		/// 후수정 해석.
		/// </summary>
		private IExpression ParsePostfix()
		{
			var expression = ParsePrimary();

			// 멤버 접근 체이닝.
			while (m_Token.Type == TokenType.Symbol && m_Token.Value == ".")
			{
				Next();

				var name = ExpectIdentifier();
				expression = new MemberAccessExpression(expression, name);
			}

			// 호출 체이닝.
			while (m_Token.Type == TokenType.Symbol && m_Token.Value == "(")
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
				expression = new InvokeExpression(expression, arguments);
			}

			return expression;
		}
	}
}