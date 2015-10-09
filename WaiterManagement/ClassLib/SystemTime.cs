using System;

namespace ClassLib
{
	public static class SystemTime
	{
		private static DateTime? _setTime;

		public static DateTime Now { get { return _setTime ?? DateTime.Now; } }

		public static void SetTime(DateTime time)
		{
			_setTime = time;
		}

		public static void Reset()
		{
			_setTime = null;
		}
	}
}