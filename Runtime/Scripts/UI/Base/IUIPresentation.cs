using System.Threading.Tasks;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 프레젠테이션 인터페이스.
	/// </summary>
	public interface IUIPresentation
	{
		/// <summary>
		/// 현재 컨트롤러를 제출한 컨트롤러 프로퍼티. (Previous)
		/// </summary>
		UIController PresentingController { get; }

		/// <summary>
		/// 현재 컨트롤러가 제출한 컨트롤러 프로퍼티. (Next)
		/// </summary>
		UIController PresentedController { get; }

		/// <summary>
		/// 제출.
		/// </summary>
		void Present(UIController controller, bool animated = false);

		/// <summary>
		/// 철회.
		/// </summary>
		void Dismiss(bool animated = false);

		/// <summary>
		/// 제출. (비동기)
		/// </summary>
		Task PresentAsync(UIController controller, bool animated = false);

		/// <summary>
		/// 철회.
		/// </summary>
		Task DismissAsync(bool animated = false);
	}
}