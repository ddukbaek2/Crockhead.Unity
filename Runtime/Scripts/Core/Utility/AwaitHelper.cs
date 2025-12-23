using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 비동기 대기 유틸리티.
	/// </summary>
	public static class AwaitHelper
	{
		/// <summary>
		/// 메인 쓰레드에서 엔진 매니지드 타이밍에 액션 수행.
		/// </summary>
		public static Task RunAsync(Func<Task> action)
		{
			// 코루틴 실행 후 태스크 완료 처리.
			static IEnumerator Process(TaskCompletionSource<bool> taskCompletionSource, Action action)
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
		/// 메인 쓰레드에서 엔진 매니지드 타이밍에 태스크 형태로 코루틴 수행.
		/// </summary>
		public static Task RunAsync(IEnumerator routine)
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
	}
}