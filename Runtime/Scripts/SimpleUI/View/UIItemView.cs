using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 항목 뷰.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UIItemView : UIView
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