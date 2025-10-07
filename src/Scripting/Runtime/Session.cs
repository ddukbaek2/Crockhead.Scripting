using System;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 스크립트 실행 흐름 주체.
	/// </summary>
	public class Session
	{
		/// <summary>
		/// 바인딩 전역 함수 목록.
		/// </summary>
		private readonly Dictionary<string, OnBindingFunctionDelegate> m_BindingFunctions;

		/// <summary>
		/// 전역 함수 목록.
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
			m_Scope = new Scope();

			// 최상위 함수들을 전역에 바인딩. (전역 환경을 캡처)
			foreach (var pair in m_Functions)
			{
				m_Scope.SetLocalVariable(pair.Key, Variable.Function(new Function(pair.Value, m_Scope)));
			}
		}

		/// <summary>
		/// 이름으로 함수 호출.
		/// </summary>
		public Variable Call(string functionName, Variable[] parameters)
		{
			// 바인딩 된 함수 호출.
			if (m_BindingFunctions.TryGetValue(functionName, out var function))
				return function.Invoke(parameters);

			// 스크립트 전역 함수 호출.
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
			var parameterCount = function.Definition.Parameters.Count;
			for (var i = 0; i < parameterCount; i++)
			{
				var argumentValue = (i < parameters.Length) ? parameters[i] : Variable.Null();
				localScope.SetLocalVariable(function.Definition.Parameters[i], argumentValue);
			}

			try
			{
				// 현재 함수의 구문 목록 실행.
				foreach (var statement in function.Definition.Body)
				{
					ExecuteStatement(localScope, statement);
				}
			}
			catch (ReturnTrigger returnTrigger)
			{
				return returnTrigger.Value ?? Variable.Null();
			}

			// 모든 함수는 반환값을 가지며, 지정하지 않으면 NullValue.
			return Variable.Null();
		}

		/// <summary>
		/// 구문 실행.
		/// </summary>
		private void ExecuteStatement(Scope scope, IStatement statement)
		{
			// 함수 선언 구문.
			if (statement is FunctionDeclarationStatement functionDeclaration)
			{
				var innerDefinition = new FunctionDefinition(functionDeclaration.Name, functionDeclaration.Parameters, functionDeclaration.Body);
				var function = Variable.Function(new Function(innerDefinition, scope));
				scope.SetLocalVariable(functionDeclaration.Name, function);
				return;
			}

			// 변수 선언 구문.
			if (statement is VariableDeclarationStatement variableDeclaration)
			{
				var initialValue = variableDeclaration.Initializer != null ? variableDeclaration.Initializer.Evaluate(new ExpressionContext(this, scope)) : Variable.Null();
				scope.SetLocalVariable(variableDeclaration.Name, initialValue);
				return;
			}

			// 표현식 구문.
			if (statement is ExpressionStatement expression)
			{
				expression.Expression.Evaluate(new ExpressionContext(this, scope));
				return;
			}

			// 반환 구문.
			if (statement is ReturnStatement returnStatement)
			{
				var returnTrigger = default(ReturnTrigger);
				if (returnStatement.Expression != null)
				{
					var variable = returnStatement.Expression.Evaluate(new ExpressionContext(this, scope));
					returnTrigger = new ReturnTrigger(variable);
				}
				else
				{
					var variable = Variable.Null();
					returnTrigger = new ReturnTrigger(variable);
				}

				throw returnTrigger;
			}

			throw new Exception("지원하지 않는 문장");
		}
	}
}
