using System;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class ReturnFlowSignal : Exception
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public Value Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ReturnFlowSignal(Value value) : base()
		{
			Value = value;
		}
	}
}