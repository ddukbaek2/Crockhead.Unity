using Crockhead.Core;
using System;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 기반 컴포넌트.
	/// </summary>
	public abstract class UnityBehaviour : Objectable
	{
		#region INSPECTOR
		#endregion

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected sealed override void Awake()
		{
			base.Awake();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected sealed override void Start()
		{
			base.Start();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected sealed override void OnDestroy()
		{
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
			base.OnCreate();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose()
		{
			base.OnCreate();
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
		/// 애셋을 로드하여 생성. (비동기)
		/// </summary>
		public static async Task<UnityBehaviour> CreateFromAssetAsync(Type componentType, string assetPath, AssetPathType assetPathType = AssetPathType.Resources, Transform parentTransform = null)
		{
			try
			{
				if (componentType == null)
					throw new ArgumentNullException(nameof(componentType));
				if (string.IsNullOrWhiteSpace(assetPath))
					throw new ArgumentException(nameof(assetPath));
				if (!Reflections.IsBaseClass(componentType, typeof(UnityBehaviour)))
					throw new ArgumentException(nameof(componentType));

				var obj = await InstantiationHelper.CreateFromAssetAsync(assetPath, assetPathType, null, parentTransform);
				var component = (UnityBehaviour)obj.GetOrAddComponent(componentType);
				if (parentTransform != null)
					component.transform.SetParent(parentTransform, true);
				TransformHelper.ResetTransform(component.transform);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 특성에서 애셋을 로드하여 생성. (비동기)
		/// </summary>
		public static async Task<UnityBehaviour> CreateFromAttributeAsync(Type componentType, Transform parentTransform = null)
		{
			try
			{
				if (!Reflections.TryGetAttribute<AssetPathAttribute>(componentType, out var assetPathAttribute))
					throw new InvalidOperationException(nameof(componentType));

				var assetPathValue = assetPathAttribute.Value;
				var assetPathType = assetPathAttribute.Type;
				var component = await UnityBehaviour.CreateFromAssetAsync(componentType, assetPathValue, assetPathType, parentTransform);
				return component;
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