namespace Crockhead.Scripting
{
	/// <summary>
	/// 표현식 구문.
	/// </summary>
	public sealed class ExpressionStatement : IStatement
	{
		/// <summary>
		/// 표현식 프로퍼티.
		/// </summary>
		public IExpression Expression { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ExpressionStatement(IExpression expression)
		{
			Expression = expression;
		}
	}
}