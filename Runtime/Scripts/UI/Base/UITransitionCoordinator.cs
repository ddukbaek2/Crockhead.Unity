using Crockhead.Core;
using System.Collections;
using System.Threading.Tasks;
using DG.Tweening;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 컨트롤러 전환 조정자.
	/// </summary>
	public class UITransitionCoordinator : Disposable
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UITransitionCoordinator() : base()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		private void AppearAnimation()
		{
		}

		private void DisappearAnimation()
		{
		}

		/// <summary>
		/// 비동기 트랜지션 처리.
		/// </summary>
		public async Task TransitAsync(UIController from, UIController to, bool animated)
		{
			// 한 프레임 지나서 메인 쓰레드 전환.
			await Task.Yield();

			// 시작.
			from?.BeginAppearanceTransition(false, animated);
			to?.BeginAppearanceTransition(true, animated);

			// 트랜지션 처리.
			var fromAnimator = from != null ? new UIAnimator(from.View) : null;
			var toAnimator = to != null ? new UIAnimator(to.View) : null;
			var fromAnimationTask = fromAnimator.AnimateAsync(0.5f, AppearAnimation);
			var toAnimationTask = fromAnimator.AnimateAsync(0.5f, DisappearAnimation);

			// 비동기 대기.
			await Task.WhenAll(fromAnimationTask, toAnimationTask);

			// 종료.
			from?.EndAppearanceTransition();
			to?.EndAppearanceTransition();
		}
	}
}