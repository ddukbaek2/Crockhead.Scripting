namespace Crockhead.Scripting
{
	/// <summary>
	/// 함수.
	/// </summary>
	public sealed class Function
	{
		/// <summary>
		/// 함수 정의.
		/// </summary>
		public FunctionDefinition Definition { get; }

		/// <summary>
		/// 현재 스코프.
		/// </summary>
		public Scope CapturedScope { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Function(FunctionDefinition definition, Scope capturedScope)
		{
			Definition = definition;
			CapturedScope = capturedScope;
		}

		/// <summary>
		/// 동일 여부 반환.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is Function function)
			{
				return Definition.Name == function.Definition.Name;
			}

			return base.Equals(obj);
		}
	}
}
