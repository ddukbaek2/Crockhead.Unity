using Crockhead.Core;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 애니메이션.
	/// <para>프로퍼티 목록으로 previous, next 전환.</para>
	/// </summary>
	public class UIAnimation : Disposable
	{
		/// <summary>
		/// 대상 뷰.
		/// </summary>
		private UIView m_View;

		/// <summary>
		/// 트윈 목록에 대한 그룹 시퀀스.
		/// </summary>
		private Sequence m_Sequence;

		/// <summary>
		/// 이전 프로퍼티 목록.
		/// </summary>
		private Dictionary<string, UIProperty> m_Previous;

		/// <summary>
		/// 다음 프로퍼티 목록.
		/// </summary>
		private Dictionary<string, UIProperty> m_Next;

		/// <summary>
		/// 변경이 있는 프로퍼티 목록.
		/// </summary>
		private HashSet<string> m_Targets;

		/// <summary>
		/// 시퀀스 프로퍼티.
		/// </summary>
		public Sequence Sequence => m_Sequence;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIAnimation(UIView view) : base()
		{
			m_View = view;
			m_Sequence = DOTween.Sequence();
			m_Previous = new Dictionary<string, UIProperty>();
			m_Next = new Dictionary<string, UIProperty>();
			m_Targets = new HashSet<string>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 프로퍼티 수집.
		/// </summary>
		private void CollectProperties(UIView view, ref Dictionary<string, UIProperty> properties)
		{
			properties.Add("gameObject.activeSelf", new UIProperty<bool>("gameObject.activeSelf", view.gameObject.activeSelf));
			properties.Add("transform.localPosition", new UIProperty<Vector3>("transform.localPosition", view.transform.localPosition));
			properties.Add("transform.localScale", new UIProperty<Vector3>("transform.localScale", view.transform.localScale));
			properties.Add("transform.localEulerAngles", new UIProperty<Vector3>("transform.localEulerAngles", view.transform.localEulerAngles));

			if (view.RectTransform != null)
			{
				properties.Add("rectTransform.anchoredPosition", new UIProperty<Vector2>("rectTransform.anchoredPosition", view.RectTransform.anchoredPosition));
				properties.Add("rectTransform.anchoredPosition3D", new UIProperty<Vector3>("rectTransform.anchoredPosition3D", view.RectTransform.anchoredPosition3D));
				properties.Add("rectTransform.sizeDelta", new UIProperty<Vector2>("rectTransform.sizeDelta", view.RectTransform.sizeDelta));
				properties.Add("rectTransform.offsetMax", new UIProperty<Vector2>("rectTransform.offsetMax", view.RectTransform.offsetMax));
				properties.Add("rectTransform.offsetMin", new UIProperty<Vector2>("rectTransform.offsetMin", view.RectTransform.offsetMin));
				properties.Add("rectTransform.pivot", new UIProperty<Vector2>("rectTransform.pivot", view.RectTransform.pivot));
				properties.Add("rectTransform.anchorMax", new UIProperty<Vector2>("rectTransform.anchorMax", view.RectTransform.anchorMax));
				properties.Add("rectTransform.anchorMin", new UIProperty<Vector2>("rectTransform.anchorMin", view.RectTransform.anchorMin));
			}

			if (view.CanvasGroup != null)
			{
				properties.Add("canvasGroup.alpha", new UIProperty<float>("canvasGroup.alpha", view.CanvasGroup.alpha));
			}
		}

		/// <summary>
		/// 프로퍼티 백업.
		/// </summary>
		private void BeginProperties(UIView view)
		{
			m_View = view;
			BeginProperties();
		}

		private void BeginProperties()
		{
			m_Previous.Clear();
			CollectProperties(m_View, ref m_Previous);
		}

		/// <summary>
		/// 프로퍼티 변경점 체크.
		/// </summary>
		private void EndProperties()
		{
			m_Next.Clear();
			CollectProperties(m_View, ref m_Next);

			m_Targets.Clear();
			foreach (var pair in m_Previous)
			{
				var previous = pair.Value;
				var next = m_Next[pair.Key];
				if (previous == next)
					continue;

				m_Targets.Add(pair.Key);
			}
		}

		/// <summary>
		/// 변경 될 프로퍼티 목록에 대한 병렬 트윈 목록 생성.
		/// </summary>
		private Sequence CreateSequence(float duration)
		{
			m_Sequence = DOTween.Sequence();
			if (m_Targets.Count == 0)
				return m_Sequence;

			// 변경점에 대해서 트윈 생성 및 시퀀스에 추가..
			foreach (var target in m_Targets)
			{
				var previous = m_Previous[target];
				var next = m_Next[target];
				var tween = default(Tweener);

				switch (target)
				{
					case "gameObject.activeSelf":
						{
							break;
						}

					case "transform.localPosition":
						{
							var from = UIProperty.GetValue<Vector3>(previous);
							var to = UIProperty.GetValue<Vector3>(next);
							tween = DOTween.To(() => m_View.transform.localPosition, (value) => m_View.transform.localPosition = value, to, duration).From(from);
							break;
						}

					case "transform.localScale":
						{
							var from = UIProperty.GetValue<Vector3>(previous);
							var to = UIProperty.GetValue<Vector3>(next);
							tween = DOTween.To(() => m_View.transform.localScale, (value) => m_View.transform.localScale = value, to, duration).From(from);
							break;
						}

					case "transform.localEulerAngles":
						{
							var from = UIProperty.GetValue<Vector3>(previous);
							var to = UIProperty.GetValue<Vector3>(next);
							tween = DOTween.To(() => m_View.transform.localEulerAngles, (value) => m_View.transform.localEulerAngles = value, to, duration).From(from);
							break;
						}

					case "rectTransform.anchoredPosition":
						{
							var from = UIProperty.GetValue<Vector2>(previous);
							var to = UIProperty.GetValue<Vector2>(next);
							tween = DOTween.To(() => m_View.RectTransform.anchoredPosition, (value) => m_View.RectTransform.anchoredPosition = value, to, duration).From(from);
							break;
						}

					case "rectTransform.anchoredPosition3D":
						{
							var from = UIProperty.GetValue<Vector3>(previous);
							var to = UIProperty.GetValue<Vector3>(next);
							tween = DOTween.To(() => m_View.RectTransform.anchoredPosition3D, (value) => m_View.RectTransform.anchoredPosition3D = value, to, duration).From(from);
							break;
						}

					case "rectTransform.sizeDelta":
						{
							var from = UIProperty.GetValue<Vector2>(previous);
							var to = UIProperty.GetValue<Vector2>(next);
							tween = DOTween.To(() => m_View.RectTransform.sizeDelta, (value) => m_View.RectTransform.sizeDelta = value, to, duration).From(from);
							break;
						}

					case "rectTransform.offsetMax":
						{
							var from = UIProperty.GetValue<Vector2>(previous);
							var to = UIProperty.GetValue<Vector2>(next);
							tween = DOTween.To(() => m_View.RectTransform.offsetMax, (value) => m_View.RectTransform.offsetMax = value, to, duration).From(from);
							break;
						}

					case "rectTransform.offsetMin":
						{
							var from = UIProperty.GetValue<Vector2>(previous);
							var to = UIProperty.GetValue<Vector2>(next);
							tween = DOTween.To(() => m_View.RectTransform.offsetMin, (value) => m_View.RectTransform.offsetMin = value, to, duration).From(from);
							break;
						}

					case "rectTransform.anchorMax":
						{
							var from = UIProperty.GetValue<Vector2>(previous);
							var to = UIProperty.GetValue<Vector2>(next);
							tween = DOTween.To(() => m_View.RectTransform.anchorMax, (value) => m_View.RectTransform.anchorMax = value, to, duration).From(from);
							break;
						}

					case "rectTransform.anchorMin":
						{
							var from = UIProperty.GetValue<Vector2>(previous);
							var to = UIProperty.GetValue<Vector2>(next);
							tween = DOTween.To(() => m_View.RectTransform.anchorMin, (value) => m_View.RectTransform.anchorMin = value, to, duration).From(from);
							break;
						}

					case "canvasGroup.alpha":
						{
							var from = UIProperty.GetValue<float>(previous);
							var to = UIProperty.GetValue<float>(next);
							tween = DOTween.To(() => m_View.CanvasGroup.alpha, (value) => m_View.CanvasGroup.alpha = value, to, duration).From(from);
							break;
						}
				}

				if (tween == null)
					continue;

				tween = tween.SetEase(Ease.Linear).SetUpdate(true).SetAutoKill(false);
				m_Sequence.Join(tween);
			}

			return m_Sequence;
		}

		/// <summary>
		/// 준비.
		/// </summary>
		public Sequence Prepare(float duration, Action animation)
		{
			// 프로퍼티 검사.
			BeginProperties(m_View);
			animation?.Invoke();
			EndProperties();
			CreateSequence(duration);
			return m_Sequence;
		}

		/// <summary>
		/// 시작.
		/// </summary>
		public void Play()
		{
			if (m_Sequence.IsPlaying())
				return;

			m_Sequence.Play();
		}

		/// <summary>
		/// 정지.
		/// </summary>
		public void Stop(bool completed = false)
		{
			if (!m_Sequence.IsPlaying())
				return;

			if (completed)
			{
				m_Sequence.Complete();
			}
			else
			{
				m_Sequence.Kill();
			}
		}
	}
}