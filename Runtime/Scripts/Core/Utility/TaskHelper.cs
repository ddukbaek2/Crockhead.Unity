using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 태스크 유틸리티.
	/// </summary>
	public static class TaskHelper
	{
		/// <summary>
		/// 유니티 비동기 대기 객체를 태스크로 변환.
		/// </summary>
		public static Task WaitForCompletion(this AsyncOperation asyncOperation)
		{
			var taskCompletionSource = new TaskCompletionSource<bool>();
			void Completed(AsyncOperation asyncOperation)
			{
				taskCompletionSource.SetResult(true);
			}

			asyncOperation.completed += Completed;
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 유니티 비동기 대기 객체를 태스크로 변환.
		/// </summary>
		public static Task<T[]> WaitForCompletion<T>(this AsyncInstantiateOperation<T> asyncInstantiateOperation)
		{
			var taskCompletionSource = new TaskCompletionSource<T[]>();
			void Completed(AsyncOperation asyncOperation)
			{
				var asyncInstantiateOperation = asyncOperation as AsyncInstantiateOperation<T>;
				taskCompletionSource.SetResult(asyncInstantiateOperation.Result);
			}

			asyncInstantiateOperation.completed += Completed;
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 코루틴 태스크 실행. (메인 쓰레드 / 엔진 매니지드 타이밍)
		/// </summary>
		public static Task StartForeground(this IEnumerator routine)
		{
			// 코루틴 실행 후 태스크 완료 처리.
			static IEnumerator Process(TaskCompletionSource<bool> taskCompletionSource, IEnumerator routine)
			{
				yield return routine;
				taskCompletionSource.SetResult(true);
				yield break;
			}

			var taskCompletionSource = new TaskCompletionSource<bool>();
			UnityRuntime.Instance.StartCoroutine(Process(taskCompletionSource, routine));
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 일반 태스크 실행. (무작위 백그라운드 쓰레드)
		/// </summary>
		public static Task StartBackground(Action action)
		{
			var task = Task.Run(action);
			return task;
		}

		/// <summary>
		/// 일반 태스크 실행. (무작위 백그라운드 쓰레드)
		/// </summary>
		public static Task<TResult> StartBackground<TResult>(Func<TResult> action)
		{
			var task = Task.Run(action);
			return task;
		}
	}
}