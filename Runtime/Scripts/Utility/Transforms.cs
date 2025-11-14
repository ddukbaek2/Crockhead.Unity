//using UnityEngine;


//namespace Crockhead.Unity
//{
//	/// <summary>
//	/// 트랜스폼 유틸리티.
//	/// </summary>
//	public static class Transforms
//	{
//		/// <summary>
//		/// 트랜스폼 초기화.
//		/// </summary>
//		public static void ResetTransform(this Transform transform)
//		{
//			if (transform == null)
//				return;

//			transform.localPosition = Vector3.zero;
//			transform.localScale = Vector3.one;
//			transform.localRotation = Quaternion.identity;

//			//Debug.Log("Transforms.ResetTransform()");
//		}

//		/// <summary>
//		/// 렉트 트랜스폼 초기화.
//		/// </summary>
//		public static void ResetRectTransform(this RectTransform rectTransform)
//		{
//			if (rectTransform == null)
//				return;

//			Transforms.ResetTransform(rectTransform.transform);

//			rectTransform.anchorMin = Vector2.zero;
//			rectTransform.anchorMax = Vector2.one;
//			rectTransform.anchoredPosition3D = Vector3.zero;
//			rectTransform.sizeDelta = Vector2.zero;
//			rectTransform.pivot = Vector2.one * 0.5f;

//			//Debug.Log("Transforms.ResetRectTransform()");
//		}
//	}
//}