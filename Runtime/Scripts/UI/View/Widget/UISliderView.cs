using System;
using UnityEngine;
using UnityEngine.UI;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 슬라이더 뷰.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UISliderView : Slider, IUIWidget
	{
		#region INSPECTOR
		[NonSerialized] private RectTransform m_RectTransform;
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