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
		/// 컴포넌트 타입의 이름 프로퍼티.
		/// </summary>
		public static string SharedComponentTypeName => typeof(UnityRuntime).Name;

		/// <summary>
		/// 애플리케이션 종료 중인지 여부 프로퍼티.
		/// </summary>
		public static bool IsApplicationQuitting { private set; get; } = false;

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

			GameObject.DontDestroyOnLoad(gameObject);
			//gameObject.hideFlags = HideFlags.HideAndDontSave;
			UnityThreadDispatcher.Create();
			DispatchQueue.Foreground = new DispatchQueue();
			m_Scheduler = new Scheduler();
		}

		/// <summary>
		/// 애플리케이션 일시정지/재개됨. (프로세스 중단 및 백그라운드 전환)
		/// <para>모바일용</para>
		/// <para>안드로이드: 홈화면이동, 앱전환, 잠금화면이동, 전화수신, 화면꺼짐, 액티비티일시정지</para>
		/// <para>아이폰: 홈화면이동, 앱전환, 잠금화면이동, 전화/페이스타임수신, 컨트롤센터, 알림센터</para>
		/// <para>윈도우: 전체화면앱이 포커스상실</para>
		/// <para>맥: 전체화면앱이 다른전체화면앱으로이동</para>
		/// <para>웹: 브라우저탭비활성화</para>
		/// </summary>
		protected virtual void OnApplicationPause(bool pause)
		{
			//#if UNITY_EDITOR
			//			Debug.Log($"[{ComponentTypeName}] OnApplicationPause(pause: {pause})");
			//#endif
		}

		/// <summary>
		/// 애플리케이션 포커스획득/상실됨. (입력 포커스 상실)
		/// <para>데스크탑용</para>
		/// </summary>
		protected virtual void OnApplicationFocus(bool focus)
		{
			//#if UNITY_EDITOR
			//			Debug.Log($"[{ComponentTypeName}] OnApplicationFocus(focus: {focus})");
			//#endif
		}

		/// <summary>
		/// 애플리케이션 종료됨.
		/// </summary>
		protected virtual void OnApplicationQuit()
		{
			//#if UNITY_EDITOR
			//			Debug.Log($"[{ComponentTypeName}] OnApplicationQuit()");
			//#endif
			IsApplicationQuitting = true;
		}

		/// <summary>
		/// 애플리케이션 메모리 정리 요청됨.
		/// </summary>
		protected virtual void OnLowMemory()
		{
			//#if UNITY_EDITOR
			//			Debug.Log($"[{ComponentTypeName}] OnLowMemory()");
			//#endif
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose()
		{
			UnityThreadDispatcher.Dispose();
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