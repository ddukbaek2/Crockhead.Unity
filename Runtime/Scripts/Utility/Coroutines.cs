using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 코루틴 유틸리티.
	/// </summary>
	public static class Coroutines
	{
		/// <summary>
		/// 코루틴 시작.
		/// </summary>
		public static Coroutine StartCoroutine(IEnumerator routine)
		{
			return UnityRunner.Instance.StartCoroutine(routine);
		}

		/// <summary>
		/// 코루틴 정지.
		/// </summary>
		public static void StopCoroutine(IEnumerator routine)
		{
			UnityRunner.Instance.StopCoroutine(routine);
		}

		/// <summary>
		/// 코루틴 정지.
		/// </summary>
		public static void StopCoroutine(Coroutine routine)
		{
			UnityRunner.Instance.StopCoroutine(routine);
		}

		/// <summary>
		/// 코루틴 모두 정지.
		/// </summary>
		public static void StopAllCoroutines()
		{
			UnityRunner.Instance.StopAllCoroutines();
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

			return Coroutines.StartCoroutine(Process(delay, action));
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

			return Coroutines.StartCoroutine(Process(action));
		}

		/// <summary>
		/// 태스크 대기.
		/// </summary>
		public static Coroutine WaitForTaskCompletion(Task task)
		{
			static IEnumerator Routine(Task task)
			{
				while (!task.IsCompleted)
					yield return null;
			}

			return Coroutines.StartCoroutine(Routine(task));
		}
	}
}