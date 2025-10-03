using Crockhead.Core;
using System;
using System.IO;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 게임오브젝트 유틸리티.
	/// </summary>
	public static class GameObjects
	{
		/// <summary>
		/// 게임오브젝트 생성.
		/// </summary>
		public static GameObject CreateGameObject(string assetPath)
		{
			try
			{
				using var assetReader = new AssetReader<GameObject>(assetPath);
				assetReader.Read();
				var asset = assetReader.Result;
				if (asset == null)
					throw new NullReferenceException(assetPath);

				var obj = GameObject.Instantiate<GameObject>(asset);
				var name = Path.GetFileNameWithoutExtension(assetPath);

				obj.name = name;
				return obj;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				return null;
			}
		}

		/// <summary>
		/// 게임오브젝트 + 컴포넌트 생성.
		/// </summary>
		public static Component CreateGameObjectWithComponent(Type componentType, AssetPathType assetPathType, string assetPath)
		{
			if (!Reflections.IsBaseClass(componentType, typeof(Component)))
				throw new InvalidCastException(componentType.Name);

			var gameObject = default(GameObject);
			var componentName = componentType.Name;

			switch (assetPathType)
			{
				case AssetPathType.Resources:
					{
						gameObject = GameObjects.CreateGameObject(assetPath);
						break;
					}

				case AssetPathType.Addressables:
					{
						gameObject = new GameObject(componentName);
						break;
					}

				default:
					{
						gameObject = new GameObject(componentName);
						break;
					}
			}

			var component = gameObject.GetOrAddComponent(componentType);
			return component;
		}


		/// <summary>
		/// 게임오브젝트 + 컴포넌트 생성.
		/// </summary>
		public static TComponent CreateGameObjectWithComponent<TComponent>(AssetPathType assetPathType, string assetPath) where TComponent : Component
		{
			var componentType = typeof(TComponent);
			var component = GameObjects.CreateGameObjectWithComponent(componentType, assetPathType, assetPath);
			return component as TComponent;
		}

		/// <summary>
		/// 게임오브젝트 + 컴포넌트 생성.
		/// </summary>
		public static TComponent CreateGameObjectWithComponent<TComponent>(string assetPath) where TComponent : Component
		{
			var assetPathType = AssetPaths.GetInferAssetPathType(assetPath);
			var componentType = typeof(TComponent);
			var component = GameObjects.CreateGameObjectWithComponent(componentType, assetPathType, assetPath);
			return component as TComponent;
		}

		/// <summary>
		/// 게임오브젝트 + 컴포넌트 생성.
		/// </summary>
		public static TComponent CreateGameObjectWithComponent<TComponent>(AssetPathAttribute assetPath) where TComponent : Component
		{
			var assetPathType = assetPath.Type;
			var assetPathValue = assetPath.Value;
			var componentType = typeof(TComponent);
			var component = GameObjects.CreateGameObjectWithComponent(componentType, assetPathType, assetPathValue);
			return component as TComponent;
		}

		/// <summary>
		/// 게임오브젝트 + 컴포넌트 생성.
		/// </summary>
		public static TComponent CreateGameObjectWithComponent<TComponent>() where TComponent : Component
		{
			var type = typeof(TComponent);
			if (Reflections.TryGetAttribute<AssetPathAttribute>(type, out var attribute))
			{
				var component = GameObjects.CreateGameObjectWithComponent<TComponent>(attribute);
				return component;
			}
			else
			{
				var typeName = type.Name;
				var gameObject = new GameObject(typeName);
				var component = GameObjects.GetOrAddComponent<TComponent>(gameObject);
				return component;
			}
		}

		/// <summary>
		/// 기존의 컴포넌트를 가져오거나 없으면 새로 추가하여 반환.
		/// </summary>
		public static Component GetOrAddComponent(this GameObject obj, Type componentType)
		{
			if (obj == null)
				return null;

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
			var component = GameObjects.GetOrAddComponent(obj, typeof(TComponent));
			return component as TComponent;
		}
	}
}