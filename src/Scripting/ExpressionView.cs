namespace Crockhead.Scripting
{
	/// <summary>
	/// 표현식 뷰.
	/// </summary>
	public sealed class ExpressionView
	{
		private readonly VirtualMachine m_RootVirtualMachine;
		private readonly Environment m_CurrentEnvironment;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ExpressionView(VirtualMachine rootVirtualMachine, Environment currentEnvironment)
		{
			this.m_RootVirtualMachine = rootVirtualMachine;
			this.m_CurrentEnvironment = currentEnvironment;
		}

		/// <summary>
		/// 변수 설정.
		/// </summary>
		public void SetVariable(string name, Value value)
		{
			m_CurrentEnvironment.Set(name, value);
		}

		/// <summary>
		/// 변수 반환.
		/// </summary>
		public Value GetVariable(string name)
		{
			return m_CurrentEnvironment.Get(name);
		}

		/// <summary>
		/// 변수 반환 시도.
		/// </summary>
		public bool TryGetVariable(string name, out Value value)
		{
			return m_CurrentEnvironment.TryGet(name, out value);
		}

		/// <summary>
		/// 
		/// </summary>
		public Value CallByNameOrVariable(string name, Value[] parameters)
		{
			if (TryGetVariable(name, out var value) && value.Type == ValueType.Function)
			{
				var functionObject = (Function)value.Object;
				return m_RootVirtualMachine.Call(functionObject.Value, parameters);
			}
			return m_RootVirtualMachine.Call(name, parameters);
		}
	}
}