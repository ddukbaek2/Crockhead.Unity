using System;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 뷰 컨트롤러. (컨트롤러를 뷰와 별도로 생성하지 않고 외부에서 참조하는 형태)
	/// </summary>
	public class UIViewController : UIController
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIViewController(UIView view) : base()
		{
			if (view == null)
				throw new ArgumentNullException(nameof(view));

			// 뷰에 값이 생기면 더이상 로드될 일은 없음.
			m_View = view;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_View = null;

			base.OnDispose(explicitDisposing);
		}
	}
}