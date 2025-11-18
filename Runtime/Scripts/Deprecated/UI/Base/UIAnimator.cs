using Crockhead.Core;
using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;


namespace Crockhead.Unity.UI.Deprecated
{
	/// <summary>
	/// 애니메이션 처리기.
	/// </summary>
	public class UIAnimator : Disposable
	{
		/// <summary>
		/// 애니메이션.
		/// </summary>
		private UIAnimation m_Animation;

		/// <summary>
		/// 애니메이션 진행 중 여부 프로퍼티.
		/// </summary>
		public bool IsPlaying => m_Animation.Sequence.IsPlaying();

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIAnimator(UIView view) : base()
		{
			m_Animation = new UIAnimation(view);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIAnimator(UIAnimation animation) : base()
		{
			m_Animation = animation;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 비동기 애니메이션 처리.
		/// </summary>
		public async Task AnimateAsync(float duration, Action action)
		{
			static IEnumerator Routine(UIAnimation animation)
			{				
				yield return animation.Sequence.WaitForCompletion();
			}

			m_Animation.Prepare(duration, action);
			m_Animation.Play();
			await TaskHelper.StartForeground(Routine(m_Animation));
		}

		/// <summary>
		/// 비동기 애니메이션 처리.
		/// </summary>
		public async Task AnimateAsync()
		{
			static IEnumerator Routine(UIAnimation animation)
			{
				yield return animation.Sequence.WaitForCompletion();
			}

			m_Animation.Play();
			await TaskHelper.StartForeground(Routine(m_Animation));
		}
	}
}