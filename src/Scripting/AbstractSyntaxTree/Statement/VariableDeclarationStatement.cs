namespace Crockhead.Scripting
{
	/// <summary>
	/// 변수 선언 구문.
	/// </summary>
	public sealed class VariableDeclarationStatement : IStatement
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 초기값 표현식 프로퍼티.
		/// </summary>
		public IExpression Expression { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public VariableDeclarationStatement(string name, IExpression expression)
		{
			Name = name;
			Expression = expression;
		}
	}
}
