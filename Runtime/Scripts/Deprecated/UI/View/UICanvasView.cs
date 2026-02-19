//using UnityEngine;
//using UnityEngine.UI;


//namespace Crockhead.Unity.UI.Deprecated
//{
//	/// <summary>
//	/// 캔버스 뷰.
//	/// </summary>
//	[ExecuteAlways]
//	[RequireComponent(typeof(Canvas))]
//	public class UICanvasView : UIView
//	{
//		/// <summary>
//		/// 프레젠테이션 조정자.
//		/// </summary>
//		private UIPresentationCoordinator m_PresentationCoordinator;

//		/// <summary>
//		/// 시작 컨트롤러.
//		/// </summary>
//		private UIController m_TargetController;

//		/// <summary>
//		/// 캔버스.
//		/// </summary>
//		private Canvas m_Canvas;

//		/// <summary>
//		/// 캔버스 스케일러.
//		/// </summary>
//		private CanvasScaler m_CanvasScaler;

//		/// <summary>
//		/// 그래픽 레이캐스터.
//		/// </summary>
//		private GraphicRaycaster m_GraphicRaycaster;

//		/// <summary>
//		/// 캔버스 프로퍼티.
//		/// </summary>
//		public Canvas Canvas => m_Canvas;

//		/// <summary>
//		/// 캔버스 스케일러 프로퍼티.
//		/// </summary>
//		public CanvasScaler CanvasScaler => m_CanvasScaler;

//		/// <summary>
//		/// 그래픽 레이캐스터 프로퍼티.
//		/// </summary>
//		public GraphicRaycaster GraphicRaycaster => m_GraphicRaycaster;

//		/// <summary>
//		/// 생성됨.
//		/// </summary>
//		protected override void Awake()
//		{
//			base.Awake();

//			m_PresentationCoordinator = new UIPresentationCoordinator();
//			m_TargetController = null;
//			m_Canvas = GetComponent<Canvas>();
//			m_CanvasScaler = GetComponent<CanvasScaler>();
//			m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
//		}

//		/// <summary>
//		/// 파괴됨.
//		/// </summary>
//		protected override void OnDestroy()
//		{
//			base.OnDestroy();
//		}

//		/// <summary>
//		/// 표시.
//		/// </summary>
//		public void Present(UIController controller)
//		{
//			if (controller == null)
//				return;

//			if (controller.PresentationCoordinator != null)
//				return;

//			if (m_TargetController != null)
//				return;

//			m_TargetController = controller;

//			// 체인 설정 및 출력.
//			m_TargetController.PresentationCoordinator = m_PresentationCoordinator;
//			m_PresentationCoordinator.Present(m_TargetController, false);
//		}
//	}
//}