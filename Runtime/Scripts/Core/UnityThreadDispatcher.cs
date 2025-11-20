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
		private static ThreadDispatcher m_ThreadDispatcher;

		/// <summary>
		/// 생성.
		/// </summary>
		internal static void Create()
		{
			Destroy();
			m_ThreadDispatcher = new ThreadDispatcher();
		}

		/// <summary>
		/// 파괴.
		/// </summary>
		internal static void Destroy()
		{
			if (m_ThreadDispatcher == null || m_ThreadDispatcher.IsDisposed)
				return;

			Disposables.Dispose(m_ThreadDispatcher);
			m_ThreadDispatcher = null;
		}

		/// <summary>
		/// 호출 요청.
		/// </summary>
		public static void Post(Action action)
		{
			if (m_ThreadDispatcher == null || m_ThreadDispatcher.IsDisposed)
			{
				action?.Invoke();
				return;
			}

			m_ThreadDispatcher.Post(action);
		}
	}
}