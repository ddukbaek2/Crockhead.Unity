using Crockhead.Core;
using DG.Tweening;
using System;
using System.Collections.Generic;


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
		/// 체크 목록.
		/// </summary>
		private HashSet<UIProperty> m_Properties;

		/// <summary>
		/// 이전 프로퍼티 목록.
		/// </summary>
		private Dictionary<string, string> m_Previous;

		/// <summary>
		/// 다음 프로퍼티 목록.
		/// </summary>
		private Dictionary<string, string> m_Next;

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

			m_Properties = new HashSet<UIProperty>();
			//m_Properties.Add(new UIProperty());
			m_Previous = new Dictionary<string, string>();
			m_Next = new Dictionary<string, string>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 프로퍼티 백업.
		/// </summary>
		private void SnapshotProperties(UIView view)
		{
			m_View = view;
			m_Previous.Clear();
			m_Previous.Add("", "");
		}

		/// <summary>
		/// 프로퍼티 변경점 체크.
		/// </summary>
		private void CheckDirtyProperties()
		{
			m_Next.Clear();
		}

		/// <summary>
		/// 변경 될 프로퍼티 목록에 대한 트윈 목록 생성.
		/// </summary>
		private Tween[] CreateAnimationTweens()
		{
			return new Tween[]
			{
			};
		}

		/// <summary>
		/// 준비.
		/// </summary>
		public Sequence Prepare(float duration, Action animation)
		{
			// 프로퍼티 검사.
			SnapshotProperties(m_View);
			animation?.Invoke();
			CheckDirtyProperties();

			// 병렬 시퀀스 생성.
			m_Sequence = DOTween.Sequence();
			foreach (var tween in CreateAnimationTweens())
			{
				m_Sequence.Join(tween);
			}

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
		public void Stop()
		{
			if (!m_Sequence.IsPlaying())
				return;

			m_Sequence.Kill();
		}
	}
}