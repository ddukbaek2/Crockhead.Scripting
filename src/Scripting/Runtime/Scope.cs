using System;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 변수 공간.
	/// </summary>
	public sealed class Scope
	{
		/// <summary>
		/// 현재 공간의 변수 목록.
		/// </summary>
		private readonly Dictionary<string, Variable> m_Variables;

		/// <summary>
		/// 상위 변수 공간.
		/// </summary>
		private readonly Scope m_Parent;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Scope(Scope parent)
		{
			m_Variables = new Dictionary<string, Variable>();
			m_Parent = parent;
		}

		/// <summary>
		/// 변수 반환 시도.
		/// </summary>
		public bool TryGetVariable(string name, out Variable variable)
		{
			// 지역 변수 반환 시도.
			if (m_Variables.TryGetValue(name, out variable))
				return true;

			// 상위 지역 변수 반환 시도.
			if (m_Parent != null)
				return m_Parent.TryGetVariable(name, out variable);

			// 없으면 Null 반환.
			variable = Variable.Null();
			return false;
		}

		/// <summary>
		/// 변수 반환.
		/// </summary>
		public Variable GetVariable(string name)
		{
			if (TryGetVariable(name, out var variable))
				return variable;

			throw new Exception($"변수 '{name}' 를 찾을 수 없습니다.");
		}

		/// <summary>
		/// 지역 변수 설정.
		/// </summary>
		public void SetLocalVariable(string name, Variable variable)
		{
			m_Variables[name] = variable;
		}

		/// <summary>
		/// 지역 변수 및 상위 지역 변수 설정.
		/// </summary>
		public void SetVariable(string name, Variable variable)
		{
			// 있으면 값 교체.
			if (m_Variables.ContainsKey(name))
			{
				m_Variables[name] = variable;
				return;
			}

			// 상위 스코프에 있으면 값 교체.
			if (m_Parent != null && m_Parent.ExistsVariable(name))
			{
				m_Parent.SetVariable(name, variable);
				return;
			}

			// 지역 변수에 추가.
			SetLocalVariable(name, variable);
		}

		/// <summary>
		/// 현재 스코프에 변수 존재 여부 반환.
		/// </summary>
		private bool ExistsVariable(string name)
		{
			if (m_Variables.ContainsKey(name))
				return true;

			return m_Parent != null && m_Parent.ExistsVariable(name);
		}
	}
}