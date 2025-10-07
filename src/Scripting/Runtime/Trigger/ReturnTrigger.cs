using System;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 반환 유발자.
	/// <para>Exception 기반으로 throw 된 ReturnTrigger가 값을 스크립트 구문 해석 중에 모든 스택을 건너뛰고 즉시 반환 시킨다.</para>
	/// </summary>
	public sealed class ReturnTrigger : Exception
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public Variable Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ReturnTrigger(Variable value) : base()
		{
			Value = value;
		}
	}
}