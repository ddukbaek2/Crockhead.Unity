using UnityEngine;
using UnityEngine.UI;


namespace Crockhead.Unity.UIKitLite
{
	/// <summary>
	/// 기본 윈도우. (컴포넌트)
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(Canvas))]
	public sealed class UIWindowBehaviour : UIViewBehaviour, IUIWindow
	{
		/// <summary>
		/// 캔버스.
		/// </summary>
		private Canvas m_Canvas;

		/// <summary>
		/// 캔버스 스케일러.
		/// </summary>
		private CanvasScaler m_CanvasScaler;

		/// <summary>
		/// 그래픽 레이캐스터.
		/// </summary>
		private GraphicRaycaster m_GraphicRaycaster;

		/// <summary>
		/// 루트 뷰 컨트롤러.
		/// </summary>
		private UIViewController m_RootViewController;

		/// <summary>
		/// 캔버스 프로퍼티.
		/// </summary>
		public Canvas Canvas => m_Canvas;

		/// <summary>
		/// 캔버스 스케일러 프로퍼티.
		/// </summary>
		public CanvasScaler CanvasScaler => m_CanvasScaler;

		/// <summary>
		/// 그래픽 레이캐스터 프로퍼티.
		/// </summary>
		public GraphicRaycaster GraphicRaycaster => m_GraphicRaycaster;

		/// <summary>
		/// 루트 뷰 컨트롤러 프로퍼티.
		/// </summary>
		public UIViewController RootViewController
		{
			set
			{
				var previous = m_RootViewController;
				var next = value;
				var hasPrevious = previous != null;
				m_RootViewController = next;
				m_RootViewController.Window = this;

				if (hasPrevious)
				{
					UIViewController.StartViewDisappear(previous);
					UIViewController.StartViewAppear(next);
				}
				else
				{
					UIViewController.StartViewAppear(next);
				}
			}
			get
			{
				return m_RootViewController;
			}
		}

		/// <summary>
		/// 윈도우 순서 프로퍼티.
		/// </summary>
		public int Order
		{
			set => m_Canvas.sortingOrder = value;
			get => m_Canvas.sortingOrder;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_Canvas = GetComponent<Canvas>();
			m_CanvasScaler = GetComponent<CanvasScaler>();
			m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
			m_RootViewController = null;
		}

		/// <summary>
		/// 키를 생성하고 루트 뷰 컨트롤러를 윈도우에 표시.
		/// </summary>
		public void MakeKeyAndVisible()
		{
			if (m_RootViewController == null)
				return;

			m_RootViewController.View.RectTransform.SetParent(RectTransform, false);
			UIViewController.StartViewAppear(m_RootViewController);
		}
	}
}