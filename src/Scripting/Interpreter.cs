using System;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 인터프리터.
	/// </summary>
	public sealed class Interpreter
	{
		/// <summary>
		/// 네이티브 함수 목록.
		/// </summary>
		private Dictionary<string, NativeFunction> m_NativeFunctions;

		/// <summary>
		/// 함수 목록.
		/// </summary>
		private Dictionary<string, FunctionDefinition> m_Functions;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Interpreter()
		{
			m_NativeFunctions = new Dictionary<string, NativeFunction>();
			m_Functions = new Dictionary<string, FunctionDefinition>();
		}

		/// <summary>
		/// 함수 추가.
		/// </summary>
		public void AddFunction(string name, NativeFunction function)
		{
			m_NativeFunctions[name] = function;
		}

		/// <summary>
		/// 함수 제거.
		/// </summary>
		public void RemoveFunction(string name)
		{
			m_NativeFunctions.Remove(name);
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Value Call(string name, params Value[] parameters)
		{
			if (!m_Functions.TryGetValue(name, out var functionDefinition))
				throw new Exception(name);

			var machine = new VirtualMachine(m_NativeFunctions, m_Functions);
			return machine.Call(functionDefinition, parameters);
		}

		/// <summary>
		/// 문법 검사.
		/// </summary>
		public List<SyntaxDiagnostic> ValidateSyntax(string script)
		{
			var diagnostics = new List<SyntaxDiagnostic>();
			try
			{
				var parser = new Parser(script);
				parser.ParseAllFunctions(out _);
			}
			catch (SyntaxException exception)
			{
				diagnostics.Add(new SyntaxDiagnostic(exception.Line, exception.Column, exception.Message));
			}

			return diagnostics;
		}

		/// <summary>
		/// 스크립트 추가.
		/// </summary>
		public void Load(string script)
		{
			var parser = new Parser(script);
			parser.ParseAllFunctions(out m_Functions);
		}
	}
}
