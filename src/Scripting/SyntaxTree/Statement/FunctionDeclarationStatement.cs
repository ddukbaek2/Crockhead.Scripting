using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 함수 정의 구문.
	/// </summary>
	public sealed class FunctionDeclarationStatement : IStatement
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 파라메터 프로퍼티.
		/// </summary>
		public List<string> Parameters { get; }

		/// <summary>
		/// 내용 프로퍼티.
		/// </summary>
		public List<IStatement> Body { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FunctionDeclarationStatement() : this(string.Empty, new List<string>(), new List<IStatement>())
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FunctionDeclarationStatement(string name, List<string> parameters, List<IStatement> body)
		{
			Name = name;
			Parameters = parameters;
			Body = body;
		}
	}
}