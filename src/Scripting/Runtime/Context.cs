namespace Crockhead.Scripting
{
	/// <summary>
	/// 표현식 객체가 평가시 변수 접근이나 함수 호출시 사용되는 컨텍스트.
	/// <para>현재 스크립트 실행 주체와 표현식 객체를 간접적으로 연결하기 위한 설계적 용도.</para>
	/// </summary>
	public sealed class Context
	{
		/// <summary>
		/// 실행 주체.
		/// </summary>
		private readonly Session m_Session;

		/// <summary>
		/// 현재 스코프.
		/// </summary>
		private readonly Scope m_Scope;

		/// <summary>
		/// 실행 주체 프로퍼티.
		/// </summary>
		public Session Session => m_Session;

		/// <summary>
		/// 현재 영역 프로퍼티.
		/// </summary>
		public Scope Scope => m_Scope;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Context(Session session, Scope scope)
		{
			m_Session = session;
			m_Scope = scope;
		}

		/// <summary>
		/// 변수 설정.
		/// </summary>
		public void SetVariable(string name, Variable variable)
		{
			m_Scope.SetVariable(name, variable);
		}

		/// <summary>
		/// 변수 반환.
		/// </summary>
		public Variable GetVariable(string name)
		{
			return m_Scope.GetVariable(name);
		}

		/// <summary>
		/// 변수 반환 시도.
		/// </summary>
		public bool TryGetVariable(string name, out Variable variable)
		{
			// 암시적 this 멤버 탐색
			if (m_Scope.TryGetVariable("this", out var __this) && __this.Type == ValueType.Struct)
			{
				var __sv = (StructValue)__this.Value;
				if (__sv.Fields.TryGetValue(name, out variable)) return true;
			}

			return m_Scope.TryGetVariable(name, out variable);
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Variable Call(string name, Variable[] parameters)
		{
			if (TryGetVariable(name, out var variable) && variable.Type == ValueType.Function)
			{
				var functionValue = (FunctionValue)variable.Value;
				return m_Session.Call(functionValue.Value, parameters);
			}
			return m_Session.Call(name, parameters);
		}

		/// <summary>
		/// 함수 호출.
		/// </summary>
		public Variable Call(Function function, Variable[] parameters)
		{
			return m_Session.Call(function, parameters);
		}

		/// <summary>
		/// 함수 호출. (객체를 통한 메서드 호출)
		/// </summary>
		public Variable Call(Variable targetVariable, Function function, Variable[] parameters)
		{
			return m_Session.Call(targetVariable, function, parameters);
		}
	}
}
