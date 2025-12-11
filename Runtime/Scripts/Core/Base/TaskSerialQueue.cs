using System;
using System.Threading;
using System.Threading.Tasks;


namespace Crockhead.Core
{
	/// <summary>
	/// 태스크 직렬 큐.
	/// </summary>
	public class TaskSerialQueue
	{
		/// <summary>
		/// 작업/
		/// </summary>
		private Task m_Task;

		/// <summary>
		/// 설정.
		/// </summary>
		private TaskContinuationOptions m_TaskContinuationOptions;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public TaskSerialQueue()
		{
			m_Task = Task.CompletedTask;
			m_TaskContinuationOptions = TaskContinuationOptions.ExecuteSynchronously;
		}

		/// <summary>
		/// 작업 추가.
		/// </summary>
		private Task Enqueue(Func<Task, Task> onStarted)
		{
			if (onStarted == null)
				throw new ArgumentNullException(nameof(onStarted));

			static Task Completed(Task last)
			{
				return Task.CompletedTask;
			}

			// 작업 추가 및 체인 재할당.
			var task = m_Task.ContinueWith(onStarted, m_TaskContinuationOptions).Unwrap();
			m_Task = task.ContinueWith(Completed, m_TaskContinuationOptions).Unwrap();

			return task;
		}

		/// <summary>
		/// 작업 추가.
		/// </summary>
		public Task Enqueue(Task task)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));

			Task OnStarted(Task last)
			{
				return task;
			}

			// 작업 추가 및 체인 재할당.
			var enqueuedTask = Enqueue(OnStarted);
			return task;
		}

		/// <summary>
		/// 작업 추가.
		/// </summary>
		public Task Enqueue(Func<Task> creator)
		{
			if (creator == null)
				throw new ArgumentNullException(nameof(creator));

			Task OnStarted(Task last)
			{
				return creator();
			}

			// 작업 추가 및 체인 재할당.
			var enqueuedTask = Enqueue(OnStarted);
			return enqueuedTask;
		}

		/// <summary>
		/// 취소 가능한 작업 추가.
		/// </summary>
		public Task Enqueue(Func<CancellationToken, Task> creator, CancellationToken cancellationToken)
		{
			if (creator == null)
				throw new ArgumentNullException(nameof(creator));

			Task OnStarted(Task last)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return Task.FromCanceled(cancellationToken);
				}
				
				return creator(cancellationToken);
			}
			
			// 작업 추가 및 체인 재할당.
			var enqueuedTask = Enqueue(OnStarted);
			return enqueuedTask;
		}
	}
}