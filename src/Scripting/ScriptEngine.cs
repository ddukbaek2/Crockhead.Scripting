using System;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 스크립트 엔진.
	/// </summary>
	public sealed class ScriptEngine
	{
		/// <summary>
		/// 바인딩 된 C# 함수 목록.
		/// </summary>
		private Dictionary<string, OnBindingFunctionDelegate> m_BindingFunctions;

		/// <summary>
		/// 스크립트 로드 된 전역 함수 목록.
		/// </summary>
		private Dictionary<string, FunctionDefinition> m_Functions;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ScriptEngine()
		{
			m_BindingFunctions = new Dictionary<string, OnBindingFunctionDelegate>();
			m_Functions = new Dictionary<string, FunctionDefinition>();
		}

		/// <summary>
		/// 스크립트 불러오기.
		/// </summary>
		public void Load(string script, bool unload = true)
		{
			if (unload)
				Unload();

			var parser = new Parser(script);
			parser.Parse(out m_Functions);
		}

		/// <summary>
		/// 스크립트 언로드.
		/// </summary>
		public void Unload()
		{
			m_Functions.Clear();
		}

		/// <summary>
		/// 바인딩 함수 추가.
		/// </summary>
		public void AddFunction(string name, OnBindingFunctionDelegate function)
		{
			m_BindingFunctions[name] = function;
		}

		/// <summary>
		/// 바인딩 함수 제거.
		/// </summary>
		public void RemoveFunction(string name)
		{
			m_BindingFunctions.Remove(name);
		}

		/// <summary>
		/// 모든 바인딩 함수 제거.
		/// </summary>
		public void RemoveAllFunctions()
		{
			m_BindingFunctions.Clear();
		}

		/// <summary>
		/// 로드된 스크립트 실행. (함수명으로 실행)
		/// </summary>
		public Variable Execute(string name, params Variable[] parameters)
		{
			if (!m_Functions.TryGetValue(name, out var functionDefinition))
				throw new Exception(name);

			var session = new Session(m_BindingFunctions, m_Functions);
			return session.Call(functionDefinition, parameters);
		}

		/// <summary>
		/// 문법 유효성 검사.
		/// </summary>
		public List<SyntaxDiagnostic> ValidateSyntax(string script)
		{
			var diagnostics = new List<SyntaxDiagnostic>();
			try
			{
				var parser = new Parser(script);
				parser.Parse(out _);
			}
			catch (SyntaxException exception)
			{
				diagnostics.Add(new SyntaxDiagnostic(exception.Line, exception.Column, exception.Message));
			}

			return diagnostics;
		}
	}
}
