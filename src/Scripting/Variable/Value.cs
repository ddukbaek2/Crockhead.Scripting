using System;
using System.Globalization;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 값.
	/// </summary>
	public sealed class Value
	{
		/// <summary>
		/// 값 타입 프로퍼티.
		/// </summary>
		public ValueType Type { get; }

		/// <summary>
		/// 실제 값 프로퍼티. (객체)
		/// </summary>
		public Object Object { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Value(ValueType type, Object obj)
		{
			Type = type;
			Object = obj;
		}

		/// <summary>
		/// 논리 변환.
		/// </summary>
		public bool ToBoolean()
		{
			return Object.ToBoolean();
		}

		/// <summary>
		/// 숫자 변환.
		/// </summary>
		public BigRational ToNumber()
		{
			if (Type == ValueType.Number)
			{
				return ((Number)Object).Value;
			}
			else if (Type == ValueType.Boolean)
			{
				return ((Boolean)Object).Value ? BigRational.FromInteger(1) : BigRational.FromInteger(0);
			}
			else if (Type == ValueType.String)
			{
				if (double.TryParse(Object.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
					return BigRational.ParseDecimal(value.ToString(CultureInfo.InvariantCulture));
			}
			
			throw new InvalidCastException();
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			return Object.ToString();
		}

		/// <summary>
		/// 널 생성.
		/// </summary>
		public static Value Null()
		{
			return new Value(ValueType.Null, Scripting.Null.Instance);
		}

		/// <summary>
		/// 논리 생성.
		/// </summary>
		public static Value Boolean(bool value)
		{
			return new Value(ValueType.Boolean, new Boolean(value));
		}

		/// <summary>
		/// 숫자 생성.
		/// </summary>
		public static Value Number(BigRational value)
		{
			return new Value(ValueType.Number, new Number(value));
		}

		/// <summary>
		/// 숫자 생성.
		/// </summary>
		public static Value NumberFromLong(long value)
		{
			return new Value(ValueType.Number, new Number(BigRational.FromInteger(value)));
		}

		/// <summary>
		/// 숫자 생성.
		/// </summary>
		public static Value NumberFromDouble(double value)
		{
			var text = value.ToString(CultureInfo.InvariantCulture);
			return new Value(ValueType.Number, new Number(BigRational.ParseDecimal(text)));
		}

		/// <summary>
		/// 문자열 생성.
		/// </summary>
		public static Value String(string value)
		{
			return new Value(ValueType.String, new String(value));
		}

		/// <summary>
		/// 함수 생성.
		/// </summary>
		public static Value Function(UserFunction value)
		{
			return new Value(ValueType.Function, new Function(value));
		}
	}
}