using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 기본 뷰. (컴포넌트)
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UIView : UIBehaviour
	{
		/// <summary>
		/// 렉트 트랜스폼.
		/// </summary>
		private RectTransform m_RectTransform;

		/// <summary>
		/// 영역 프로퍼티.
		/// </summary>
		public RectTransform RectTransform => m_RectTransform;

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			m_RectTransform = GetComponent<RectTransform>();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected override void Start()
		{
			base.Start();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		/// <summary>
		/// 애니메이션.
		/// </summary>
		public static async Task Animate(UIView view, float duration, Action animation)
		{
			var animator = new UIAnimator(view);
			await animator.AnimateAsync(duration, animation);
		}
	}
}