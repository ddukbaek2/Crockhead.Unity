using Crockhead.Core;
using System.Collections.Generic;
using System.Threading.Tasks;


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

		private void OnViewDidAppearAnimation()
		{
		}

		private void OnViewDidDisappearAnimation()
		{
		}

		/// <summary>
		/// 비동기 트랜지션 처리.
		/// </summary>
		public async Task TransitionAsync(UIController previous, UIController next, bool animated)
		{
			//// 한 프레임 지나서 메인 쓰레드 전환.
			//await Task.Yield();
			//await DispatchQueue.Foreground.TransitionAsync(() =>
			await UnityThreadDispatcher.PostAsync(async () => 
			{
				if (animated)
				{
					var tasks = new List<Task>();
					if (previous != null)
					{
						var fromAnimator = new UIAnimator(previous.View);
						var animationTask = fromAnimator.AnimateAsync(0.5f, OnViewDidDisappearAnimation);
						tasks.Add(animationTask);
					}

					if (next != null)
					{
						var toAnimator = new UIAnimator(next.View);
						var animationTask = toAnimator.AnimateAsync(0.5f, OnViewDidAppearAnimation);
						tasks.Add(animationTask);
					}

					// 비동기 대기.
					await Task.WhenAll(tasks);
				}
				else
				{
					await Task.CompletedTask;
				}
			});
		}
	}
}