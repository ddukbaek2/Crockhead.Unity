using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 코루틴 확장 유틸리티.
	/// </summary>
	public static class TaskExtensions
	{
		/// <summary>
		/// 델리게이트.
		/// </summary>
		public delegate IEnumerator OnWaitForTaskProcess<TResult>(TaskCompletionSource<TResult> taskCompletionSource);

		/// <summary>
		/// 코루틴으로 메인 쓰레드에서 태스크 진행 및 완료 처리.
		/// </summary>
		private static IEnumerator WaitForTaskProcess(TaskCompletionSource<bool> taskCompletionSource, IEnumerator routine)
		{
			yield return routine;
			taskCompletionSource.SetResult(true);
		}

		/// <summary>
		/// 코루틴으로 메인 쓰레드에서 태스크 진행 대기 및 완료 처리.
		/// </summary>
		private static IEnumerator WaitForTaskProcess(TaskCompletionSource<bool> taskCompletionSource, Task task)
		{
			while (!task.IsCompleted)
				yield return null;
			if (task.IsFaulted)
				taskCompletionSource.SetException(task.Exception);
			else if (task.IsCanceled)
				taskCompletionSource.SetCanceled();
			else
				taskCompletionSource.SetResult(true);
		}

		/// <summary>
		/// 코루틴으로 메인 쓰레드에서 태스크 진행 대기 및 완료 처리.
		/// </summary>
		private static IEnumerator WaitForTaskProcess<TResult>(TaskCompletionSource<TResult> taskCompletionSource, Task<TResult> task)
		{
			while (!task.IsCompleted)
				yield return null;
			if (task.IsFaulted)
				taskCompletionSource.SetException(task.Exception);
			else if (task.IsCanceled)
				taskCompletionSource.SetCanceled();
			else
				taskCompletionSource.SetResult(task.Result);
		}

		/// <summary>
		/// 컴포넌트에서 태스크 실행.
		/// <para>실제 내부 처리는 해당 컴포넌트에서 코루틴을 실행 후 코루틴 대기하면서 태스크 변환.</para>
		/// <para>코루틴을 Task 변환.</para>
		/// </summary>
		public static Task StartTask(this MonoBehaviour target, IEnumerator routine)
		{
			var taskCompletionSource = new TaskCompletionSource<bool>();
			target.StartCoroutine(WaitForTaskProcess(taskCompletionSource, routine));
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 컴포넌트에서 태스크 실행.
		/// <para>실제 내부 처리는 해당 컴포넌트에서 코루틴을 실행 후 코루틴 대기하면서 태스크 변환.</para>
		/// <para>코루틴을 Task 변환.</para>
		/// <para>routineWithTaskCompletionSource 처리부에서 taskCompletionSource.SetResult(value) 처리 필요. 안하면 안끝남.</para>
		/// </summary>
		public static Task<TResult> StartTask<TResult>(this MonoBehaviour target, OnWaitForTaskProcess<TResult> routineWithTaskCompletionSource)
		{
			var taskCompletionSource = new TaskCompletionSource<TResult>();
			var routine = routineWithTaskCompletionSource.Invoke(taskCompletionSource);
			target.StartCoroutine(routine);
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 컴포넌트에서 태스크 실행.
		/// <para>실제 내부 처리는 해당 컴포넌트에서 코루틴을 실행 후 코루틴 대기하면서 태스크 변환.</para>
		/// <para>실제 외부 Task의 처리를 기다릴 수 있는 버전.</para>
		/// </summary>
		public static Task StartTask(this MonoBehaviour target, Task task)
		{
			var taskCompletionSource = new TaskCompletionSource<bool>();
			target.StartCoroutine(WaitForTaskProcess(taskCompletionSource, task));
			return taskCompletionSource.Task;
		}

		/// <summary>
		/// 컴포넌트에서 태스크 실행.
		/// <para>실제 내부 처리는 해당 컴포넌트에서 코루틴을 실행 후 코루틴 대기하면서 태스크 변환.</para>
		/// <para>실제 외부 Task의 처리를 기다리고 결과값을 실을 수 있는 제너릭 버전.</para>
		/// </summary>
		public static Task<TResult> StartTask<TResult>(this MonoBehaviour target, Task<TResult> task)
		{
			var taskCompletionSource = new TaskCompletionSource<TResult>();
			target.StartCoroutine(WaitForTaskProcess<TResult>(taskCompletionSource, task));
			return taskCompletionSource.Task;
		}
	}
}