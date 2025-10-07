using System;

namespace Crockhead.Scripting
{
	/// <summary>
	/// break 유발자.
	/// </summary>
	public sealed class BreakTrigger : Exception
	{
		public BreakTrigger() : base() { }
	}
}
