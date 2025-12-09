using Crockhead.Core;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 런타임 처리기.
	/// </summary>
	[ExecuteAlways]
	public class UnityRuntime : SharedComponent<UnityRuntime>
	{
		/// <summary>
		/// 스케쥴러.
		/// </summary>
		private Scheduler m_Scheduler;

		/// <summary>
		/// 스케쥴러 프로퍼티.
		/// </summary>
		public Scheduler Scheduler => m_Scheduler;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate()
		{
			base.OnCreate();

			//gameObject.hideFlags = HideFlags.HideAndDontSave;
			UnityThreadDispatcher.Create();
			DispatchQueue.Foreground = new DispatchQueue();
			m_Scheduler = new Scheduler();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose()
		{
			UnityThreadDispatcher.Destroy();
			Disposables.Dispose(m_Scheduler);

			base.OnDispose();
		}
	}
}