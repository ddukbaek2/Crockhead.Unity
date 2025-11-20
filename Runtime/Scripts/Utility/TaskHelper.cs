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
		/// 코루틴 태스크 실행. (메인 쓰레드 / 엔진 매니지드 타이밍)
		/// </summary>
		public static Task StartForeground(IEnumerator routine)
		{
			// 코루틴 실행 후 태스크 완료 처리.
			static IEnumerator Routine(TaskCompletionSource<bool> taskCompletionSource, IEnumerator routine)
			{
				yield return routine;
				taskCompletionSource.SetResult(true);
				yield break;
			}

			var taskCompletionSource = new TaskCompletionSource<bool>();
			UnityRuntime.Instance.StartCoroutine(Routine(taskCompletionSource, routine));
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