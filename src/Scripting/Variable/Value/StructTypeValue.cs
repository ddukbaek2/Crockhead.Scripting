using Crockhead.Core;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 구조체 타입 값.
	/// </summary>
	public sealed class StructTypeValue : Value
	{
		/// <summary>
		/// 
		/// </summary>
		public override ValueType Type => ValueType.StructType;

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 
		/// </summary>
		public List<string> Fields { get; }

		/// <summary>
		/// 
		/// </summary>
		public Dictionary<string, FunctionDefinition> Methods { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public StructTypeValue(string name, List<string> fields, Dictionary<string, FunctionDefinition> methods)
		{
			Name = name;
			Fields = fields;
			Methods = methods;
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool ToBoolean()
		{
			return true;
		}

		/// <summary>
		/// 숫자 타입 변환.
		/// </summary>
		public override Number ToNumber()
		{
			return Number.Zero;
		}

		/// <summary>
		/// 
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
	}
}
