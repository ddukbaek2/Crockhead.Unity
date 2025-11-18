using Crockhead.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UIKitLite.Deprecated
{
	/// <summary>
	/// UI의 논리적 객체.
	/// <para>시각적 단위인 IUIView를 관리 및 제어하는 주체.</para>
	/// </summary>
	public class UIViewController : Disposable
	{
		/// <summary>
		/// 윈도우.
		/// </summary>
		private IUIWindow m_Window;

		/// <summary>
		/// 부모.
		/// </summary>
		private UIViewController m_Parent;

		/// <summary>
		/// 자식 목록.
		/// </summary>
		private List<UIViewController> m_Children;

		/// <summary>
		/// 현재 객체가 전시한 객체.
		/// </summary>
		private UIViewController m_PresentedViewController;

		/// <summary>
		/// 현재 객체를 전시한 객체.
		/// </summary>
		private UIViewController m_PresentingViewController;

		/// <summary>
		/// 전시 시작 진행 중 여부.
		/// </summary>
		private bool m_IsBeingPresented;

		/// <summary>
		/// 전시 중단 진행 중 여부.
		/// </summary>
		private bool m_IsBeingDismissed;

		/// <summary>
		/// 뷰.
		/// </summary>
		private IUIView m_View;

		/// <summary>
		/// 윈도우 프로퍼티.
		/// </summary>
		public IUIWindow Window
		{
			internal set => m_Window = value;
			get => m_Window;
		}

		/// <summary>
		/// 부모 프로퍼티.
		/// </summary>
		public UIViewController Parent => m_Parent;

		/// <summary>
		/// 자식 목록 프로퍼티.
		/// </summary>
		public IEnumerable<UIViewController> Children => m_Children;

		/// <summary>
		/// 현재 객체가 전시한 객체 프로퍼티.
		/// </summary>
		public UIViewController PresentedViewController => m_PresentedViewController;

		/// <summary>
		/// 현재 객체를 전시한 객체 프로퍼티.
		/// </summary>
		public UIViewController PresentingViewController => m_PresentingViewController;

		/// <summary>
		/// 전시 시작 진행 중 여부 프로퍼티.
		/// </summary>
		public bool IsBeingPresented => m_IsBeingPresented;

		/// <summary>
		/// 전시 중단 진행 중 여부 프로퍼티.
		/// </summary>
		public bool IsBeingDismissed => m_IsBeingDismissed;

		/// <summary>
		/// 뷰 로드 여부 프로퍼티.
		/// </summary>
		public bool IsViewLoaded => m_View != null;

		/// <summary>
		/// 뷰 프로퍼티. (접근시 로드되어있지 않으면 로드)
		/// </summary>
		public IUIView View
		{
			get
			{
				if (m_View == null)
				{
					m_View = OnViewWillLoad();
					OnViewDidLoad();
				}

				return m_View;
			}
		}

		/// <summary>
		/// 로드 된 뷰 프로퍼티.
		/// </summary>
		public IUIView ViewIfLoaded => m_View;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIViewController() : base()
		{
			m_Parent = null;
			m_Children = new List<UIViewController>();
			m_PresentedViewController = null;
			m_PresentingViewController = null;
			m_IsBeingPresented = false;
			m_IsBeingDismissed = false;
			m_View = null;
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
		protected virtual IUIView OnViewWillLoad()
		{
			if (IsViewLoaded)
				return m_View;

			var type = GetType();
			var view = UIHelper.CreateView(type);
			return view;
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
		protected virtual void OnViewWillAppear()
		{
		}

		/// <summary>
		/// 뷰 등장 완료됨.
		/// </summary>
		protected virtual void OnViewDidAppear()
		{
		}

		/// <summary>
		/// 뷰 퇴장 시작됨.
		/// </summary>
		protected virtual void OnViewWillDisappear()
		{
		}

		/// <summary>
		/// 뷰 퇴장 완료됨.
		/// </summary>
		protected virtual void OnViewDidDisappear()
		{
		}

		/// <summary>
		/// 뷰 레이아웃 시작됨.
		/// </summary>
		protected virtual void OnViewWillLayout()
		{
		}

		/// <summary>
		/// 전시 시작.
		/// </summary>
		public void Present(UIViewController viewController)
		{
			viewController.m_Window = m_Window;
			m_PresentedViewController = viewController;
			viewController.m_PresentingViewController = this;

			// 현재 전시중인 대상은 퇴장.
			StartViewDisappear(this);

			// 새로 전시된 대상은 등장.
			viewController.m_IsBeingPresented = true;
			StartViewAppear(viewController, completion: () => { viewController.m_IsBeingPresented = false; });
		}

		/// <summary>
		/// 전시 중단.
		/// </summary>
		public void Dismiss(bool animated = false, Action completion = null)
		{
			// 현재 전시중인 대상은 퇴장.
			m_IsBeingDismissed = true;
			m_PresentingViewController.m_PresentedViewController = null;
			StartViewDisappear(this, completion: () => { m_IsBeingDismissed = false; completion?.Invoke(); });

			//현재 전시중인 대상을 전시시킨 이전 전시 대상은 재등장.
			StartViewAppear(m_PresentingViewController);
		}

		/// <summary>
		/// 뷰 등장. (내부용)
		/// </summary>
		internal static void StartViewAppear(UIViewController viewController, bool animated = false, Action initializer = null, Action completion = null)
		{
			var dispatchQueue = UIApplication.SharedInstance.DispatchQueue;
			dispatchQueue.StartAsync(initializer);
			dispatchQueue.StartAsync(() => viewController.OnViewWillAppear());
			if (animated)
			{
				// Transtioning.
			}
			dispatchQueue.StartAsync(() => viewController.OnViewDidAppear());
			dispatchQueue.StartAsync(completion);
		}

		/// <summary>
		/// 뷰 퇴장. (내부용)
		/// </summary>
		internal static void StartViewDisappear(UIViewController viewController, bool animated = false, Action initializer = null, Action completion = null)
		{
			var dispatchQueue = UIApplication.SharedInstance.DispatchQueue;
			dispatchQueue.StartAsync(initializer);
			dispatchQueue.StartAsync(() => viewController.OnViewWillDisappear());
			if (animated)
			{
				// Transtioning.
			}
			dispatchQueue.StartAsync(() => viewController.OnViewDidDisappear());
			dispatchQueue.StartAsync(completion);
		}
	}
}