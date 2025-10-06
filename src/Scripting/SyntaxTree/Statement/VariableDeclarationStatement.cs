namespace Crockhead.Scripting
{
	/// <summary>
	/// 변수 정의 구문.
	/// </summary>
	public sealed class VariableDeclarationStatement : IStatement
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 표현식 프로퍼티.
		/// </summary>
		public IExpression Initializer { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public VariableDeclarationStatement(string name, IExpression initializer)
		{
			Name = name;
			Initializer = initializer;
		}
	}
}
