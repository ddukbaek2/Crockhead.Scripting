namespace Crockhead.Scripting
{
	/// <summary>
	/// 변수 표현식.
	/// </summary>
	public sealed class VariableExpression : IExpression
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public VariableExpression(string name)
		{
			Name = name;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Variable Evaluate(ExpressionContext context)
		{
			return context.GetVariable(Name);
		}
	}
}