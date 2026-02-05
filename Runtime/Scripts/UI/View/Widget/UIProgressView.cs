using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 진행 뷰.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UIProgressView : UISliderView, IUIWidget
	{
		//#region INSPECTOR
		//[NonSerialized] private RectTransform m_RectTransform;
		//#endregion

		///// <summary>
		///// 렉트 트랜스폼 프로퍼티.
		///// </summary>
		//public RectTransform RectTransform => m_RectTransform;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			//if (m_RectTransform == null)
			//{
			//	m_RectTransform = GetComponent<RectTransform>();
			//}

			interactable = false;
		}
	}
}