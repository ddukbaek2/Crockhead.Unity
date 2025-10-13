using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 패널 뷰.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UIPanelView : UIView
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
		}
	}
}