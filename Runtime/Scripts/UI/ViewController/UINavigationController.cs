using Crockhead.Core;
using System.Collections.Generic;


namespace Crockhead.Unity.UI
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

		public void Push(UIViewController viewController)
		{
		}

		public void Pop()
		{
		}
	}
}