using Crockhead.Core;
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
		public static Component GetOrAddComponent(Type componentType, Transform transform, string transformPath = "")
		{
			if (componentType == null)
				throw new ArgumentNullException(nameof(componentType));
			if (!Reflections.IsBaseClass(componentType, typeof(Component)))
				throw new NotSupportedException($"[TransformHelper] Not Supported Type: {componentType}");
			if (transform == null)
				throw new ArgumentNullException(nameof(transform));

			// 하위 경로가 없을 경우.
			if (string.IsNullOrWhiteSpace(transformPath))
			{
				var component = transform.GetComponent(componentType);
				if (component == null)
					component = transform.gameObject.AddComponent(componentType);
				return component;
			}
			// 하위 경로가 있을 경우.
			else
			{
				// 기본 경로 검색.
				var target = transform.Find(transformPath);
				if (target != null)
				{
					var component = target.GetComponent(componentType);
					if (component == null)
						component = target.gameObject.AddComponent(componentType);
					return component;
				}
				// 수동 각 경로 검색.
				// 경로가 없으면 무조건 생성.
				else
				{
					var parent = transform;
					var transformNames = transformPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
					foreach (var transformName in transformNames)
					{
						target = parent.Find(transformName);
						if (target == null)
						{
							var obj = new GameObject(transformName);
							target = obj.transform;
							target.SetParent(parent, true);
							target.localPosition = Vector3.zero;
							target.localScale = Vector3.one;
							target.localRotation = Quaternion.identity;
						}

						parent = target;
					}

					var component = target.gameObject.AddComponent(componentType);
					return component;
				}
			}
		}

		/// <summary>
		/// 대상 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public static TComponent GetOrAddComponent<TComponent>(Transform transform, string transformPath = "") where TComponent : Component
		{
			try
			{
				var componentType = typeof(TComponent);
				var component = GetOrAddComponent(componentType, transform, transformPath);
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