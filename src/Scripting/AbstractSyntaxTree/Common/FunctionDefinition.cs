using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 실제 함수 데이터.
	/// </summary>
	public sealed class FunctionDefinition
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
		/// 구문 목록 프로퍼티.
		/// </summary>
		public List<IStatement> Body { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FunctionDefinition() : this(string.Empty, new List<string>(), new List<IStatement>())
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FunctionDefinition(string name, List<string> parameters, List<IStatement> body)
		{
			Name = name;
			Parameters = parameters;
			Body = body;
		}
	}
}