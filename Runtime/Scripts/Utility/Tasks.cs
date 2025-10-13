using System;
using System.Collections;
using System.Threading.Tasks;


namespace Crockhead.Unity
{
	/// <summary>
	/// 태스크 유틸리티.
	/// </summary>
	public static class Tasks
	{
		/// <summary>
		/// 코루틴 태스크 실행.
		/// </summary>
		public static Task StartForeground(IEnumerator routine)
		{
			// 코루틴 실행 후 태스크 완료 처리.
			static IEnumerator InternalExecuteProcess(TaskCompletionSource<bool> taskCompletionSource, IEnumerator routine)
			{
				yield return routine;
				taskCompletionSource.SetResult(true);
				yield break;
			}

			var taskCompletionSource = new TaskCompletionSource<bool>();
			UnityRunner.Instance.StartCoroutine(InternalExecuteProcess(taskCompletionSource, routine));
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 일반 태스크 실행.
		/// </summary>
		public static Task StartBackground(Action action)
		{
			var task = Task.Run(action);
			return task;
		}

		/// <summary>
		/// 일반 태스크 실행.
		/// </summary>
		public static Task<TResult> StartBackground<TResult>(Func<TResult> action)
		{
			var task = Task.Run(action);
			return task;
		}
	}
}