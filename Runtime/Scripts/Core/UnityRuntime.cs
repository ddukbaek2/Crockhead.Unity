using Crockhead.Core;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 런타임 처리기.
	/// </summary>
	[ExecuteAlways]
	public class UnityRuntime : Objectable
	{
		/// <summary>
		/// 공유 인스턴스.
		/// </summary>
		private static UnityRuntime s_Instance;

		/// <summary>
		/// 공유 인스턴스 프로퍼티.
		/// </summary>
		public static UnityRuntime Instance => UnityRuntime.Create();

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

		/// <summary>
		/// 생성.
		/// </summary>
		public static UnityRuntime Create()
		{
			if (s_Instance == null)
			{
				s_Instance = GameObject.FindAnyObjectByType<UnityRuntime>();
			}

			if (s_Instance == null)
			{
				var obj = new GameObject("UnityRuntime");
				s_Instance = obj.AddComponent<UnityRuntime>();
			}

			return s_Instance;
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public static void Dispose()
		{
			if (s_Instance == null)
				return;

			GameObject.Destroy(s_Instance.gameObject);
		}
	}
}