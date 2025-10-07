using System;

namespace Crockhead.Scripting
{
	/// <summary>
	/// continue 유발자.
	/// </summary>
	public sealed class ContinueTrigger : Exception
	{
		public ContinueTrigger() : base() { }
	}
}
