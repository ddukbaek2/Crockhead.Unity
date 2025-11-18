using Crockhead.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UIKitLite.Deprecated
{
	/// <summary>
	/// 뷰. (클래스)
	/// <para>IUIView의 실제 기능 구현체.</para>
	/// </summary>
	public sealed class UIViewAdapter : Disposable
	{
		private delegate void OnAddContainerEvent(UIViewAdapter superview, UIViewAdapter subview);
		private delegate void OnAddTransformEvent(UIViewAdapter superview, UIViewAdapter subview);
		private delegate void OnRemovedEvent(UIViewAdapter subview, UIViewAdapter removedSuperview, IUIWindow removedWindow);

		/// <summary>
		/// 대상 뷰 프로퍼티.
		/// </summary>
		public IUIView View { get; }

		/// <summary>
		/// 렉트 트랜스폼 프로퍼티.
		/// </summary>
		public RectTransform RectTransform { get; }

		/// <summary>
		/// 윈도우 프로퍼티.
		/// </summary>
		public IUIWindow Window { internal set; get; }

		/// <summary>
		/// 상위 뷰 프로퍼티.
		/// </summary>
		public IUIView Superview { internal set; get; }

		/// <summary>
		/// 하위 뷰 목록 프로퍼티.
		/// </summary>
		public List<IUIView> Subviews { internal set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIViewAdapter(IUIView view, RectTransform rectTransform)
		{
			View = view;
			RectTransform = rectTransform;

			Window = null;
			Superview = null;
			Subviews = new List<IUIView>();

			var viewType = view.GetType();
			UIHelper.SetViewEvents(viewType);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 기존의 윈도우에서 제거되기 직전 혹은 새로운 윈도우가 추가되기 직전에 호출됨.
		/// </summary>
		internal void OnWillMoveToWindow(IUIWindow window)
		{
			UIHelper.ExecuteViewEvent(View, "OnWillMoveToWindow", window);
		}

		/// <summary>
		/// 기존의 윈도우에서 제거된 직후 혹은 새로운 윈도우가 추가된 직후 호출됨.
		/// </summary>
		internal void OnDidMoveToWindow()
		{
			UIHelper.ExecuteViewEvent(View, "OnDidMoveToWindow");
		}

		/// <summary>
		/// 기존의 상위 뷰에서 제거되기 직전 혹은 새로운 상위 뷰가 추가되기 직전에 호출됨.
		/// </summary>
		internal void OnWillMoveToSuperview(IUIView superview)
		{
			UIHelper.ExecuteViewEvent(View, "OnWillMoveToSuperview", superview);
		}

		/// <summary>
		/// 기존의 상위 뷰에서 제거된 직후 혹은 새로운 상위 뷰가 추가된 직후 호출됨.
		/// </summary>
		internal void OnDidMoveToSuperview()
		{
			UIHelper.ExecuteViewEvent(View, "OnDidMoveToSuperview");
		}

		/// <summary>
		/// 기존의 하위 뷰가 현재 뷰에 추가된 직후 호출됨.
		/// </summary>
		internal void OnDidAddSubview(IUIView subview)
		{
			UIHelper.ExecuteViewEvent(View, "OnDidAddSubview", subview);
		}

		/// <summary>
		/// 기존의 하위 뷰가 현재 뷰에서 제거되기 직전 호출됨.
		/// </summary>
		internal void OnWillRemoveSubview(IUIView subview)
		{
			UIHelper.ExecuteViewEvent(View, "OnWillRemoveSubview", subview);
		}

		/// <summary>
		/// 현재 모든 하위 뷰를 수집.
		/// </summary>
		public void CollectAllSubviews()
		{
		}

		/// <summary>
		/// 현재 뷰의 맨 마지막 위치에 하위 뷰 추가.
		/// </summary>
		public void AddSubview(IUIView subview)
		{
			// 계층적으로 꼬이는 상황.
			if (!UIHelper.IsAvailableAddChild(View, subview))
				return;

			// 이미 현재 뷰의 하위 뷰로 존재하는 상황.
			if (Subviews.Contains(subview))
				return;

			// 기존 상위 뷰 제거.
			UIViewAdapter.StartRemoveFromSuperview(subview, OnRemovedEvent);

			// 새로운 상위 뷰 등록.
			UIViewAdapter.StartAddToSuperview(subview, View, OnAddCollection, OnAddChildTransform);

			static void OnRemovedEvent(UIViewAdapter subview, UIViewAdapter removedSuperview, IUIWindow removedWindow)
			{
				//var hasSuperviewDetachEvent = removedSuperview != null;
				//var hasWindowDetachEvent = removedWindow != null;
			}

			static void OnAddCollection(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				superviewAdapter.Subviews.Add(subviewAdapter.View);
			}

			static void OnAddChildTransform(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				subviewAdapter.RectTransform.SetParent(superviewAdapter.RectTransform, false);
			}
		}

		/// <summary>
		/// 현재 뷰의 대상 위치에 하위 뷰 추가.
		/// </summary>
		public void InsertSubview(IUIView subview, int index)
		{
			// 계층적으로 꼬이는 상황.
			if (!UIHelper.IsAvailableAddChild(View, subview))
				return;

			// 이미 현재 뷰의 하위 뷰로 존재하는 상황.
			if (Subviews.Contains(subview))
				return;

			// 기존 상위 뷰 제거.
			UIViewAdapter.StartRemoveFromSuperview(subview, OnRemovedEvent);

			// 새로운 상위 뷰 등록.
			var isLastSibling = false;
			var siblingIndex = index;
			UIViewAdapter.StartAddToSuperview(subview, View, OnAddCollection, OnAddTransform);

			static void OnRemovedEvent(UIViewAdapter subview, UIViewAdapter removedSuperview, IUIWindow removedWindow)
			{
				//var hasSuperviewDetachEvent = removedSuperview != null;
				//var hasWindowDetachEvent = removedWindow != null;
			}

			void OnAddCollection(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				if (siblingIndex >= 0 && siblingIndex < superviewAdapter.Subviews.Count)
				{
					superviewAdapter.Subviews.Insert(siblingIndex, subview);
				}
				else
				{
					isLastSibling = true;
					superviewAdapter.Subviews.Add(subview);
				}
			}

			void OnAddTransform(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				subviewAdapter.RectTransform.SetParent(superviewAdapter.RectTransform, false);
				if (!isLastSibling)
					subviewAdapter.RectTransform.SetSiblingIndex(siblingIndex);
			}
		}

		/// <summary>
		/// 현재 뷰의 대상 하위 뷰의 이전 위치에 하위 뷰 삽입.
		/// </summary>
		public void InsertSubviewAboveSibling(IUIView subview, IUIView sibling)
		{
			// 계층적으로 꼬이는 상황.
			if (!UIHelper.IsAvailableAddChild(View, subview))
				return;

			// 이미 현재 뷰의 하위 뷰로 존재하는 상황.
			if (Subviews.Contains(subview))
				return;

			// 기존 상위 뷰 제거.
			UIViewAdapter.StartRemoveFromSuperview(subview, OnRemovedEvent);

			// 새로운 상위 뷰 등록.
			var isLastSibling = false;
			var siblingIndex = Subviews.Count - 1;
			UIViewAdapter.StartAddToSuperview(subview, View, OnAddCollection, OnAddTransform);

			static void OnRemovedEvent(UIViewAdapter subview, UIViewAdapter removedSuperview, IUIWindow removedWindow)
			{
				//var hasSuperviewDetachEvent = removedSuperview != null;
				//var hasWindowDetachEvent = removedWindow != null;
			}

			void OnAddCollection(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				if (sibling != null && superviewAdapter.Subviews.Contains(sibling))
				{
					siblingIndex = superviewAdapter.Subviews.IndexOf(sibling);
					if (siblingIndex + 1 < superviewAdapter.Subviews.Count)
					{
						superviewAdapter.Subviews.Insert(siblingIndex + 1, subview);
					}
					else
					{
						isLastSibling = true;
						superviewAdapter.Subviews.Add(subview);
					}
				}
				else
				{
					isLastSibling = true;
					superviewAdapter.Subviews.Add(subview);
				}
			}

			void OnAddTransform(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				subviewAdapter.RectTransform.SetParent(RectTransform, false);
				if (!isLastSibling)
					subviewAdapter.RectTransform.SetSiblingIndex(siblingIndex);
			}
		}

		/// <summary>
		/// 현재 뷰의 대상 하위 뷰의 다음 위치에 하위 뷰 삽입.
		/// </summary>
		public void InsertSubviewBelowSibling(IUIView subview, IUIView sibling)
		{
			// 계층적으로 꼬이는 상황.
			if (!UIHelper.IsAvailableAddChild(View, subview))
				return;

			// 이미 현재 뷰의 하위 뷰로 존재하는 상황.
			if (Subviews.Contains(subview))
				return;

			// 기존 상위 뷰 제거.
			UIViewAdapter.StartRemoveFromSuperview(subview, OnRemovedEvent);

			// 새로운 상위 뷰 등록.
			var isLastSibling = false;
			var siblingIndex = Subviews.Count - 1;
			UIViewAdapter.StartAddToSuperview(subview, View, OnAddCollection, OnAddTransform);

			static void OnRemovedEvent(UIViewAdapter subview, UIViewAdapter removedSuperview, IUIWindow removedWindow)
			{
				//var hasSuperviewDetachEvent = removedSuperview != null;
				//var hasWindowDetachEvent = removedWindow != null;
			}

			void OnAddCollection(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				if (sibling != default && superviewAdapter.Subviews.Contains(sibling))
				{
					siblingIndex = superviewAdapter.Subviews.IndexOf(sibling);
					superviewAdapter.Subviews.Insert(siblingIndex, subview);
				}
				else
				{
					isLastSibling = true;
					superviewAdapter.Subviews.Add(subview);
				}
			}

			void OnAddTransform(UIViewAdapter superviewAdapter, UIViewAdapter subviewAdapter)
			{
				subviewAdapter.RectTransform.SetParent(RectTransform, false);
				if (!isLastSibling)
					subviewAdapter.RectTransform.SetSiblingIndex(siblingIndex);
			}
		}

		/// <summary>
		/// 대상 하위 뷰를 가장 나중에 그리게 함. (맨 위)
		/// </summary>
		public void BringSubviewToFront(IUIView subview)
		{
			if (subview == null)
				return;

			if (!Subviews.Contains(subview))
				return;

			Subviews.Remove(subview);
			Subviews.Add(subview);

			var subviewAdapter = UIHelper.GetViewAdapter(subview);
			subviewAdapter.RectTransform.SetAsLastSibling();
		}

		/// <summary>
		/// 대상 하위 뷰를 가장 먼저 그리게 함. (맨 아래)
		/// </summary>
		public void SendSubviewToBack(IUIView subview)
		{
			if (subview == null)
				return;

			if (!Subviews.Contains(subview))
				return;

			Subviews.Remove(subview);
			Subviews.Insert(0, subview);

			var subviewAdapter = UIHelper.GetViewAdapter(subview);
			subviewAdapter.RectTransform.SetAsFirstSibling();
		}

		/// <summary>
		/// 현재 뷰를 상위 뷰에서 제거.
		/// </summary>
		public void RemoveFromSuperview()
		{
			if (Superview == null)
				return;

			UIViewAdapter.StartRemoveFromSuperview(View);
		}

		/// <summary>
		/// 대상 하위 뷰를 상위 뷰에게 추가. (내부용)
		/// </summary>
		private static bool StartAddToSuperview(IUIView subview, IUIView superview, OnAddContainerEvent onAddContainer, OnAddTransformEvent onAddTransform)
		{
			if (subview == null)
				throw new ArgumentNullException(nameof(subview));
			if (superview == null)
				throw new ArgumentNullException(nameof(superview));
			if (onAddContainer == null)
				throw new ArgumentNullException(nameof(onAddContainer));
			if (onAddTransform == null)
				throw new ArgumentNullException(nameof(onAddTransform));

			var subviewAdapter = UIHelper.GetViewAdapter(subview);
			var superviewAdapter = UIHelper.GetViewAdapter(superview);
			var superviewWindow = superviewAdapter.Window;
			var hasWindowAttachEvent = superviewWindow != null;

			// 하위 뷰: 상위 뷰 변경 시작됨.
			subviewAdapter.OnWillMoveToSuperview(superviewAdapter.View);

			// 하위 뷰: 윈도우 변경 시작됨.
			if (hasWindowAttachEvent)
				subviewAdapter.OnWillMoveToWindow(superviewAdapter.Window);

			// 처리: 상위 뷰에 하위 뷰 추가.
			onAddContainer?.Invoke(superviewAdapter, subviewAdapter);

			// 처리: 유니티 트랜스폼 계층 구조 설정.
			onAddTransform?.Invoke(superviewAdapter, subviewAdapter);

			// 처리: 하위 뷰의 상위 뷰 설정.
			subviewAdapter.Superview = superviewAdapter.View;

			// 처리: 하위 뷰의 윈도우 설정.
			if (hasWindowAttachEvent)
				subviewAdapter.Window = superviewAdapter.Window;

			// 상위 뷰: 하위 뷰 추가됨.
			superviewAdapter.OnDidAddSubview(subview);

			// 하위 뷰: 상위 뷰 변경 완료됨.
			subviewAdapter.OnDidMoveToSuperview();

			// 하위 뷰: 윈도우 변경 완료됨.
			if (hasWindowAttachEvent)
				subviewAdapter.OnDidMoveToWindow();

			return true;
		}

		/// <summary>
		/// 대상 하위 뷰를 상위 뷰에게서 제거. (내부용)
		/// </summary>
		private static bool StartRemoveFromSuperview(IUIView subview, OnRemovedEvent onRemoved = null)
		{
			if (subview == null)
				throw new ArgumentNullException(nameof(subview));

			var subviewAdapter = UIHelper.GetViewAdapter(subview);
			var subviewSuperview = subview.Superview;
			var subviewWindow = subviewAdapter.Window;
			var superviewAdapter = UIHelper.GetViewAdapter(subviewSuperview);
			var hasSuperviewDetachEvent = subviewSuperview != null;
			var hasWindowDetachEvent = subviewWindow != null;

			// 아무런 이벤트도 발생할 수 없는 상태.
			if (!hasSuperviewDetachEvent && !hasWindowDetachEvent)
			{
				onRemoved?.Invoke(subviewAdapter, null, null);
				return false;
			}

			// 하위 뷰: 상위 뷰 변경 시작됨.
			if (hasSuperviewDetachEvent)
				subviewAdapter.OnWillMoveToSuperview(null);

			// 상위 뷰: 하위 뷰 제거됨.
			if (hasSuperviewDetachEvent)
				superviewAdapter.OnWillRemoveSubview(subview);

			// 하위 뷰: 윈도우 변경 시작됨.
			if (hasWindowDetachEvent)
				subviewAdapter.OnWillMoveToWindow(null);

			// 처리: 상위 뷰에서 하위 뷰 제거.
			if (hasSuperviewDetachEvent)
				superviewAdapter.Subviews.Remove(subviewAdapter.View);

			// 처리: 하위 뷰의 상위 뷰 제거.
			if (hasSuperviewDetachEvent)
				subviewAdapter.Superview = null;

			// 처리: 유니티 트랜스폼 계층 구조 제거.
			if (hasSuperviewDetachEvent)
				subviewAdapter.RectTransform.SetParent(null, false);

			// 처리: 하위 뷰에서 윈도우 제거.
			if (hasWindowDetachEvent)
				subviewAdapter.Window = null;

			// 하위 뷰: 상위 뷰 변경 완료됨.
			if (hasSuperviewDetachEvent)
				subviewAdapter.OnDidMoveToSuperview();

			// 하위 뷰: 윈도우 변경 완료됨.
			if (hasWindowDetachEvent)
				subviewAdapter.OnDidMoveToWindow();

			onRemoved?.Invoke(subviewAdapter, superviewAdapter, subviewWindow);
			return true;
		}
	}
}