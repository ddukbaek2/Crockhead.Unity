using Crockhead.Core;
using System;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// UI 작업 큐.
	/// </summary>
	public class UIDispatchQueue : Disposable
	{
		/// <summary>
		/// 순차 큐.
		/// </summary>
		private SequentialOperationExecutor m_Queue;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIDispatchQueue() : base()
		{
			m_Queue = new SequentialOperationExecutor();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 실행.
		/// <para>직렬큐 이므로 큐에 적재되어 기존 큐의 작업부터 순차 실행되므로 실제 입력한 함수의 실행은 한참 뒤.</para>
		/// </summary>
		public void StartAsync(Action execute)
		{
			if (execute == null)
				return;

			m_Queue.Execute(execute);
		}
	}
}