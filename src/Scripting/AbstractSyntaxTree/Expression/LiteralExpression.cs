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
		public Variable Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LiteralExpression(Variable value)
		{
			Value = value;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Variable Evaluate(ExpressionContext context)
		{
			return Value;
		}
	}
}