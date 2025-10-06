using System;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 실행 환경.
	/// </summary>
	public class VirtualMachine
	{
		/// <summary>
		/// 바인딩 함수 목록.
		/// </summary>
		private readonly Dictionary<string, NativeFunction> m_NativeFunctions;

		/// <summary>
		/// 함수 목록.
		/// </summary>
		private readonly Dictionary<string, FunctionDefinition> m_Functions;

		/// <summary>
		/// 전역 환경.
		/// </summary>
		private readonly Environment m_GlobalEnvironment;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public VirtualMachine(Dictionary<string, NativeFunction> nativeFunctions, Dictionary<string, FunctionDefinition> functions)
		{
			m_NativeFunctions = nativeFunctions;
			m_Functions = functions;
			m_GlobalEnvironment = new Environment();

			// 최상위 함수들을 전역에 바인딩. (전역 환경을 캡처)
			foreach (var pair in m_Functions)
			{
				m_GlobalEnvironment.SetLocal(pair.Key, Value.Function(new UserFunction(pair.Value, m_GlobalEnvironment)));
			}
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Value Call(string functionName, Value[] parameters)
		{
			if (m_NativeFunctions.TryGetValue(functionName, out var native))
				return native(parameters);
			if (m_Functions.TryGetValue(functionName, out var definition))
				return Call(new UserFunction(definition, m_GlobalEnvironment), parameters);

			throw new Exception($"알 수 없는 함수: {functionName}");
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Value Call(FunctionDefinition definition, Value[] parameters)
		{
			return Call(new UserFunction(definition, m_GlobalEnvironment), parameters);
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Value Call(UserFunction function, Value[] parameters)
		{
			var localEnvironment = new Environment(function.CapturedEnvironment);
			var parameterCount = function.Definition.Parameters.Count;
			for (var i = 0; i < parameterCount; i++)
			{
				var argumentValue = (i < parameters.Length) ? parameters[i] : Value.Null();
				localEnvironment.SetLocal(function.Definition.Parameters[i], argumentValue);
			}

			try
			{
				foreach (var statement in function.Definition.Body)
					ExecuteStatement(localEnvironment, statement);
			}
			catch (ReturnFlowSignal returnFlowSignal)
			{
				return returnFlowSignal.Value ?? Value.Null();
			}

			// 모든 함수는 반환값을 가지며, 지정하지 않으면 Null.
			return Value.Null();
		}

		/// <summary>
		/// 구문 실행.
		/// </summary>
		private void ExecuteStatement(Environment environment, IStatement statement)
		{
			if (statement is FunctionDeclarationStatement functionDeclaration)
			{
				var innerDefinition = new FunctionDefinition(functionDeclaration.Name, functionDeclaration.Parameters, functionDeclaration.Body);
				var function = Value.Function(new UserFunction(innerDefinition, environment));
				environment.SetLocal(functionDeclaration.Name, function);
				return;
			}

			if (statement is VariableDeclarationStatement variableDeclaration)
			{
				var initialValue = variableDeclaration.Initializer != null
					? variableDeclaration.Initializer.Evaluate(new ExpressionView(this, environment))
					: Value.Null();
				environment.SetLocal(variableDeclaration.Name, initialValue);
				return;
			}

			if (statement is ExpressionStatement expressionStatement)
			{
				expressionStatement.Expression.Evaluate(new ExpressionView(this, environment));
				return;
			}

			if (statement is ReturnStatement returnStatement)
			{
				var value = returnStatement.Expression != null
					? returnStatement.Expression.Evaluate(new ExpressionView(this, environment))
					: Value.Null();
				throw new ReturnFlowSignal(value);
			}

			throw new Exception("지원하지 않는 문장");
		}
	}
}
