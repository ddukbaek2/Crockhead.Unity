using System;
using System.Threading;


namespace Crockhead.Unity
{
	/// <summary>
	/// 메인 쓰레드.
	/// </summary>
	public static class UnityMainThread
	{
		/// <summary>
		/// 컨텍스트.
		/// </summary>
		public static SynchronizationContext Context { private set; get; }

		/// <summary>
		/// 초기화. (메인쓰레드에서 호출)
		/// </summary>
		public static void Initialize()
		{
			Context = SynchronizationContext.Current;
		}

		/// <summary>
		/// 메인쓰레드에서 호출.
		/// </summary>
		public static void Post(Action action)
		{
			if (action == null)
				return;

			var context = Context;
			if (context != null)
			{
				void Post(object state)
				{
					action.Invoke();
				}

				context.Post(Post, null);
			}
			else
			{
				action.Invoke();
			}
		}
	}
}