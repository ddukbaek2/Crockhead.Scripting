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
	}
}
