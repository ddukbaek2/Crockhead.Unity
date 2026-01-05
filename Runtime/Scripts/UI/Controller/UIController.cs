using Crockhead.Core;
using System;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 컨트롤러. (모달)
	/// </summary>
	public class UIController : Disposable
	{
		/// <summary>
		/// 소속 윈도우.
		/// </summary>
		private UIWindow m_Window;

		/// <summary>
		/// 프레젠테이션 조정자.
		/// </summary>
		private UIPresentationCoordinator m_PresentationCoordinator;

		/// <summary>
		/// 모달 제출 스타일.
		/// <para>현재 컨트롤러가 Present 될 때 뒤가 보일 것인지 여부 등을 설정.</para>
		/// </summary>
		private UIModalPresentaionStyle m_ModalPresentationStyle;

		/// <summary>
		/// 소유한 뷰.
		/// </summary>
		internal UIView m_View;

		/// <summary>
		/// 트랜지션 상태.
		/// </summary>
		private UITransitionStatus m_TransitionStatus;

		/// <summary>
		/// 소속 윈도우 프로퍼티.
		/// </summary>
		public UIWindow Window { internal set => SetWindow(value); get => m_Window; }

		/// <summary>
		/// 프레젠테이션 조정자 프로퍼티.
		/// </summary>
		public UIPresentationCoordinator PresentationCoordinator { internal set => SetPresentationCoordinator(value); get => m_PresentationCoordinator; }

		/// <summary>
		/// 현재 컨트롤러가 제출되었는지 여부.
		/// </summary>
		public bool IsModal => m_PresentationCoordinator != null ? m_PresentationCoordinator.Contains(this) : false;

		/// <summary>
		/// 모달 제출 스타일 프로퍼티.
		/// </summary>
		public UIModalPresentaionStyle ModalPresentationStyle { set => m_ModalPresentationStyle = value; get => m_ModalPresentationStyle; }

		/// <summary>
		/// 소유한 뷰 프로퍼티. (동기식 자동생성)
		/// </summary>
		public UIView View
		{
			get
			{
				LoadView();
				return m_View;
			}
		}

		/// <summary>
		/// 소유한 뷰 프로퍼티. (뷰 없으면 null)
		/// </summary>
		public UIView LoadedView => m_View;

		/// <summary>
		/// 뷰 로드 여부 프로퍼티.
		/// </summary>
		public bool ViewIfLoaded => m_View != null;

		/// <summary>
		/// 내가 제출한 대상 프로퍼티. (Next)
		/// </summary>
		public UIController PresentedController => m_PresentationCoordinator.GetPresentedController(this);

		/// <summary>
		/// 나를 제출한 대상 프로퍼티. (Previous)
		/// </summary>
		public UIController PresentingController => m_PresentationCoordinator.GetPresentingController(this);

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIController() : base()
		{
			m_Window = null;
			m_View = null;
			m_TransitionStatus = UITransitionStatus.None;
			m_ModalPresentationStyle = UIModalPresentaionStyle.Fullscreen;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIController(UIWindow window) : this()
		{
			SetWindow(window);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_Window = null;

			if (m_View != null)
			{
				GameObject.Destroy(m_View.gameObject);
				m_View = null;
			}

			Debug.Log("[UIController] OnDispose()");

			//base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 윈도우 설정.
		/// </summary>
		internal void SetWindow(UIWindow window)
		{
			m_Window = window;
		}

		/// <summary>
		/// 프레젠테이션 조정자 설정.
		/// </summary>
		internal void SetPresentationCoordinator(UIPresentationCoordinator presentationCoordinator)
		{
			m_PresentationCoordinator = presentationCoordinator;
		}

		/// <summary>
		/// 뷰 로드.
		/// </summary>
		public void LoadView()
		{
			try
			{
				if (ViewIfLoaded)
					return;

				// 현재 윈도우가 없을 경우.
				if (m_Window == null)
				{
					Debug.Log("[UIController] LoadView(): Not Binding Window");
					throw new NullReferenceException(nameof(m_Window));
				}

				var parentRectTransform = m_Window?.RectTransform ?? null;

				// 뷰 로드 직전 정보를 수집하고, 정보에 따른 뷰를 생성.
				var viewLoadConfiguration = OnViewWillLoad(typeof(UIView));
				var viewType = viewLoadConfiguration.ViewType;
				var assetPath = viewLoadConfiguration.AssetPath;
				var assetPathType = viewLoadConfiguration.AssetPathType;

				// 경로가 없다면 생성.
				if (string.IsNullOrWhiteSpace(assetPath))
				{
					m_View = (UIView)UIView.Create(viewType, parentRectTransform);
				}
				// 경로가 있다면 로드.
				else
				{
					m_View = (UIView)UIView.CreateFromAsset(viewType, assetPath, assetPathType, parentRectTransform);
				}

				m_View.SetController(this);
				//m_View.gameObject.SetActive(false);
				OnViewDidLoad();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				throw;
			}
		}

		/// <summary>
		/// 뷰 로드. (비동기)
		/// </summary>
		public async Task LoadViewAsync()
		{
			if (ViewIfLoaded)
				return;

			// 현재 윈도우가 없을 경우.
			if (m_Window == null)
			{
				Debug.Log("[UIController] LoadViewAsync(): Not Binding Window");
				throw new NullReferenceException(nameof(m_Window));
			}

			var parentRectTransform = m_Window?.RectTransform ?? null;

			// 뷰 로드 직전 정보를 수집하고, 정보에 따른 뷰를 생성.
			var viewLoadConfiguration = OnViewWillLoad(typeof(UIView));
			var viewType = viewLoadConfiguration.ViewType;
			var assetPath = viewLoadConfiguration.AssetPath;
			var assetPathType = viewLoadConfiguration.AssetPathType;

			// 경로가 없다면 생성.
			if (string.IsNullOrWhiteSpace(assetPath))
			{
				var node = UIView.Create(viewType, parentRectTransform);
				m_View = node as UIView;
			}
			// 경로가 있다면 로드.
			else
			{
				var node = await UIView.CreateFromAssetAsync(viewType, assetPath, assetPathType, parentRectTransform);
				m_View = node as UIView;
			}

			m_View.SetController(this);
			//m_View.gameObject.SetActive(false);
			OnViewDidLoad();
		}

		/// <summary>
		/// 뷰 로드 직전 호출됨.
		/// <para>이를 상속 받아서 뷰 설정을 각 상속 뷰 별로 커스텀하면 특성 없이 각 뷰 마다 연결될 애셋을 개별 지정 가능.</para>
		/// </summary>
		protected virtual UIViewLoadConfiguration OnViewWillLoad(Type viewType)
		{
			if (viewType == null)
				throw new ArgumentNullException(nameof(viewType));

			var controllerType = GetType();
			var assetPathValue = string.Empty;
			var assetPathType = AssetPathType.Resources;

			// 컨트롤러에 부착된 애셋 경로 특성 사용.
			// AssetPathAttribute 혹은 ViewBindingAttribute 가 부착 되어있다는 전제. (강제사항은 아님)
			if (Reflections.TryGetAttribute<AssetPathAttribute>(controllerType, out var assetPathAttribute))
			{
				assetPathValue = assetPathAttribute.Value;
				assetPathType = assetPathAttribute.Type;

				// 뷰 바인딩 특성 사용.
				var viewBindingAttribute = assetPathAttribute as UIViewBindingAttribute;
				if (viewBindingAttribute != null)
				{
					// 지정 뷰 설정.
					viewType = viewBindingAttribute.ViewType;
				}
			}
			// 뷰에 부착된 애셋 경로 특성 사용.
			// 뷰에서 ViewBindingAttribute를 사용하는 것은 모순.
			else if (Reflections.TryGetAttribute<AssetPathAttribute>(viewType, out assetPathAttribute))
			{
				assetPathValue = assetPathAttribute.Value;
				assetPathType = assetPathAttribute.Type;
			}

			return (UIViewLoadConfiguration)(viewType, assetPathValue, assetPathType);
		}

		/// <summary>
		/// 뷰 로드됨.
		/// </summary>
		protected virtual void OnViewDidLoad()
		{
		}

		/// <summary>
		/// 뷰 나타나기 직전 호출됨.
		/// </summary>
		protected virtual void OnViewWillAppear()
		{
			View.gameObject.SetActive(true);

			m_TransitionStatus = UITransitionStatus.Appearing;
			//foreach (var child in m_Children)
			//{
			//	child.BeginAppearanceTransition(true, animated);
			//}
		}

		/// <summary>
		/// 뷰 나타난 직후 호출됨.
		/// </summary>
		protected virtual void OnViewDidAppear()
		{
			m_TransitionStatus = UITransitionStatus.Appeared;
			//foreach (var child in m_Children)
			//{
			//	child.EndAppearanceTransition();
			//}
		}

		/// <summary>
		/// 뷰 사라지기 직전 호출됨.
		/// </summary>
		protected virtual void OnViewWillDisappear()
		{
			m_TransitionStatus = UITransitionStatus.Disappearing;
			//foreach (var child in m_Children)
			//{
			//	child.BeginAppearanceTransition(false, animated);
			//}
		}

		/// <summary>
		/// 뷰 사라진 직후 호출됨.
		/// </summary>
		protected virtual void OnViewDidDisappear()
		{
			m_TransitionStatus = UITransitionStatus.Disappeared;
			//foreach (var child in m_Children)
			//{
			//	child.EndAppearanceTransition();
			//}

			View.gameObject.SetActive(false);
		}

		/// <summary>
		/// 뷰가 최상위가 된 직후 호출됨.
		/// </summary>
		protected virtual void OnViewDidBecomeTop()
		{
		}

		/// <summary>
		/// 뷰가 최상위가 아니게 된 직후 호출됨.
		/// </summary>
		protected virtual void OnViewDidResignTop()
		{
		}

		/// <summary>
		/// 제출. (현재 컨트롤러가 제출)
		/// </summary>
		public void Present(UIController controller, bool animated = false)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));		
			if (PresentedController != null)
				throw new Exception("[UIController] Already Appeared.");

			controller.Window = m_Window;
			controller.PresentationCoordinator = m_PresentationCoordinator;

			controller.LoadView();
			_ = m_PresentationCoordinator.PresentAsync(controller, animated);			
		}

		/// <summary>
		/// 제출. (현재 컨트롤러가 대상 컨트롤러를 제출)
		/// </summary>
		public async Task PresentAsync(UIController controller, bool animated = false)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));
			if (PresentedController != null)
				throw new Exception("[UIController] Already Appeared.");

			controller.Window = m_Window;
			controller.PresentationCoordinator = m_PresentationCoordinator;
			await controller.LoadViewAsync();

			await m_PresentationCoordinator.PresentAsync(controller, animated);
		}

		/// <summary>
		/// 철회. (현재 컨트롤러 자신이 스스로 철회) 
		/// </summary>
		public async Task DismissAsync(bool animated = false)
		{
			// 프레젠테이션 조정자가 없는 경우 발표되지 않은 것. 
			if (m_PresentationCoordinator == null)
				return;

			await m_PresentationCoordinator.DismissAsync(this, animated);
			m_PresentationCoordinator = null;
		}

		/// <summary>
		/// 등장/퇴장 시작 설정.
		/// </summary>
		internal void BeginAppearanceTransition(bool isAppearing, bool animated)
		{
			// 열기나 닫기가 진행 중이면 제외.
			if (m_TransitionStatus == UITransitionStatus.Appearing || m_TransitionStatus == UITransitionStatus.Disappearing)
				return;

			// 이미 열기 되었는데 또 열려고 하거나 닫기 되었는데 또 닫으려고 하면 제외.
			if ((m_TransitionStatus == UITransitionStatus.Appeared && isAppearing) || (m_TransitionStatus == UITransitionStatus.Disappeared && !isAppearing))
				return;

			if (isAppearing)
			{
				OnViewWillAppear();
			}
			else
			{
				OnViewWillDisappear();
			}
		}

		/// <summary>
		/// 등장/퇴장 완료 설정.
		/// </summary>
		internal void EndAppearanceTransition()
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

		///// <summary>
		///// 뷰 반환.
		///// </summary>
		//public TUIView GetView<TUIView>() where TUIView : UIView
		//{
		//	return (TUIView)View;
		//}

		protected Task Animate(float withDuration, Action animation, Action completion = null)
		{
			var animator = new UIAnimator(View);
			return animator.AnimateAsync(0.5f, animation);
		}
	}
}