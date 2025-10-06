namespace Crockhead.Scripting
{
	/// <summary>
	/// 함수.
	/// </summary>
	public sealed class UserFunction
	{
		/// <summary>
		/// 
		/// </summary>
		public FunctionDefinition Definition { get; }

		/// <summary>
		/// 
		/// </summary>
		public Environment CapturedEnvironment { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UserFunction(FunctionDefinition definition, Environment capturedEnvironment)
		{
			Definition = definition;
			CapturedEnvironment = capturedEnvironment;
		}
	}
}
