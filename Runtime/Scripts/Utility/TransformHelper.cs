using Crockhead.Core;
using Crockhead.Unity.Exceptions;
using System;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 트랜스폼 유틸리티.
	/// </summary>
	public static class TransformHelper
	{
		/// <summary>
		/// 대상 트랜스폼에 대한 값 초기화.
		/// </summary>
		public static void ResetTransform(Transform transform)
		{
			if (transform == null)
				throw new ArgumentNullException(nameof(transform));

			transform.localPosition = Vector3.zero;
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.identity;
		}

		/// <summary>
		/// 대상 렉트 트랜스폼 초기화.
		/// </summary>
		public static void ResetRectTransform(RectTransform rectTransform)
		{
			if (rectTransform == null)
				throw new ArgumentNullException(nameof(rectTransform));

			TransformHelper.ResetTransform(rectTransform.transform);

			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchoredPosition3D = Vector3.zero;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.pivot = Vector2.one * 0.5f;
		}

		/// <summary>
		/// 대상 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public static Component GetOrAddComponent(this Transform transform, Type componentType)
		{
			try
			{
				var component = TransformHelper.GetOrAddComponent(transform, string.Empty, componentType);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 대상 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public static Component GetOrAddComponent(this Transform transform, string transformPath, Type componentType)
		{
			try
			{
				var component = TransformHelper.GetOrAddComponent(transform, transformPath, componentType, true);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 대상 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public static Component GetOrAddComponent(this Transform transform, string transformPath, Type componentType, bool makeChildren)
		{
			if (componentType == null)
				throw new ArgumentNullException(nameof(componentType));
			if (!Reflections.IsBaseClass(componentType, typeof(Component)))
				throw new NotSupportedException($"Not Component Type: {componentType}");
			if (transform == null)
				throw new ArgumentNullException(nameof(transform));

			// 하위 경로가 없을 경우.
			if (string.IsNullOrWhiteSpace(transformPath))
			{
				var component = transform.GetComponent(componentType);
				if (component == null)
					component = InstantiationHelper.GetOrAddComponent(transform.gameObject, componentType);
				return component;
			}
			// 하위 경로가 있을 경우.
			else
			{
				// 기본 경로 탐색.
				var target = transform.Find(transformPath);
				if (target != null)
				{
					var component = target.GetComponent(componentType);
					if (component == null)
						component = InstantiationHelper.GetOrAddComponent(target.gameObject, componentType);
					return component;
				}
				else
				{
					// 수동으로 각 하위 노드를 순회하여 경로 탐색.
					var parent = transform;
					var transformNames = transformPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
					foreach (var transformName in transformNames)
					{
						target = parent.Find(transformName);
						if (target == null)
						{
							// 경로 생성.
							if (makeChildren)
							{
								var obj = new GameObject(transformName);
								target = obj.transform;
								target.SetParent(parent, true);
								TransformHelper.ResetTransform(target);
							}							
							else
							{
								// 실패처리.
								throw new NotFoundTransformException(transform, transformPath);
							}
						}

						parent = target;
					}

					var component = InstantiationHelper.GetOrAddComponent(target.gameObject, componentType);
					return component;
				}
			}
		}

		/// <summary>
		/// 대상 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public static TComponent GetOrAddComponent<TComponent>(this Transform transform) where TComponent : Component
		{
			try
			{
				var component = TransformHelper.GetOrAddComponent<TComponent>(transform, string.Empty);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 대상 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public static TComponent GetOrAddComponent<TComponent>(this Transform transform, string transformPath) where TComponent : Component
		{
			try
			{
				var component = TransformHelper.GetOrAddComponent<TComponent>(transform, transformPath, true);
				return component;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 대상 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public static TComponent GetOrAddComponent<TComponent>(this Transform transform, string transformPath, bool makeChildren) where TComponent : Component
		{
			try
			{
				var componentType = typeof(TComponent);
				var component = TransformHelper.GetOrAddComponent(transform, transformPath, componentType, makeChildren);
				if (component == null)
					return null;

				return component as TComponent;
			}
			catch
			{
				throw;
			}
		}
	}
}