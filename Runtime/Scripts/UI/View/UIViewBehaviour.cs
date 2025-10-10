//using System.Collections.Generic;
using Crockhead.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 기본 뷰. (컴포넌트)
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UIViewBehaviour : UIBehaviour, IUIView
	{
		/// <summary>
		/// 뷰.
		/// </summary>
		internal UIViewAdapter m_ViewAdapter;

		/// <summary>
		/// 영역 프로퍼티.
		/// </summary>
		public RectTransform RectTransform => m_ViewAdapter.RectTransform;

		/// <summary>
		/// 윈도우 프로퍼티.
		/// </summary>
		public UIWindow Window => m_ViewAdapter.Window;

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
		protected override void Awake()
		{
			var rectTransform = GetComponent<RectTransform>();
			m_ViewAdapter = new UIViewAdapter(this, rectTransform);
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			Disposables.Dispose(m_ViewAdapter);
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