using System.Collections.Generic;

namespace Crockhead.Scripting
{
	/// <summary>구조체 인스턴스 값.</summary>
	public sealed class StructValue : Value
	{
		public string TypeName { get; }
		public Dictionary<string, Variable> Fields { get; }

		public StructValue(string typeName, Dictionary<string, Variable> fields)
		{
			TypeName = typeName;
			Fields = fields;
		}

		public override bool ToBoolean() { return true; }
		public override Number ToNumber() { return Number.Zero; }
		public override string ToString() { return "[" + TypeName + "]"; }
	}
}
