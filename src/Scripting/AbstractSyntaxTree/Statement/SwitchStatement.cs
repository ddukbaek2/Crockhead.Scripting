using System.Collections.Generic;

namespace Crockhead.Scripting
{
	/// <summary>
	/// 분기 구문.
	/// <para>switch (expr) { case v: ...; default: ...; }</para>
	/// </summary>
	public sealed class SwitchStatement : IStatement
	{
		/// <summary>
		/// 표현식 구문.
		/// </summary>
		public IExpression Expression { get; }

		/// <summary>
		/// 분기 구문.
		/// </summary>
		public List<(IExpression Label, List<IStatement> Body)> Cases { get; }

		/// <summary>
		/// 기본 분기 구문.
		/// </summary>
		public List<IStatement> DefaultBody { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SwitchStatement(IExpression expression, List<(IExpression, List<IStatement>)> cases, List<IStatement> defaultBody)
		{
			Expression = expression;
			Cases = cases;
			DefaultBody = defaultBody;
		}
	}
}
