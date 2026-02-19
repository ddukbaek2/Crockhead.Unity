//using DG.Tweening;
//using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 트윈 유틸리티.
	/// </summary>
	public static class UITweenHelper
	{
		/// <summary>
		/// 생성자 델리게이트.
		/// </summary>
		//public delegate Tweener CreateDelegate<TTarget, TValue>(TTarget target, TValue from, TValue to, float duration);

		/// <summary>
		/// 설정자 델리게이트.
		/// </summary>
		public delegate void Setter<TTarget, TValue>(TTarget target, TValue value);

		/// <summary>
		/// 반환자 델리게이트.
		/// </summary>
		public delegate TValue Getter<TTarget, TValue>(TTarget target);

		/// <summary>
		/// 실제 바인딩 된 트윈 생성 객체 목록.
		/// </summary>
		private static readonly Dictionary<string, Delegate> s_Creators;

		/// <summary>
		/// 실제 바인딩 된 트인 프로퍼티 반환자 목록.
		/// </summary>
		private static readonly Dictionary<string, Delegate> s_Getters;

		/// <summary>
		/// 실제 바인딩 된 트인 프로퍼티 설정자 목록.
		/// </summary>
		private static readonly Dictionary<string, Delegate> s_Setters;

		/// <summary>
		/// 프로퍼티 목록.
		/// </summary>
		private static HashSet<string> s_Properties;

		/// <summary>
		/// 프로퍼티 목록 프로퍼티.
		/// </summary>
		public static HashSet<string> Properties => s_Properties;

		/// <summary>
		/// 생성됨.
		/// </summary>
		static UITweenHelper()
		{
			s_Creators = new Dictionary<string, Delegate>();
			s_Getters = new Dictionary<string, Delegate>();
			s_Setters = new Dictionary<string, Delegate>();
			s_Properties = new HashSet<string>();

			//Register<UIView, Vector3, VectorOptions>("transform.localPosition",
			//	static (target, value) => target.transform.localPosition = value,
			//	static (target) => target.transform.localPosition);
			//Register<UIView, Vector3, VectorOptions>("transform.localScale",
			//	static (target, value) => target.transform.localScale = value,
			//	static (target) => target.transform.localScale);
			//Register<UIView, Vector3, VectorOptions>("transform.localEulerAngles",
			//	static (target, value) => target.transform.localEulerAngles = value,
			//	static (target) => target.transform.localEulerAngles);
			//Register<UIView, Vector2, VectorOptions>("rectTransform.anchoredPosition",
			//	static (target, value) => target.RectTransform.anchoredPosition = value,
			//	static (target) => target.RectTransform.anchoredPosition);
			//Register<UIView, Vector3, VectorOptions>("rectTransform.anchoredPosition3D",
			//	static (target, value) => target.RectTransform.anchoredPosition3D = value,
			//	static (target) => target.RectTransform.anchoredPosition3D);
			//Register<UIView, Vector2, VectorOptions>("rectTransform.sizeDelta",
			//	static (target, value) => target.RectTransform.sizeDelta = value,
			//	static (target) => target.RectTransform.sizeDelta);
			//Register<UIView, Vector2, VectorOptions>("rectTransform.pivot",
			//	static (target, value) => target.RectTransform.pivot = value,
			//	static (target) => target.RectTransform.pivot);
			//Register<UIView, Vector2, VectorOptions>("rectTransform.offsetMax",
			//	static (target, value) => target.RectTransform.offsetMax = value,
			//	static (target) => target.RectTransform.offsetMax);
			//Register<UIView, Vector2, VectorOptions>("rectTransform.offsetMin",
			//	static (target, value) => target.RectTransform.offsetMin = value,
			//	static (target) => target.RectTransform.offsetMin);
			//Register<UIView, Vector2, VectorOptions>("rectTransform.anchorMax",
			//	static (target, value) => target.RectTransform.anchorMax = value,
			//	static (target) => target.RectTransform.anchorMax);
			//Register<UIView, Vector2, VectorOptions>("rectTransform.anchorMin",
			//	static (target, value) => target.RectTransform.anchorMin = value,
			//	static (target) => target.RectTransform.anchorMin);
			//Register<UIView, float, FloatOptions>("canvasGroup.alpha",
			//	static (target, value) => target.GetComponent<CanvasGroup>().alpha = value,
			//	static (target) => target.GetComponent<CanvasGroup>().alpha);
		}

		/// <summary>
		/// 프로퍼티 생성.
		/// </summary>
		public static UIProperty CreateProperty(UIView target, string name)
		{
			var functor = GetGetter(name);
			if (functor == null)
				return null;

			var value = functor.DynamicInvoke(target);
			var valueType = value.GetType();
			var propertyType = typeof(UIProperty<>);
			var genericPropertyType = propertyType.MakeGenericType(valueType);
			return (UIProperty)Activator.CreateInstance(genericPropertyType, value);
		}

		///// <summary>
		///// 트윈 생성.
		///// </summary>
		//public static Tweener CreateTweener<TValue>(UIView target, string name, UIProperty from, UIProperty to, float duration)
		//{
		//	var creator = GetCreator<UIView, TValue>(name);
		//	if (creator == null)
		//		return null;

		//	var startValue = UIProperty.GetValue<TValue>(from);
		//	var endValue = UIProperty.GetValue<TValue>(to);
		//	return creator.Invoke(target, startValue, endValue, duration);
		//}

		///// <summary>
		///// 트윈 생성.
		///// </summary>
		//public static Tweener Create<TValue>(UIView target, string name, TValue from, TValue to, float duration)
		//{
		//	var creator = GetCreator<UIView, TValue>(name);
		//	if (creator == null)
		//		return null;

		//	return creator.Invoke(target, from, to, duration);
		//}

		///// <summary>
		///// 트윈 생성.
		///// </summary>
		//public static Tweener Create<TTarget, TValue>(TTarget target, string name, TValue from, TValue to, float duration)
		//{
		//	var creator = GetCreator<TTarget, TValue>(name);
		//	if (creator == null)
		//		return null;

		//	return creator.Invoke(target, from, to, duration);
		//}

		/// <summary>
		/// 생성자 반환.
		/// </summary>
		public static Delegate GetCreator(string name)
		{
			if (!s_Creators.TryGetValue(name, out var functor))
				return null;

			return functor;
		}

		/// <summary>
		/// 설정자 반환.
		/// </summary>
		public static Delegate GetSetter(string name)
		{
			if (!s_Setters.TryGetValue(name, out var functor))
				return null;

			return functor;
		}

		/// <summary>
		/// 반환자 반환.
		/// </summary>
		public static Delegate GetGetter(string name)
		{
			if (!s_Getters.TryGetValue(name, out var functor))
				return null;

			return functor;
		}

		///// <summary>
		///// 생성자 반환.
		///// </summary>
		//public static CreateDelegate<TTarget, TValue> GetCreator<TTarget, TValue>(string name)
		//{
		//	var functor = GetCreator(name);
		//	if (functor == null)
		//		return null;

		//	var creator = (CreateDelegate<TTarget, TValue>)functor;
		//	return creator;
		//}

		/// <summary>
		/// 설정자 반환.
		/// </summary>
		public static Setter<TTarget, TValue> GetSetter<TTarget, TValue>(TTarget target, string name)
		{
			var functor = GetSetter(name);
			if (functor == null)
				return null;

			var setter = (Setter<TTarget, TValue>)functor;
			return setter;
		}

		/// <summary>
		/// 반환자 반환.
		/// </summary>
		public static Getter<TTarget, TValue> GetGetter<TTarget, TValue>(TTarget target, string name)
		{
			var functor = GetGetter(name);
			if (functor == null)
				return null;

			var getter = (Getter<TTarget, TValue>)functor;
			return getter;
		}

		///// <summary>
		///// 함수자 등록.
		///// </summary>
		//private static void Register<TTarget, TValue, TPlugOptions>(string name, Setter<TTarget, TValue> setter, Getter<TTarget, TValue> getter)
		//	where TPlugOptions : struct, IPlugOptions
		//{
		//	if (string.IsNullOrWhiteSpace(name))
		//		throw new ArgumentNullException(nameof(name));
		//	if (setter == null)
		//		throw new ArgumentNullException(nameof(setter));
		//	if (getter == null)
		//		throw new ArgumentNullException(nameof(setter));

		//	// 생성.
		//	var creator = new CreateDelegate<TTarget, TValue>((target, from, to, duration) =>
		//	{
		//		setter(target, from);
		//		var tween = DOTween.To<TValue, TValue, TPlugOptions>(
		//			null,
		//			() => getter(target),
		//			(value) => setter(target, value),
		//			to,
		//			duration);
		//		return tween;
		//	});

		//	s_Getters[name] = getter;
		//	s_Setters[name] = setter;
		//	s_Creators[name] = creator;
		//	s_Properties.Add(name);
		//}
	}
}