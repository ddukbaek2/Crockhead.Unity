using Crockhead.Core;
using System;
using System.Threading;
using System.Threading.Tasks;


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
		private static ThreadPostDispatcher s_ThreadPostDispatcher;

		/// <summary>
		/// 생성.
		/// </summary>
		public static void Create()
		{
			Dispose();

			// 대상 쓰레드일 때 생성해야 대상 쓰레드의 컨텍스트를 정상 캡쳐함.
			s_ThreadPostDispatcher = new ThreadPostDispatcher();
		}

		/// <summary>
		/// 파괴.
		/// </summary>
		public static void Dispose()
		{
			if (s_ThreadPostDispatcher == null || s_ThreadPostDispatcher.IsDisposed)
				return;

			Disposables.Dispose(s_ThreadPostDispatcher);
			s_ThreadPostDispatcher = null;
		}

		/// <summary>
		/// 메인 쓰레드에서 동기 호출 요청.
		/// </summary>
		public static void Post(Action action)
		{
			if (s_ThreadPostDispatcher == null || s_ThreadPostDispatcher.IsDisposed)
			{
				// 쓰레드 디스패쳐가 없을 경우 현재 쓰레드에서 즉시 호출.
				action?.Invoke();
				return;
			}

			// 호출 요청.
			s_ThreadPostDispatcher.Post(action);
		}

		/// <summary>
		/// 메인 쓰레드에서 동기 함수를 비동기 호출 요청.
		/// </summary>
		public static Task PostAsync(Action action)
		{
			// 호출 요청.
			var taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
			UnityThreadDispatcher.Post(() =>
			{
				try
				{
					action?.Invoke();
					taskCompletionSource.SetResult(null);
				}
				catch (Exception exception)
				{
					taskCompletionSource.SetException(exception);
				}
			});

			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 메인 쓰레드에서 비동기 함수를 비동기 호출 요청.
		/// </summary>
		public static Task PostAsync(Func<Task> taskFactory)
		{
			if (s_ThreadPostDispatcher == null || s_ThreadPostDispatcher.IsDisposed)
				throw new InvalidOperationException("ThreadDispatcher is Invalid.");

			static async Task RunAsync(Func<Task> taskFactory, TaskCompletionSource<object> taskCompletionSource)
			{
				try
				{
					var task = taskFactory.Invoke();
					await task.ConfigureAwait(true);
					taskCompletionSource.SetResult(null);
				}
				catch (Exception exception)
				{
					taskCompletionSource.SetException(exception);
				}
			}

			// 호출 요청.
			var taskCompletionSource = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
			UnityThreadDispatcher.Post(() => _ = RunAsync(taskFactory, taskCompletionSource));

			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 메인 쓰레드에서 비동기 함수를 비동기 호출 요청.
		/// </summary>
		public static Task<T> PostAsync<T>(Func<Task<T>> taskFactory)
		{
			if (s_ThreadPostDispatcher == null || s_ThreadPostDispatcher.IsDisposed)
				throw new InvalidOperationException("ThreadDispatcher is Invalid.");

			static async Task RunAsync(Func<Task<T>> taskFactory, TaskCompletionSource<T> taskCompletionSource)
			{
				try
				{
					var task = taskFactory.Invoke();
					var result = await task.ConfigureAwait(true);
					taskCompletionSource.SetResult(result);
				}
				catch (Exception exception)
				{
					taskCompletionSource.SetException(exception);
				}
			}

			// 호출 요청.
			var taskCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
			UnityThreadDispatcher.Post(() => _ = RunAsync(taskFactory, taskCompletionSource));

			return taskCompletionSource.Task;
		}
	}
}