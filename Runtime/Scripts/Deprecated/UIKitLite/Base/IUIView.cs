using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UIKitLite.Deprecated
{
	/// <summary>
	/// 뷰 인터페이스.
	/// <para>UI의 시각적 단위 객체. (유니티에서는 UIKitLite 컴포넌트)</para>
	/// </summary>
	public interface IUIView
	{
		/// <summary>
		/// 렉트 트랜스폼 프로퍼티.
		/// </summary>
		RectTransform RectTransform { get; }

		/// <summary>
		/// 윈도우 프로퍼티.
		/// </summary>
		IUIWindow Window { get; }

		/// <summary>
		/// 상위 뷰 프로퍼티.
		/// </summary>
		IUIView Superview { get; }

		/// <summary>
		/// 하위 뷰 목록 프로퍼티.
		/// </summary>
		IEnumerable<IUIView> Subviews { get; }

		/// <summary>
		/// 현재 뷰의 맨 마지막 위치에 하위 뷰 추가.
		/// </summary>
		void AddSubview(IUIView view);

		/// <summary>
		/// 현재 뷰의 대상 위치에 하위 뷰 추가.
		/// </summary>
		void InsertSubview(IUIView view, int index);

		/// <summary>
		/// 현재 뷰의 대상 하위 뷰의 이전 위치에 하위 뷰 삽입.
		/// </summary>
		void InsertSubviewAboveSibling(IUIView view, IUIView sibling);

		/// <summary>
		/// 현재 뷰의 대상 하위 뷰의 다음 위치에 하위 뷰 삽입.
		/// </summary>
		void InsertSubviewBelowSibling(IUIView view, IUIView sibling);

		/// <summary>
		/// 대상 하위 뷰를 가장 나중에 그리게 함. (맨 위)
		/// </summary>
		void BringSubviewToFront(IUIView view);

		/// <summary>
		/// 대상 하위 뷰를 가장 먼저 그리게 함. (맨 아래)
		/// </summary>
		void SendSubviewToBack(IUIView view);

		/// <summary>
		/// 현재 뷰를 상위 뷰에서 제거.
		/// </summary>
		void RemoveFromSuperview();
	}
}