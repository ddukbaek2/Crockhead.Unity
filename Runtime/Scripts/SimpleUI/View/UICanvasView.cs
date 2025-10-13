using UnityEngine;
using UnityEngine.UI;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 캔버스 뷰.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(Canvas))]
	public class UICanvasView : UIView
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
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_Canvas = GetComponent<Canvas>();
			m_CanvasScaler = GetComponent<CanvasScaler>();
			m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		/// <summary>
		/// .
		/// </summary>
		public void Present(UIController controller)
		{
			//controller.View
		}
	}
}