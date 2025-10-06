namespace Crockhead.Scripting
{
	/// <summary>
	/// 반환 구문.
	/// </summary>
	public sealed class ReturnStatement : IStatement
	{
		/// <summary>
		/// 표현식 프로퍼티.
		/// </summary>
		public IExpression Expression { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ReturnStatement(IExpression expression)
		{
			Expression = expression;
		}
	}
}
