using Crockhead.Core;
using UnityEngine;
using IDotNetDisposable = System.IDisposable;


namespace Crockhead.Unity.Deprecated
{
	/// <summary>
	/// 공유 컴포넌트.
	/// <para>IDotNetDisposable 인터페이스 구현체.</para>
	/// <para>IDisposable 인터페이스 구현체.</para>
	/// </summary>
	public abstract class SharedComponent<TComponent> : MonoBehaviour, IDisposable where TComponent : SharedComponent<TComponent>
	{
		/// <summary>
		/// 생성 되었는지 여부 프로퍼티.
		/// </summary>
		public static bool IsCreated => SharedInstances.IsSet<TComponent>();

		/// <summary>
		/// 공유 컴포넌트 프로퍼티.
		/// </summary>
		public static TComponent Instance => Create();

		/// <summary>
		/// 해제 되었는지 여부 프로퍼티.
		/// <para>IDisposable 인터페이스 구현.</para>
		/// </summary>
		bool IDisposable.IsDisposed => this != null && !SharedInstances.IsSet<TComponent>();

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
			OnDispose(false);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected abstract void OnCreate();

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected abstract void OnInitialize();

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected abstract void OnDispose(bool explicitDisposing);

		/// <summary>
		/// 해제.
		/// <para>IDotNetDisposable 인터페이스 구현.</para>
		/// </summary>
		void IDotNetDisposable.Dispose()
		{
			if (!SharedInstances.TryGet<TComponent>(out var sharedInstance))
				return;

			if (sharedInstance != this)
				return;

			SharedInstances.Unset<TComponent>();
			OnDispose(true);
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

			sharedInstance = GameObjectHelper.CreateGameObjectWithComponent<TComponent>();
			SharedInstances.Set<TComponent>(sharedInstance);
			return sharedInstance;
		}
	}
}