using Crockhead.Core;
using System.Collections.Generic;


namespace Crockhead.Unity.UIKitLite.Deprecated
{
	/// <summary>
	/// 네비게이션 컨트롤러.
	/// </summary>
	public sealed class UINavigationController : UIViewController
	{
		/// <summary>
		/// 스택 목록.
		/// </summary>
		private Stack<UIViewController> m_Stack;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UINavigationController() : base()
		{
			m_Stack = new Stack<UIViewController>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 뷰 로드 시작됨.
		/// </summary>
		protected override IUIView OnViewWillLoad()
		{
			//return base.OnViewWillLoad();
			return null;
		}

		public void Push(UIViewController viewController)
		{
		}

		public void Pop()
		{
		}
	}
}