using Crockhead.Core;
using System;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 기반 컴포넌트.
	/// </summary>
	public abstract class UnityBehaviour : MonoBehaviour
	{
		#region INSPECTOR
		#endregion

		/// <summary>
		/// 컴포넌트 타입의 이름 프로퍼티.
		/// </summary>
		public string ComponentName { private set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		private void Awake()
		{
			var type = GetType();
			ComponentName = type.Name;
			Debug.Log($"[{ComponentName}] Awake()");

			OnCreate();
		}

		/// <summary>
		/// 시작됨.
		/// </summary>
		private void Start()
		{
			Debug.Log($"[{ComponentName}] OnPrepare()");

			OnInitialize();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		private void OnDestroy()
		{
			Debug.Log($"[{ComponentName}] OnDestroy()");

			OnDispose();
		}

		/// <summary>
		/// 활성화됨.
		/// </summary>
		protected virtual void OnEnable()
		{
			Debug.Log($"[{ComponentName}] OnEnable()");
		}

		/// <summary>
		/// 비활성화됨.
		/// </summary>
		protected virtual void OnDisable()
		{
			Debug.Log($"[{ComponentName}] OnDisable()");
		}

		/// <summary>
		/// 재시작.
		/// </summary>
		protected virtual void Reset()
		{
			Debug.Log($"[{ComponentName}] Reset()");
		}

		//protected virtual void OnRectTransformDimensionsChange()
		//{
		//	Debug.Log($"[{componentType.Name}] OnRectTransformDimensionsChange()");
		//}

		protected virtual void OnBeforeTransformParentChanged()
		{
			Debug.Log($"[{ComponentName}] OnBeforeTransformParentChanged()");
		}

		protected virtual void OnTransformParentChanged()
		{
			Debug.Log($"[{ComponentName}] OnTransformParentChanged()");
		}

		//protected virtual void OnCanvasGroupChanged()
		//{
		//	Debug.Log($"[{componentType.Name}] OnCanvasGroupChanged()");
		//}

		//protected virtual void OnCanvasHierarchyChanged()
		//{
		//	Debug.Log($"[{componentType.Name}] OnCanvasHierarchyChanged()");
		//}

		protected virtual void OnDidApplyAnimationProperties()
		{
			Debug.Log($"[{ComponentName}] OnDidApplyAnimationProperties()");
		}

		/// <summary>
		/// 유효성 체크.
		/// </summary>
		protected virtual void OnValidate()
		{
			Debug.Log($"[{ComponentName}] OnValidate()");
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
		/// 객체 파괴 여부.
		/// </summary>
		public virtual bool IsDestroyed()
		{
			return this == null;
		}

		/// <summary>
		/// 활성화 여부.
		/// </summary>
		public virtual bool IsActive()
		{
			return isActiveAndEnabled;
		}

		/// <summary>
		/// 자식 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public TComponent GetOrAddComponent<TComponent>() where TComponent : Component
		{
			var component = GetOrAddComponent<TComponent>(string.Empty);
			return component;
		}

		/// <summary>
		/// 자식 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public TComponent GetOrAddComponent<TComponent>(string transformPath) where TComponent : Component
		{
			var component = TransformHelper.GetOrAddComponent<TComponent>(transform, transformPath);
			return component;
		}

		/// <summary>
		/// 생성.
		/// </ummary>
		public static UnityBehaviour Create(Type componentType, Transform parentTransform = null)
		{
			try
			{
				if (componentType == null)
					throw new ArgumentNullException(nameof(componentType));
				if (!Reflections.IsBaseClass(componentType, typeof(UnityBehaviour)))
					throw new ArgumentException(nameof(componentType));

				var obj = InstantiationHelper.Create(componentType.Name, null, parentTransform);
				var component = (UnityBehaviour)obj.GetOrAddComponent(componentType);
				if (parentTransform != null)
					component.transform.SetParent(parentTransform, true);
				TransformHelper.ResetTransform(obj.transform);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 애셋을 로드하여 생성.
		/// </summary>
		public static UnityBehaviour CreateFromAsset(Type componentType, string assetPath, AssetPathType assetPathType = AssetPathType.Resources, Transform parentTransform = null)
		{
			try
			{
				if (componentType == null)
					throw new ArgumentNullException(nameof(componentType));
				if (string.IsNullOrWhiteSpace(assetPath))
					throw new ArgumentException(nameof(assetPath));
				if (!Reflections.IsBaseClass(componentType, typeof(UnityBehaviour)))
					throw new ArgumentException(nameof(componentType));

				var obj = InstantiationHelper.CreateFromAsset(assetPath, assetPathType, null, parentTransform);
				var component = (UnityBehaviour)obj.GetOrAddComponent(componentType);
				if (parentTransform != null)
					component.transform.SetParent(parentTransform, true);
				TransformHelper.ResetTransform(obj.transform);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 특성에서 애셋을 로드하여 생성.
		/// </summary>
		public static UnityBehaviour CreateFromAttribute(Type componentType, Transform parentTransform = null)
		{
			try
			{
				if (!Reflections.TryGetAttribute<AssetPathAttribute>(componentType, out var assetPathAttribute))
					throw new InvalidOperationException(nameof(componentType));

				var assetPathValue = assetPathAttribute.Value;
				var assetPathType = assetPathAttribute.Type;
				return UnityBehaviour.CreateFromAsset(componentType, assetPathValue, assetPathType, parentTransform);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 생성.
		/// </summary>
		public static TComponent Create<TComponent>(Transform parentTransform = null) where TComponent : UnityBehaviour
		{
			try
			{
				var componentType = typeof(TComponent);
				var component = (TComponent)UnityBehaviour.Create(componentType, parentTransform);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 애셋을 로드하여 생성.
		/// </summary>
		public static TComponent CreateFromAsset<TComponent>(Transform parentTransform = null) where TComponent : UnityBehaviour
		{
			try
			{
				var componentType = typeof(TComponent);
				var component = (TComponent)UnityBehaviour.Create(componentType, parentTransform);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 특성에서 애셋을 로드하여 생성.
		/// </summary>
		public static TComponent CreateFromAttribute<TComponent>(Transform parentTransform = null) where TComponent : UnityBehaviour
		{
			try
			{
				var componentType = typeof(TComponent);
				var component = (TComponent)UnityBehaviour.CreateFromAttribute(componentType, parentTransform);
				return component;
			}
			catch
			{
				throw;
			}
		}
	}
}