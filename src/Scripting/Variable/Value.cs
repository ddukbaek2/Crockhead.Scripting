using System;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 객체의 값.
	/// </summary>
	public abstract class Value
	{
		/// <summary>
		/// 값 종류.
		/// </summary>
		public abstract ValueType Type { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Value()
		{
		}

		/// <summary>
		/// 논리 타입 변환.
		/// </summary>
		public virtual bool ToBoolean()
		{
			throw new InvalidOperationException();
		}

		/// <summary>
		/// 문자열 변환.
		/// </summary>
		public override string ToString()
		{
			return $"<{Type}>";
		}
	}
}
