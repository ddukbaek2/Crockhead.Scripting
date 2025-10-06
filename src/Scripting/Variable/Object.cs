using System;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 객체.
	/// </summary>
	public abstract class Object
	{
		/// <summary>
		/// 타입.
		/// </summary>
		public abstract string Type { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Object()
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
