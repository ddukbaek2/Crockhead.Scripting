using System.Collections.Generic;

namespace Crockhead.Scripting
{
	/// <summary>구조체 타입 값.</summary>
	public sealed class StructTypeValue : Value
	{
		public string Name { get; }
		public List<string> Fields { get; }
		public Dictionary<string, FunctionDefinition> Methods { get; }

		public StructTypeValue(string name, List<string> fields, Dictionary<string, FunctionDefinition> methods)
		{
			Name = name;
			Fields = fields;
			Methods = methods;
		}

		public override bool ToBoolean() { return true; }
		public override Number ToNumber() { return Number.Zero; }
		public override string ToString() { return Name; }
	}
}
