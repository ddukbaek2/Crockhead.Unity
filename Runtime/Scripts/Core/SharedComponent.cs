using Crockhead.Core;
using System;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 공유 컴포넌트.
	/// </summary>
	public abstract class SharedComponent<TComponent> : MonoBehaviour where TComponent : SharedComponent<TComponent>
	{
		/// <summary>
		/// 생성 되었는지 여부 프로퍼티.
		/// </summary>
		public static bool IsCreated => SharedInstances.IsSet<TComponent>();

		/// <summary>
		/// 애플리케이션 종료 중인지 여부 프로퍼티.
		/// </summary>
		public static bool IsApplicationQuitting { private set; get; } = false;

		/// <summary>
		/// 공유 컴포넌트 프로퍼티.
		/// </summary>
		public static TComponent Instance => Create(); // Instance

		/// <summary>
		/// 컴포넌트 타입의 이름 프로퍼티.
		/// </summary>
		public static string ComponentName => typeof(TComponent).Name;

		/// <summary>
		/// 생성됨.
		/// </summary>
		private void Awake()
		{
			if (SharedInstances.IsSet<TComponent>())
			{
				GameObject.Destroy(gameObject);
				return;
			}

			GameObject.DontDestroyOnLoad(gameObject);
			SharedInstances.Set<TComponent>((TComponent)this);
			OnCreate();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		private void Start()
		{
			OnInitialize();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		private void OnDestroy()
		{
			if (!SharedInstances.IsSet<TComponent>())
				return;

			var sharedInstance = SharedInstances.Get<TComponent>();
			if (sharedInstance != this)
				return;

			SharedInstances.Unset<TComponent>();
			OnDispose();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void OnCreate()
		{
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected virtual void OnInitialize()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected virtual void OnDispose()
		{
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
//			Debug.Log($"[{ComponentName}] OnApplicationPause(pause: {pause})");
//#endif
		}

		/// <summary>
		/// 애플리케이션 포커스획득/상실됨. (입력 포커스 상실)
		/// <para>데스크탑용</para>
		/// </summary>
		protected virtual void OnApplicationFocus(bool focus)
		{
//#if UNITY_EDITOR
//			Debug.Log($"[{ComponentName}] OnApplicationFocus(focus: {focus})");
//#endif
		}

		/// <summary>
		/// 애플리케이션 종료됨.
		/// </summary>
		protected virtual void OnApplicationQuit()
		{
//#if UNITY_EDITOR
//			Debug.Log($"[{ComponentName}] OnApplicationQuit()");
//#endif

			IsApplicationQuitting = true;
		}

		/// <summary>
		/// 애플리케이션 메모리 정리 요청됨.
		/// </summary>
		protected virtual void OnLowMemory()
		{
//#if UNITY_EDITOR
//			Debug.Log($"[{ComponentName}] OnLowMemory()");
//#endif
		}

		/// <summary>
		/// 현재 객체가 파괴 되었는지 여부.
		/// </summary>
		public bool IsDestroyed()
		{
			if (this == null)
				return true;
			return false;
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TComponent Create()
		{
			if (SharedInstances.TryGet<TComponent>(out var sharedInstance))
				return sharedInstance;

			sharedInstance = GameObject.FindAnyObjectByType<TComponent>();
			if (sharedInstance != null)
			{
				SharedInstances.Set<TComponent>(sharedInstance);
				return sharedInstance;
			}

			// 있는걸 사용하는 것은 상관없지만 종료중일때 객체의 신규 생성은 금지.
			if (IsApplicationQuitting)
			{
				var type = typeof(TComponent);
				throw new InvalidOperationException($"[SharedComponent] Cannot create object: the application is quitting. ({type.Name})");
			}

			var obj = InstantiationHelper.CreateFromAttribute<TComponent>();
			sharedInstance = obj.GetOrAddComponent<TComponent>();
			SharedInstances.Set<TComponent>(sharedInstance);
			return sharedInstance;
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public static void Dispose()
		{
			if (!SharedInstances.TryGet<TComponent>(out var sharedInstance))
				return;

			GameObject.Destroy(sharedInstance.gameObject);
		}
	}
}