using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 코루틴 유틸리티.
	/// </summary>
	public static class CoroutineHelper
	{
		/// <summary>
		/// 코루틴 시작.
		/// </summary>
		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			return UnityRuntime.Instance.StartCoroutine(routine);
		}

		/// <summary>
		/// 코루틴 정지.
		/// </summary>
		public static void StopCoroutine(IEnumerator routine)
		{
			UnityRuntime.Instance.StopCoroutine(routine);
		}

		/// <summary>
		/// 코루틴 정지.
		/// </summary>
		public static void StopCoroutine(Coroutine routine)
		{
			UnityRuntime.Instance.StopCoroutine(routine);
		}

		/// <summary>
		/// 코루틴 모두 정지.
		/// </summary>
		public static void StopAllCoroutines()
		{
			UnityRuntime.Instance.StopAllCoroutines();
		}

		/// <summary>
		/// 일정 시간 대기.
		/// </summary>
		public static Coroutine WaitForSeconds(float delay, Action action)
		{
			static IEnumerator Process(float delay, Action action)
			{
				yield return new WaitForSeconds(delay);
				action?.Invoke();
			}

			return CoroutineHelper.StartCoroutine(Process(delay, action));
		}

		/// <summary>
		/// 다음 프레임까지 대기.
		/// </summary>
		public static Coroutine WaitForNextFrame(Action action)
		{
			static IEnumerator Process(Action action)
			{
				yield return null;
				action?.Invoke();
			}

			return CoroutineHelper.StartCoroutine(Process(action));
		}

		/// <summary>
		/// 태스크 완료 대기.
		/// </summary>
		public static Coroutine WaitForTaskCompletion(Task task)
		{
			static IEnumerator Process(Task task)
			{
				while (!task.IsCompleted)
					yield return null;
			}

			if (task == null)
				return null;

			return CoroutineHelper.StartCoroutine(Process(task));
		}

		/// <summary>
		/// 태스크 완료 대기.
		/// <para>async Task Func(): WaitForTaskCompletion(Func) ==> 비동기 실행 및 완료 대기. (정상)</para>
		/// <para>async void Func(): WaitForTaskCompletion(Func) ==> 동일한 함수이나 DOTNET 언어적 제약으로 반환 Task 생성이 없어서 await 체인에 접근 불가. (오류)</para>
		/// <para>async void 는 이벤트 핸들러를 위해 언어적으로 사용자 대기 할 수 없는 비동기 기능으로 일반 함수로 호출하면 닷넷 런타임 내부에서만 비동기 호출됨.</para>
		/// </summary>
		public static Coroutine WaitForTaskCompletion(Func<Task> taskFactory)
		{
			var task = taskFactory?.Invoke();
			return CoroutineHelper.WaitForTaskCompletion(task);
		}
	}
}