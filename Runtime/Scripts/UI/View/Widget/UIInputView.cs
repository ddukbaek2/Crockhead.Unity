using TMPro;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 입력 뷰.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasRenderer))]
	public class UIInputView : TMP_InputField, IUIView
	{
		#region INSPECTOR
		#endregion

		/// <summary>
		/// 렉트 트랜스폼 프로퍼티.
		/// </summary>
		public RectTransform RectTransform => m_RectTransform;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			if (m_RectTransform == null)
			{
				m_RectTransform = GetComponent<RectTransform>();
			}
		}
	}
}