using Crockhead.Core;
//using DG.Tweening;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity.UI
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
		public bool IsPlaying => false;// m_Animation.Sequence.IsPlaying();

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
			static IEnumerator Process(UIAnimation animation)
			{
				//yield return new WaitForSeconds(animation.Duration);
				//yield return animation.Sequence.WaitForCompletion();
				yield break;
			}

			//m_Animation.Prepare(duration, action);
			//m_Animation.Play();
			await TaskHelper.StartForeground(Process(m_Animation));
		}

		/// <summary>
		/// 비동기 애니메이션 처리. (외부에서 Prepare가 호출 되어있다고 간주)
		/// </summary>
		public async Task SkipedPrepareAnimateAsync()
		{
			static IEnumerator Process(UIAnimation animation)
			{
				//yield return animation.Sequence.WaitForCompletion();
				yield break;
			}

			//m_Animation.Play();
			//m_Animation.Sequence.WaitForCompletion();
			await TaskHelper.StartForeground(Process(m_Animation));
		}
	}
}