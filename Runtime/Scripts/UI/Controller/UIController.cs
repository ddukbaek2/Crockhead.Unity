using Crockhead.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 컨트롤러.
	/// </summary>
	public class UIController : Disposable
	{
		/// <summary>
		/// 트랜지션 상태.
		/// </summary>
		public enum UITransitionStatus
		{
			/// <summary>
			/// 없음.
			/// </summary>
			None,

			/// <summary>
			/// 등장 진행 중.
			/// </summary>
			Appearing,

			/// <summary>
			/// 등장 완료.
			/// </summary>
			Appeared,

			/// <summary>
			/// 퇴장 진행 중.
			/// </summary>
			Disappearing,

			/// <summary>
			/// 퇴장 완료.
			/// </summary>
			Disappeared,
		}


		/// <summary>
		/// 뷰.
		/// </summary>
		private UIView m_View;

		/// <summary>
		/// 트랜지션 상태.
		/// </summary>
		private UITransitionStatus m_TransitionStatus;

		/// <summary>
		/// 프레젠테이션 조정자.
		/// </summary>
		private UIPresentationCoordinator m_PresentationCoordinator;

		/// <summary>
		/// 전환 조정자.
		/// </summary>
		private UITransitionCoordinator m_TransitionCoordinator;

		/// <summary>
		/// 부모.
		/// </summary>
		private UIController m_Parent;

		/// <summary>
		/// 자식.
		/// </summary>
		private List<UIController> m_Children;

		/// <summary>
		/// 뷰가 로드 되었는지 여부 프로퍼티.
		/// </summary>
		public bool IsViewLoaded => m_View != null;

		/// <summary>
		/// 뷰 프로퍼티.
		/// </summary>
		public UIView View => LoadView();

		/// <summary>
		/// 로드 된 뷰 프로퍼티.
		/// </summary>
		public UIView ViewIfLoaded => m_View;

		/// <summary>
		/// 프레젠테이션 조정자 프로퍼티.
		/// </summary>
		public UIPresentationCoordinator PresentationCoordinator { internal set => m_PresentationCoordinator = value; get => m_PresentationCoordinator; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIController() : base()
		{
			m_View = null;
			m_TransitionStatus = UITransitionStatus.None;
			m_PresentationCoordinator = null;
			m_TransitionCoordinator = null;
			m_Parent = null;
			m_Children = new List<UIController>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 뷰 로드 시작됨.
		/// </summary>
		protected virtual void OnViewWillLoad()
		{
		}

		/// <summary>
		/// 뷰 로드 완료됨.
		/// </summary>
		protected virtual void OnViewDidLoad()
		{
		}

		/// <summary>
		/// 뷰 등장 시작됨.
		/// </summary>
		protected virtual void OnViewWillAppear(bool animated)
		{
			m_TransitionStatus = UITransitionStatus.Appearing;
			foreach (var child in m_Children)
			{
				child.BeginAppearanceTransition(true, animated);
			}
		}

		/// <summary>
		/// 뷰 등장 완료됨.
		/// </summary>
		protected virtual void OnViewDidAppear()
		{
			m_TransitionStatus = UITransitionStatus.Appeared;
			foreach (var child in m_Children)
			{
				child.EndAppearanceTransition();
			}
		}

		/// <summary>
		/// 뷰 퇴장 시작됨.
		/// </summary>
		protected virtual void OnViewWillDisappear(bool animated)
		{
			m_TransitionStatus = UITransitionStatus.Disappearing;
			foreach (var child in m_Children)
			{
				child.BeginAppearanceTransition(false, animated);
			}
		}

		/// <summary>
		/// 뷰 퇴장 완료됨.
		/// </summary>
		protected virtual void OnViewDidDisappear()
		{
			m_TransitionStatus = UITransitionStatus.Disappeared;
			foreach (var child in m_Children)
			{
				child.EndAppearanceTransition();
			}
		}

		/// <summary>
		/// 뷰 로드.
		/// </summary>
		public UIView LoadView()
		{
			if (IsViewLoaded)
				return m_View;

			OnViewWillLoad();
			var type = GetType();
			var view = UIHelper.CreateView(type);
			view.gameObject.SetActive(false);
			OnViewDidLoad();
			return m_View;
		}

		/// <summary>
		/// 비동기 뷰 로드.
		/// </summary>
		public async Task<UIView> LoadViewAsync()
		{
			IEnumerator Process()
			{
				OnViewWillLoad();
				var type = GetType();
				var view = UIHelper.CreateView(type);
				view.gameObject.SetActive(false);
				OnViewDidLoad();
				yield break;
			}

			if (IsViewLoaded)
				return ViewIfLoaded;

			await Tasks.StartForeground(Process());
			return m_View;
		}

		/// <summary>
		/// 자식으로 추가.
		/// </summary>
		public void AddChild(UIController controller)
		{
			if (controller == null)
				return;
			if (!m_Children.Contains(controller))
				return;

			controller.m_Parent = this;
			m_Children.Add(controller);

			controller.BeginAppearanceTransition(true, false);
			controller.EndAppearanceTransition();
		}

		/// <summary>
		/// 자식에서 제거.
		/// </summary>
		public void RemoveChild(UIController controller)
		{
			if (controller == null)
				return;
			if (m_Children.Contains(controller))
				return;

			controller.m_Parent = null;
			m_Children.Remove(controller);

			controller.BeginAppearanceTransition(false, false);
			controller.EndAppearanceTransition();
		}

		/// <summary>
		/// 표시.
		/// <para>프레젠테이션 체인에서 가장 뒤에 추가됨.</para>
		/// </summary>
		public void Present(UIController controller, bool animated)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			try
			{
				// 현재 컨트롤러의 발표자와 동일한 도메인으로 조정자 확정.
				controller.PresentationCoordinator = m_PresentationCoordinator;
				controller.m_PresentationCoordinator.Present(controller, animated);
			}
			catch
			{
				throw;
			}
			finally
			{

			}
		}

		/// <summary>
		/// 표시 중단. (철회)
		/// <para>가장 나중에 열린 객체부터 현재 객체까지 모든 열린 객체는 역순으로 닫힘.</para>
		/// </summary>
		public void Retract(bool animated)
		{
			// 프레젠테이션 조정자가 없는 경우 발표되지 않은 것. 
			if (m_PresentationCoordinator == null)
				return;

			try
			{
				m_PresentationCoordinator.Retract(this, animated);
			}
			catch
			{
				throw;
			}
			finally
			{
				m_PresentationCoordinator = null;
			}
		}

		/// <summary>
		/// 등장/퇴장 시작 설정.
		/// </summary>
		public void BeginAppearanceTransition(bool isAppearing, bool animated)
		{
			// 열기나 닫기가 진행 중이면 제외.
			if (m_TransitionStatus == UITransitionStatus.Appearing || m_TransitionStatus == UITransitionStatus.Disappearing)
				return;

			// 이미 열기 되었는데 또 열려고 하거나 닫기 되었는데 또 닫으려고 하면 제외.
			if ((m_TransitionStatus == UITransitionStatus.Appeared && isAppearing) || (m_TransitionStatus == UITransitionStatus.Disappeared && !isAppearing))
				return;

			if (isAppearing)
			{
				OnViewWillAppear(animated);
			}
			else
			{
				OnViewWillDisappear(animated);
			}
		}

		/// <summary>
		/// 등장/퇴장 완료 설정.
		/// </summary>
		public void EndAppearanceTransition()
		{
			switch (m_TransitionStatus)
			{
				case UITransitionStatus.Appearing:
					{
						OnViewDidAppear();
						break;
					}

				case UITransitionStatus.Disappearing:
					{
						OnViewDidDisappear();
						break;
					}
			}
		}

		/// <summary>
		/// 전환 조정자 참조 설정. (내부용)
		/// </summary>
		internal void SetTransitionCoordinator(UITransitionCoordinator transitionCoordinator)
		{
			m_TransitionCoordinator = transitionCoordinator;
		}

		/// <summary>
		/// 자식 여부.
		/// </summary>
		public bool IsChildren(UIController controller)
		{
			return m_Children.Contains(controller);
		}
	}
}