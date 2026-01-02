using Crockhead.Core;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 애니메이션.
	/// <para>프로퍼티 목록으로 from, to 전환.</para>
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
		private HashSet<string> m_Properties;

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
			m_Properties = new HashSet<string>();
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
			foreach (var propertyName in UITweenHelper.Properties)
			{
				var property = UITweenHelper.CreateProperty(view, propertyName);
				if (property == null)
					continue;

				properties[propertyName] = property;
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

			// 차이점 파악.
			m_Properties.Clear();
			foreach (var pair in m_Previous)
			{
				var previous = pair.Value;
				var next = m_Next[pair.Key];
				if (previous == next)
					continue;

				m_Properties.Add(pair.Key);
			}
		}

		/// <summary>
		/// 변경 될 프로퍼티 목록에 대한 병렬 트윈 목록 생성.
		/// </summary>
		private Sequence CreateSequence(float duration)
		{
			m_Sequence = DOTween.Sequence();
			if (m_Properties.Count == 0)
				return m_Sequence;

			// 변경점에 대해서 트윈 생성 및 시퀀스에 추가.
			foreach (var property in m_Properties)
			{
				var from = m_Previous[property];
				var to = m_Next[property];
				var tweener = UITweenHelper.CreateTweener<Vector3>(m_View, property, from, to, duration);
				if (tweener == null)
					continue;

				tweener = tweener.SetEase(Ease.Linear).SetUpdate(true).SetAutoKill(false);
				m_Sequence.Join(tweener);
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