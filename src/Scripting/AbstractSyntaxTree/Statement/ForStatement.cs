using System.Collections.Generic;

namespace Crockhead.Scripting
{
	/// <summary>
	/// 반복 구문.
	/// <para>for ( init ; cond ; post ) { body }</para>
	/// </summary>
	public sealed class ForStatement : IStatement
	{
		/// <summary>
		/// 초기화 표현식 프로퍼티.
		/// </summary>
		public IStatement Initializer { get; }

		/// <summary>
		/// 조건 표현식 프로퍼티.
		/// </summary>
		public IExpression Condition { get; }

		/// <summary>
		/// 현재 루프가 끝날때 처리 표현식 프로퍼티.
		/// </summary>
		public IExpression Post { get; }

		/// <summary>
		/// 루프 내에서 처리 표현식 프로퍼티.
		/// </summary>
		public List<IStatement> Body { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ForStatement(IStatement initializer, IExpression condition, IExpression post, List<IStatement> body)
		{
			Initializer = initializer;
			Condition = condition;
			Post = post;
			Body = body;
		}
	}
}
