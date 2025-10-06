using System;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 실행 환경.
	/// </summary>
	public sealed class Environment
	{
		private readonly Dictionary<string, Value> m_Variables;
		private readonly Environment m_Parent;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Environment() : this(null)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Environment(Environment parent)
		{
			m_Variables = new Dictionary<string, Value>();
			m_Parent = parent;
		}

		/// <summary>
		/// 반환 시도.
		/// </summary>
		public bool TryGet(string name, out Value value)
		{
			if (m_Variables.TryGetValue(name, out value))
				return true;
			if (m_Parent != null)
				return m_Parent.TryGet(name, out value);

			value = Value.Null();
			return false;
		}

		/// <summary>
		/// 반환.
		/// </summary>
		public Value Get(string name)
		{
			if (TryGet(name, out var value))
				return value;
			throw new Exception($"변수 '{name}' 를 찾을 수 없습니다.");
		}

		/// <summary>
		/// 로컬 설정.
		/// </summary>
		public void SetLocal(string name, Value value)
		{
			m_Variables[name] = value;
		}

		/// <summary>
		/// 설정.
		/// </summary>
		public void Set(string name, Value value)
		{
			if (m_Variables.ContainsKey(name))
			{
				m_Variables[name] = value;
				return;
			}

			if (m_Parent != null && m_Parent.Exists(name))
			{
				m_Parent.Set(name, value);
				return;
			}

			m_Variables[name] = value;
		}

		/// <summary>
		/// 존재 여부 반환.
		/// </summary>
		private bool Exists(string name)
		{
			if (m_Variables.ContainsKey(name))
				return true;

			return m_Parent != null && m_Parent.Exists(name);
		}
	}
}