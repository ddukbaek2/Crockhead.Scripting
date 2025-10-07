namespace Crockhead.Scripting
{
	/// <summary>
	/// 표현식 객체가 평가시 변수 접근이나 함수 호출시 사용되는 컨텍스트.
	/// <para>현재 스크립트 실행 주체와 표현식 객체를 간접적으로 연결하기 위한 설계적 용도.</para>
	/// </summary>
	public sealed class ExpressionContext
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
		/// 생성됨.
		/// </summary>
		public ExpressionContext(Session session, Scope scope)
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
	}
}