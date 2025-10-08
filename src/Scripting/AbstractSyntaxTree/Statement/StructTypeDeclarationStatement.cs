using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 전역 구조체 타입 선언.
	/// </summary>
	public sealed class StructTypeDeclarationStatement : IStatement
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 멤버 필드 목록 프로퍼티.
		/// </summary>
		public List<(string FieldName, string TypeName)> Fields { get; }

		/// <summary>
		/// 멤버 메서드 목록 프로퍼티.
		/// </summary>
		public Dictionary<string, FunctionDefinition> Methods { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public StructTypeDeclarationStatement(string name, List<(string,string)> fields, Dictionary<string, FunctionDefinition> methods)
		{
			Name = name;
			Fields = fields;
			Methods = methods;
		}
	}
}
