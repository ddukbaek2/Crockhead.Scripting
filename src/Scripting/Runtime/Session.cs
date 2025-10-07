using System;
using System.Collections.Generic;

namespace Crockhead.Scripting
{
	/// <summary>
	/// 실행 세션.
	/// </summary>
	public sealed class Session
	{
		/// <summary>
		/// 바인딩 한 함수 목록.
		/// </summary>
		private readonly Dictionary<string, OnBindingFunctionDelegate> m_BindingFunctions;

		/// <summary>
		/// 스크립트 함수 목록.
		/// </summary>
		private readonly Dictionary<string, FunctionDefinition> m_Functions;

		/// <summary>
		/// 전역 스코프.
		/// </summary>
		private readonly Scope m_Scope;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Session(Dictionary<string, OnBindingFunctionDelegate> bindingFunctions, Dictionary<string, FunctionDefinition> functions)
		{
			m_BindingFunctions = bindingFunctions;
			m_Functions = functions;
			m_Scope = new Scope(null);
		}

		/// <summary>
		/// 함수 이름으로 호출.
		/// </summary>
		public Variable Call(string functionName, Variable[] parameters)
		{
			// 바인딩된 C# 함수 호출.
			if (m_BindingFunctions.TryGetValue(functionName, out var binding))
				return binding.Invoke(parameters);

			// 스크립트 함수 호출.
			if (m_Functions.TryGetValue(functionName, out var definition))
				return Call(new Function(definition, m_Scope), parameters);

			throw new Exception($"알 수 없는 함수: {functionName}");
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Variable Call(FunctionDefinition definition, Variable[] parameters)
		{
			return Call(new Function(definition, m_Scope), parameters);
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Variable Call(Function function, Variable[] parameters)
		{
			var localScope = new Scope(function.CapturedScope);

			// 인자 매핑.
			var paramCount = function.Definition.Parameters.Count;
			for (var i = 0; i < paramCount; i++)
			{
				var argument = (i < parameters.Length) ? parameters[i] : Variable.Null();
				localScope.SetLocalVariable(function.Definition.Parameters[i], argument);
			}

			try
			{
				foreach (var statement in function.Definition.Body)
				{
					ExecuteStatement(localScope, statement);
				}
			}
			catch (ReturnTrigger returnTrigger)
			{
				return returnTrigger.Value ?? Variable.Null();
			}

			return Variable.Null();
		}

		/// <summary>
		/// 구문 실행.
		/// </summary>
		private void ExecuteStatement(Scope scope, IStatement statement)
		{
			// 함수 선언.
			if (statement is FunctionDeclarationStatement funcDeclarationStatement)
			{
				var functionDefinition = new FunctionDefinition(funcDeclarationStatement.Name, funcDeclarationStatement.Parameters, funcDeclarationStatement.Body);
				var function = Variable.Function(new Function(functionDefinition, scope));
				scope.SetLocalVariable(funcDeclarationStatement.Name, function);
				return;
			}

			// 변수 선언.
			if (statement is VariableDeclarationStatement variableDeclarationStatement)
			{
				var value = variableDeclarationStatement.Expression != null
					? variableDeclarationStatement.Expression.Evaluate(new Context(this, scope))
					: Variable.Null();
				scope.SetLocalVariable(variableDeclarationStatement.Name, value);
				return;
			}

			// 표현식.
			if (statement is ExpressionStatement expressionStatement)
			{
				expressionStatement.Expression.Evaluate(new Context(this, scope));
				return;
			}

			// 반환문.
			if (statement is ReturnStatement returnStatement)
			{
				var returnVariable = returnStatement.Expression != null
					? returnStatement.Expression.Evaluate(new Context(this, scope))
					: Variable.Null();
				throw new ReturnTrigger(returnVariable);
			}

			// 조건문.
			if (statement is IfStatement ifStatement)
			{
				var context = new Context(this, scope);

				if (ifStatement.Condition.Evaluate(context).ToBoolean())
				{
					foreach (var thenStatement in ifStatement.Then)
					{
						ExecuteStatement(scope, thenStatement);
					}

					return;
				}

				foreach (var (cond, body) in ifStatement.ElseIf)
				{
					if (cond.Evaluate(context).ToBoolean())
					{
						foreach (var bodyStatement in body)
						{
							ExecuteStatement(scope, bodyStatement);
						}

						return;
					}
				}

				if (ifStatement.Else != null)
				{
					foreach (var bodyStatement in ifStatement.Else)
					{
						ExecuteStatement(scope, bodyStatement);
					}
				}
				return;
			}

			// for.
			if (statement is ForStatement forStatement)
			{
				var loopScope = new Scope(scope);

				if (forStatement.Initializer != null)
					ExecuteStatement(loopScope, forStatement.Initializer);

				while (true)
				{
					var condition = forStatement.Condition != null ? forStatement.Condition.Evaluate(new Context(this, loopScope)).ToBoolean() : true;
					if (!condition)
						break;

					try
					{
						foreach (var bodyStatement in forStatement.Body)
							ExecuteStatement(loopScope, bodyStatement);
					}
					catch (ContinueTrigger)
					{
						// continue → 다음 반복으로
					}
					catch (BreakTrigger)
					{
						break;
					}

					// 루프가 끝나기전에 처리.
					if (forStatement.Post != null)
					{
						forStatement.Post.Evaluate(new Context(this, loopScope));
					}
				}
				return;
			}

			// switch.
			if (statement is SwitchStatement switchStmt)
			{
				var ctx = new Context(this, scope);
				var value = switchStmt.Expression.Evaluate(ctx);

				bool matched = false;
				foreach (var (label, body) in switchStmt.Cases)
				{
					if (matched || ValuesEqual(value, label.Evaluate(ctx)))
					{
						matched = true;
						try
						{
							foreach (var st in body)
								ExecuteStatement(scope, st);
						}
						catch (BreakTrigger)
						{
							break;
						}
					}
				}

				if (!matched && switchStmt.DefaultBody != null)
				{
					foreach (var st in switchStmt.DefaultBody)
						ExecuteStatement(scope, st);
				}
				return;
			}

			// break.
			if (statement is BreakStatement)
				throw new BreakTrigger();

			// continue.
			if (statement is ContinueStatement)
				throw new ContinueTrigger();

			throw new Exception("지원하지 않는 문장");
		}

		/// <summary>
		/// switch-case 비교용 헬퍼.
		/// </summary>
		private static bool ValuesEqual(Variable a, Variable b)
		{
			if (a.Type != b.Type)
				return false;

			switch (a.Type)
			{
				case ValueType.Null: return true;
				case ValueType.Boolean: return a.ToBoolean() == b.ToBoolean();
				case ValueType.Number: return a.ToNumber() == b.ToNumber();
				case ValueType.String: return a.ToString() == b.ToString();
				case ValueType.Function: return a.ToFunction() == b.ToFunction();
				default: return false;
			}
		}
	}
}