using Crockhead.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 실행 큐.
	/// </summary>
	public class DispatchQueue : Disposable
	{
		/// <summary>
		/// 실행 단위.
		/// </summary>
		public readonly struct DispatchQueueItem
		{
			public Func<Task> TaskFactory { get; }
			public TaskCompletionSource<bool> TaskCompletionSource { get; }
			public int StartFrame { get; }

			/// <summary>
			/// 생성됨.
			/// </summary>
			public DispatchQueueItem(Func<Task> task, int delayFrame = 0)
			{
				TaskFactory = task;
				TaskCompletionSource = new TaskCompletionSource<bool>();

				if (delayFrame < 1)
				{
					StartFrame = -1;
				}
				else
				{
					StartFrame = Time.frameCount + delayFrame;
				}
			}
		}

		/// <summary>
		/// 메인 쓰레드 프로퍼티. (첫 호출을 Unity Runtime 컴포넌트 내부에서 진행)
		/// </summary>
		public static DispatchQueue Foreground { internal set; get; } = null;

		/// <summary>
		/// 큐.
		/// </summary>
		private Queue<DispatchQueueItem> m_Queue;

		/// <summary>
		/// 처리 중 여부.
		/// </summary>
		private bool m_IsProcessing;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public DispatchQueue() : base()
		{
			m_Queue = new Queue<DispatchQueueItem>();
			m_IsProcessing = false;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			//base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 처리 큐.
		/// </summary>
		private IEnumerator ProcessQueue()
		{
			m_IsProcessing = true;

			while (m_Queue.Count > 0)
			{
				var item = m_Queue.Dequeue();
				var failure = default(Exception);
				var task = default(Task);

				// 프레임 대기 사용.
				if (item.StartFrame > 0)
				{
					// 현재 프레임 < 실행 프레임.
					while (Time.frameCount < item.StartFrame)
						yield return null;
				}

				try
				{
					// 작업 생성.
					task = item.TaskFactory();
				}
				catch (Exception exception)
				{
					failure = exception;
				}

				// 오류가 없고 작업이 존재할 경우.
				if (failure == null && task != null)
				{
					while (!task.IsCompleted)
						yield return null;

					// 실패.
					if (task.IsFaulted)
					{
						failure = task.Exception;
					}
					// 취소.
					else if (task.IsCanceled)
					{
						item.TaskCompletionSource.SetCanceled();
						continue;
					}
				}

				// 오류 발생.
				if (failure != null)
				{
					item.TaskCompletionSource.SetException(failure);
				}
				// 완료.
				else
				{
					item.TaskCompletionSource.SetResult(true);
				}
			}

			m_IsProcessing = false;
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public Task RunAsync(Func<Task> task, int delayFrame)
		{
			var work = new DispatchQueueItem(task, delayFrame);
			m_Queue.Enqueue(work);

			if (!m_IsProcessing)
			{
				CoroutineHelper.StartCoroutine(ProcessQueue());
			}
			return work.TaskCompletionSource.Task;
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public Task RunAsync(Func<Task> task)
		{
			return RunAsync(task, 0);
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public Task RunAsync(Action action, int delayFrame)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			return RunAsync(() =>
			{
				action.Invoke();
				return Task.CompletedTask;
			}, delayFrame);
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public Task RunAsync(Action action)
		{
			return RunAsync(action, 0);
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public Task RunNextFrameAsync(Action action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));

			return RunAsync(() =>
			{
				action.Invoke();
				return Task.CompletedTask;
			}, 1);
		}
	}
}