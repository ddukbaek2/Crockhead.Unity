using Crockhead.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 게임 오브젝트 인스턴스 생성 유틸리티.
	/// </summary>
	public static class InstantiationHelper
	{
		/// <summary>
		/// 게임 오브젝트 생성.
		/// </summary>
		public static GameObject Create(string name, Type[] requiredComponentTypes = null, Transform parentTransform = null)
		{
			try
			{
				var obj = new GameObject(name);

				if (requiredComponentTypes != null)
				{
					for (var i = 0; i < requiredComponentTypes.Length; ++i)
					{
						var componentType = requiredComponentTypes[i];
						if (componentType == null)
							continue;

						obj.GetOrAddComponent(componentType);
					}
				}

				if (parentTransform != null)
				{
					obj.transform.SetParent(parentTransform, false);
				}

				TransformHelper.ResetTransform(obj.transform);

				return obj;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return null;
			}
		}

		/// <summary>
		/// 애셋으로부터 게임 오브젝트 생성.
		/// </summary>
		public static GameObject CreateFromAsset(string assetPath, AssetPathType assetPathType, Type[] requiredComponentTypes = null, Transform parentTransform = null)
		{
			try
			{
				using var assetLoader = new AssetLoader<GameObject>(assetPath, assetPathType);
				assetLoader.Load();
				var asset = assetLoader.Asset;
				if (asset == null)
					throw new NullReferenceException(assetPath);

				var assetName = Path.GetFileNameWithoutExtension(assetPath);
				var obj = GameObject.Instantiate<GameObject>(asset);
				obj.name = assetName;

				if (requiredComponentTypes != null)
				{
					for (var i = 0; i < requiredComponentTypes.Length; ++i)
					{
						var componentType = requiredComponentTypes[i];
						if (componentType == null)
							continue;

						obj.GetOrAddComponent(componentType);
					}
				}

				if (parentTransform != null)
				{
					obj.transform.SetParent(parentTransform, false);
				}

				TransformHelper.ResetTransform(obj.transform);

				return obj;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return null;
			}
		}

		/// <summary>
		/// 애셋으로부터 게임 오브젝트 생성. (비동기)
		/// </summary>
		public static async Task<GameObject> CreateFromAssetAsync(string assetPath, AssetPathType assetPathType, Type[] requiredComponentTypes = null, Transform parentTransform = null)
		{
			try
			{
				// 리소스 로드.
				using var assetLoader = new AssetLoader<GameObject>(assetPath, assetPathType);
				var asyncOperation = assetLoader.LoadAsync();
				await TaskHelper.WaitForCompletion(asyncOperation);
				var asset = assetLoader.Asset;
				if (asset == null)
					throw new NullReferenceException(assetPath);

				// 인스턴스 생성.
				var assetName = Path.GetFileNameWithoutExtension(assetPath);
				var obj = default(GameObject);
				switch (assetPathType)
				{
					case AssetPathType.Resources:
						{
							var asyncInstantiateOperation = GameObject.InstantiateAsync<GameObject>(asset);
							await TaskHelper.WaitForCompletion(asyncInstantiateOperation);
							if (asyncInstantiateOperation.Result.Length == 0)
								throw new InvalidOperationException(nameof(asyncInstantiateOperation.Result));
							obj = asyncInstantiateOperation.Result[0];
							obj.name = assetName;
							break;
						}

					case AssetPathType.Addressables:
						{
							//Addressables.InstantiateAsync
							break;
						}
				}

				if (obj == null)
					throw new InvalidOperationException(nameof(obj));

				if (requiredComponentTypes != null)
				{
					for (var i = 0; i < requiredComponentTypes.Length; ++i)
					{
						var componentType = requiredComponentTypes[i];
						if (componentType == null)
							continue;

						obj.GetOrAddComponent(componentType);
					}
				}

				if (parentTransform != null)
				{
					obj.transform.SetParent(parentTransform, false);
				}

				TransformHelper.ResetTransform(obj.transform);
				return obj;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return null;
			}
		}

		/// <summary>
		/// 대상 특성으로부터 게임 오브젝트 생성.
		/// </summary>
		public static GameObject CreateFromAttribute(Type componentType, Type[] requiredComponentTypes = null, Transform parentTransform = null)
		{
			try
			{
				if (componentType == null)
					throw new ArgumentNullException(nameof(componentType));
				if (!Reflections.TryGetAttribute<AssetPathAttribute>(componentType, out var assetPathAttribute))
					throw new InvalidCastException(nameof(componentType));

				var obj = InstantiationHelper.CreateFromAsset(assetPathAttribute.Value, assetPathAttribute.Type, requiredComponentTypes, parentTransform);
				return obj;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 대상 특성으로부터 게임 오브젝트 생성. (비동기)
		/// </summary>
		public static async Task<GameObject> CreateFromAttributeAsync(Type componentType, Type[] requiredComponentTypes = null, Transform parentTransform = null)
		{
			try
			{
				if (componentType == null)
					throw new ArgumentNullException(nameof(componentType));
				if (!Reflections.TryGetAttribute<AssetPathAttribute>(componentType, out var assetPathAttribute))
					throw new InvalidCastException(nameof(componentType));

				var obj = await InstantiationHelper.CreateFromAssetAsync(assetPathAttribute.Value, assetPathAttribute.Type, requiredComponentTypes, parentTransform);
				return obj;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 대상 특성으로부터 게임 오브젝트 생성.
		/// </summary>
		public static GameObject CreateFromAttribute<TComponent>(Type[] requiredComponentTypes = null, Transform parentTransform = null) where TComponent : Component
		{
			try
			{
				var componentType = typeof(TComponent);
				var obj = CreateFromAttribute(componentType, requiredComponentTypes, parentTransform);
				return obj;
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// 대상 특성으로부터 게임 오브젝트 생성. (비동기)
		/// </summary>
		public static async Task<GameObject> CreateFromAttributeAsync<TComponent>(Type[] requiredComponentTypes = null, Transform parentTransform = null) where TComponent : Component
		{
			try
			{
				var componentType = typeof(TComponent);
				var obj = await CreateFromAttributeAsync(componentType, requiredComponentTypes, parentTransform);
				return obj;
			}
			catch
			{
				return null;
			}
		}


		/// <summary>
		/// 기존의 컴포넌트를 가져오거나 없으면 새로 추가하여 반환.
		/// </summary>
		public static Component GetOrAddComponent(this GameObject obj, Type componentType)
		{
			if (obj == null)
				return null;

			if (componentType == null)
				throw new ArgumentNullException(nameof(componentType));
			if (!Reflections.IsBaseClass(componentType, typeof(Component)))
				throw new InvalidCastException(componentType.Name);

			var component = obj.GetComponent(componentType);
			if (component != null)
				return component;

			component = obj.AddComponent(componentType);
			return component;
		}

		/// <summary>
		/// 기존의 컴포넌트를 가져오거나 없으면 새로 추가하여 반환.
		/// </summary>
		public static TComponent GetOrAddComponent<TComponent>(this GameObject obj) where TComponent : Component
		{
			var componentType = typeof(TComponent);
			var component = InstantiationHelper.GetOrAddComponent(obj, componentType);
			return component as TComponent;
		}
	}
}