namespace Crockhead.Unity.UIKitLite
{
	/// <summary>
	/// 윈도우 인터페이스.
	/// </summary>
	public interface IUIWindow : IUIView
	{
		/// <summary>
		/// 루트 뷰 컨트롤러 프로퍼티.
		/// </summary>
		UIViewController RootViewController { set; get; }

		/// <summary>
		/// 키를 생성하고 루트 뷰 컨트롤러를 윈도우에 표시.
		/// </summary>
		void MakeKeyAndVisible();
	}
}