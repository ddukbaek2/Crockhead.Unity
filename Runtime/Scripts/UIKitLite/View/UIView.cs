using Crockhead.Core;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UIKitLite
{
	/// <summary>
	/// 기본 뷰. (클래스)
	/// </summary>
	public class UIView : Disposable, IUIView
	{
		/// <summary>
		/// 뷰 어댑터.
		/// </summary>
		internal UIViewAdapter m_ViewAdapter;

		/// <summary>
		/// 렉트 트랜스폼 프로퍼티.
		/// </summary>
		public RectTransform RectTransform => m_ViewAdapter.RectTransform;

		/// <summary>
		/// 윈도우 프로퍼티.
		/// </summary>
		public IUIWindow Window => m_ViewAdapter.Window;

		/// <summary>
		/// 상위 뷰 프로퍼티.
		/// </summary>
		public IUIView Superview => m_ViewAdapter.Superview;

		/// <summary>
		/// 하위 뷰 목록 프로퍼티.
		/// </summary>
		public IEnumerable<IUIView> Subviews => m_ViewAdapter.Subviews;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIView(RectTransform rectTransform) : base()
		{
			m_ViewAdapter = new UIViewAdapter(this, rectTransform);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Disposables.Dispose(m_ViewAdapter);
		}

		/// <summary>
		/// 기존의 윈도우에서 제거되기 직전 혹은 새로운 윈도우가 추가되기 직전에 호출됨.
		/// </summary>
		protected virtual void OnWillMoveToWindow(IUIWindow window)
		{
			m_ViewAdapter.OnWillMoveToWindow(window);
		}

		/// <summary>
		/// 기존의 윈도우에서 제거된 직후 혹은 새로운 윈도우가 추가된 직후 호출됨.
		/// </summary>
		protected virtual void OnDidMoveToWindow()
		{
			m_ViewAdapter.OnDidMoveToWindow();
		}

		/// <summary>
		/// 기존의 상위 뷰에서 제거되기 직전 혹은 새로운 상위 뷰가 추가되기 직전에 호출됨.
		/// </summary>
		protected virtual void OnWillMoveToSuperview(IUIView superview)
		{
			m_ViewAdapter.OnWillMoveToSuperview(superview);
		}

		/// <summary>
		/// 기존의 상위 뷰에서 제거된 직후 혹은 새로운 상위 뷰가 추가된 직후 호출됨.
		/// </summary>
		protected virtual void OnDidMoveToSuperview()
		{
			m_ViewAdapter.OnDidMoveToSuperview();
		}

		/// <summary>
		/// 기존의 하위 뷰가 현재 뷰에 추가된 직후 호출됨.
		/// </summary>
		protected virtual void OnDidAddSubview(IUIView subview)
		{
			m_ViewAdapter.OnDidAddSubview(subview);
		}

		/// <summary>
		/// 기존의 하위 뷰가 현재 뷰에서 제거되기 직전 호출됨.
		/// </summary>
		protected virtual void OnWillRemoveSubview(IUIView subview)
		{
			m_ViewAdapter.OnWillRemoveSubview(subview);
		}

		/// <summary>
		/// 현재 뷰의 맨 마지막 위치에 하위 뷰 추가.
		/// </summary>
		public void AddSubview(IUIView view)
		{
			m_ViewAdapter.AddSubview(view);
		}

		/// <summary>
		/// 현재 뷰의 대상 위치에 하위 뷰 추가.
		/// </summary>
		public void InsertSubview(IUIView view, int index)
		{
			m_ViewAdapter.InsertSubview(view, index);
		}

		/// <summary>
		/// 현재 뷰의 대상 하위 뷰의 이전 위치에 하위 뷰 삽입.
		/// </summary>
		public void InsertSubviewAboveSibling(IUIView view, IUIView sibling)
		{
			m_ViewAdapter.InsertSubviewAboveSibling(view, sibling);
		}

		/// <summary>
		/// 현재 뷰의 대상 하위 뷰의 다음 위치에 하위 뷰 삽입.
		/// </summary>
		public void InsertSubviewBelowSibling(IUIView view, IUIView sibling)
		{
			m_ViewAdapter.InsertSubviewBelowSibling(view, sibling);
		}

		/// <summary>
		/// 대상 하위 뷰를 가장 나중에 그리게 함. (맨 위)
		/// </summary>
		public void BringSubviewToFront(IUIView view)
		{
			m_ViewAdapter.BringSubviewToFront(view);
		}

		/// <summary>
		/// 대상 하위 뷰를 가장 먼저 그리게 함. (맨 아래)
		/// </summary>
		public void SendSubviewToBack(IUIView view)
		{
			m_ViewAdapter.SendSubviewToBack(view);
		}

		/// <summary>
		/// 현재 뷰를 상위 뷰에서 제거.
		/// </summary>
		public void RemoveFromSuperview()
		{
			m_ViewAdapter.RemoveFromSuperview();
		}
	}
}