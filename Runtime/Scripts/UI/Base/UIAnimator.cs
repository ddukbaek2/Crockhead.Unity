using Crockhead.Core;
using System;
using System.Collections.Generic;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 애니메이션 처리기.
	/// </summary>
	public class UIAnimator : Disposable
	{
		/// <summary>
		/// 대상 뷰.
		/// </summary>
		private UIView m_View;

		/// <summary>
		/// 이전 프로퍼티 목록.
		/// </summary>
		private Dictionary<string, string> m_Previous;

		/// <summary>
		/// 다음 프로퍼티 목록.
		/// </summary>
		private Dictionary<string, string> m_Next;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIAnimator() : base()
		{
			m_View = null;
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
		private void CreateAnimations()
		{
		}

		/// <summary>
		/// 애니메이션 처리.
		/// </summary>
		public async void Animate(UIView view, float duration, Action animations)
		{
			SnapshotProperties(view);
			animations?.Invoke();
			CheckDirtyProperties();
			CreateAnimations();

			//await Tasks.Wait();
		}
	}
}