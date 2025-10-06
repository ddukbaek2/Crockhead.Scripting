namespace Crockhead.Scripting
{
	/// <summary>
	/// 대입 표현식.
	/// </summary>
	public sealed class AssignmentExpression : IExpression
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 표현식 프로퍼티.
		/// </summary>
		public IExpression Expression { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssignmentExpression(string name, IExpression expression)
		{
			Name = name;
			Expression = expression;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Value Evaluate(ExpressionView expressionView)
		{
			var value = Expression.Evaluate(expressionView);
			expressionView.SetVariable(Name, value);
			return value;
		}
	}
}