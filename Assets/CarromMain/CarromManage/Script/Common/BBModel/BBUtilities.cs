using System;
using System.Threading;

namespace BBModel
{
	public class BBUtilities
	{
		[ThreadStatic]
		private static Random RandomInstance;

		public static Random ThreadSafeRandom
		{
			get
			{
				Random result;
				if ((result = RandomInstance) == null)
				{
					result = (RandomInstance = new Random(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId));
				}
				return result;
			}
		}
	}
}
