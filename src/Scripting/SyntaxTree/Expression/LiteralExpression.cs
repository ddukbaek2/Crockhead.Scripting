namespace Crockhead.Scripting
{
	/// <summary>
	/// 상수 표현식.
	/// </summary>
	public sealed class LiteralExpression : IExpression
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public Value Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LiteralExpression(Value value)
		{
			Value = value;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Value Evaluate(ExpressionView expressionView)
		{
			return Value;
		}
	}
}