using Crockhead.Core;
using System;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 공유 컴포넌트.
	/// </summary>
	public abstract class SharedComponent<TComponent> : Objectable where TComponent : SharedComponent<TComponent>
	{
		/// <summary>
		/// 생성 되었는지 여부 프로퍼티.
		/// </summary>
		public static bool IsCreated => SharedInstances.IsSet<TComponent>();

		/// <summary>
		/// 공유 컴포넌트 프로퍼티.
		/// </summary>
		public static TComponent Instance => Create(); // Instance

		/// <summary>
		/// 컴포넌트 타입의 이름 프로퍼티.
		/// </summary>
		public static string SharedComponentTypeName => typeof(TComponent).Name;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			if (SharedInstances.IsSet<TComponent>())
			{
				GameObject.Destroy(gameObject);
				return;
			}

			GameObject.DontDestroyOnLoad(gameObject);
			SharedInstances.Set<TComponent>((TComponent)this);
			base.Awake();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDestroy()
		{
			if (!SharedInstances.TryGet<TComponent>(out var sharedInstance))
				return;

			if (sharedInstance != this)
				return;

			SharedInstances.Unset<TComponent>();
			base.OnDestroy();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate()
		{
			base.OnCreate();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected override void OnInitialize()
		{
			base.OnInitialize();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose()
		{
			base.OnDispose();
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
			var componentType = typeof(TComponent);
			if (UnityRuntime.IsApplicationQuitting)
			{
				throw new InvalidOperationException($"[SharedComponent] Cannot create object: the application is quitting. ({componentType.Name})");
			}

			var obj = InstantiationHelper.CreateFromAttribute(componentType);
			if (obj == null)
				obj = InstantiationHelper.Create(componentType.Name);

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