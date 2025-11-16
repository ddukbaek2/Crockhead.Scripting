using System;

namespace Crockhead.Scripting
{
	/// <summary>
	/// break 유발자.
	/// </summary>
	public sealed class BreakTrigger : Exception
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public BreakTrigger() : base()
		{
		}
	}
}
