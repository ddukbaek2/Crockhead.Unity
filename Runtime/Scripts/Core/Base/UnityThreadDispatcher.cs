using Crockhead.Core;
using System;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 쓰레드 동기화 처리기.
	/// </summary>
	public static class UnityThreadDispatcher
	{
		/// <summary>
		/// 쓰레드 디스패쳐.
		/// </summary>
		private static ThreadDispatcher s_ThreadDispatcher;

		/// <summary>
		/// 생성.
		/// </summary>
		internal static void Create()
		{
			Destroy();
			s_ThreadDispatcher = new ThreadDispatcher();
		}

		/// <summary>
		/// 파괴.
		/// </summary>
		internal static void Destroy()
		{
			if (s_ThreadDispatcher == null || s_ThreadDispatcher.IsDisposed)
				return;

			Disposables.Dispose(s_ThreadDispatcher);
			s_ThreadDispatcher = null;
		}

		/// <summary>
		/// 호출 요청.
		/// </summary>
		public static void Post(Action action)
		{
			if (s_ThreadDispatcher == null || s_ThreadDispatcher.IsDisposed)
			{
				action?.Invoke();
				return;
			}

			s_ThreadDispatcher.Post(action);
		}
	}
}