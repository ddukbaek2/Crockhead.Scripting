namespace Crockhead.Scripting
{
	/// <summary>
	/// 멤버 대입 표현식.
	/// <para>target.name = expr</para>
	/// </summary>
	public sealed class MemberAssignmentExpression : IExpression
	{
		/// <summary>
		/// 멤버 접근 표현식.
		/// </summary>
		public MemberAccessExpression Left { get; }

		/// <summary>
		/// 우항 표현식.
		/// </summary>
		public IExpression Right { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public MemberAssignmentExpression(MemberAccessExpression left, IExpression right)
		{
			Left = left;
			Right = right;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Variable Evaluate(Context context)
		{
			var variable = Right.Evaluate(context);
			Left.Set(context, variable);
			return variable;
		}
	}
}
