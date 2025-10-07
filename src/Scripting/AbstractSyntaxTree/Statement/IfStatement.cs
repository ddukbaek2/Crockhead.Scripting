using System.Collections.Generic;

namespace Crockhead.Scripting
{
	/// <summary>
	/// 조건 구문.
	/// </summary>
	public sealed class IfStatement : IStatement
	{
		/// <summary>
		/// 조건 표현식 프로퍼티.
		/// </summary>
		public IExpression Condition { get; }

		/// <summary>
		/// 성공시 처리 표현식 프로퍼티.
		/// </summary>
		public List<IStatement> Then { get; }

		/// <summary>
		/// 실패시 대체 조건 표현식 프로퍼티.
		/// </summary>
		public List<(IExpression Condition, List<IStatement> Body)> ElseIf { get; }

		/// <summary>
		/// 모두 실패시 거짓일 때 처리.
		/// </summary>
		public List<IStatement> Else { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public IfStatement(IExpression condition, List<IStatement> then, List<(IExpression, List<IStatement>)> elseIf, List<IStatement> @else)
		{
			Condition = condition;
			Then = then;
			ElseIf = elseIf;
			Else = @else;
		}
	}
}
