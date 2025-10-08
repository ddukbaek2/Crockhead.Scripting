using System;
using System.Collections.Generic;
using System.Globalization;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 스크립트에서 변수로 맵핑되는 데이터 객체.
	/// <para>이름과 영역은 세션쪽과 스코프쪽에서 관리됨.</para>
	/// </summary>
	public sealed class Variable
	{
		/// <summary>
		/// 값 종류 프로퍼티.
		/// </summary>
		public ValueType Type { get; }

		/// <summary>
		/// 실제 값 프로퍼티. (객체)
		/// </summary>
		public Value Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Variable(ValueType type, Value value)
		{
			Type = type;
			Value = value;
		}

		/// <summary>
		/// 논리 변환.
		/// </summary>
		public bool ToBoolean()
		{
			return Value.ToBoolean();
		}

		/// <summary>
		/// 숫자 변환.
		/// </summary>
		public Number ToNumber()
		{
			if (Type == ValueType.Number)
			{
				return ((NumberValue)Value).Value;
			}
			else if (Type == ValueType.Boolean)
			{
				return ((BooleanValue)Value).Value ? Scripting.Number.Parse(1) : Scripting.Number.Parse(0);
			}
			else if (Type == ValueType.String)
			{
				if (double.TryParse(Value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
					return Scripting.Number.Parse(value.ToString(CultureInfo.InvariantCulture));
			}
			
			throw new InvalidCastException();
		}

		/// <summary>
		/// 함수 변환.
		/// </summary>
		public Function ToFunction()
		{
			if (Type == ValueType.Function)
			{
				return ((FunctionValue)Value).Value;
			}

			throw new InvalidCastException();
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			return Value.ToString();
		}

		/// <summary>
		/// 널 생성.
		/// </summary>
		public static Variable Null()
		{
			return new Variable(ValueType.Null, NullValue.SharedInstance);
		}

		/// <summary>
		/// 논리 생성.
		/// </summary>
		public static Variable Boolean(bool value)
		{
			return new Variable(ValueType.Boolean, new BooleanValue(value));
		}

		/// <summary>
		/// 숫자 생성.
		/// </summary>
		public static Variable Number(Number value)
		{
			return new Variable(ValueType.Number, new NumberValue(value));
		}

		/// <summary>
		/// 숫자 생성.
		/// </summary>
		public static Variable Number(long value)
		{
			return new Variable(ValueType.Number, new NumberValue(Scripting.Number.Parse(value)));
		}

		/// <summary>
		/// 숫자 생성.
		/// </summary>
		public static Variable Number(double value)
		{
			var text = value.ToString(CultureInfo.InvariantCulture);
			return new Variable(ValueType.Number, new NumberValue(Scripting.Number.Parse(text)));
		}

		/// <summary>
		/// 문자열 생성.
		/// </summary>
		public static Variable String(string value)
		{
			return new Variable(ValueType.String, new StringValue(value));
		}

		/// <summary>
		/// 함수 생성.
		/// </summary>
		public static Variable Function(Function value)
		{
			return new Variable(ValueType.Function, new FunctionValue(value));
		}

		/// <summary>
		/// 구조체 생성.
		/// </summary>
		public static Variable StructType(string name, List<string> fields, Dictionary<string, FunctionDefinition> methods)
		{
			return new Variable(ValueType.StructType, new StructTypeValue(name, fields, methods));
		}

		/// <summary>
		/// 구조체 생성.
		/// </summary>
		public static Variable Struct(string name, Dictionary<string, Variable> fields)
		{
			return new Variable(ValueType.Struct, new StructValue(name, fields));
		}
	}
}