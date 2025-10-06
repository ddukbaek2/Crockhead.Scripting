namespace Crockhead.Scripting
{
	/// <summary>
	/// 표현식 인터페이스.
	/// </summary>
	public interface IExpression
	{
		/// <summary>
		/// 평가.
		/// </summary>
		Value Evaluate(ExpressionView expressionView);
	}
}