using Crockhead.Core;
using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 구조체 인스턴스 값.
	/// </summary>
	public sealed class StructValue : Value
	{
		/// <summary>
		/// 
		/// </summary>
		public string TypeName { get; }

		/// <summary>
		/// 
		/// </summary>
		public override ValueType Type => ValueType.Struct;

		/// <summary>
		/// 
		/// </summary>
		public Dictionary<string, Variable> Fields { get; }

		/// <summary>
		/// 
		/// </summary>
		public StructValue(string typeName, Dictionary<string, Variable> fields)
		{
			TypeName = typeName;
			Fields = fields;
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
			return "[" + TypeName + "]";
		}
	}
}
