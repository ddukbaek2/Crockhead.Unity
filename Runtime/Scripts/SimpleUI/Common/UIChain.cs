using Crockhead.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 체인.
	/// </summary>
	public class UIChain : Disposable
	{
		/// <summary>
		/// 연결 리스트.
		/// </summary>
		private LinkedList<UIController> m_Controllers;

		/// <summary>
		/// 여는 중인지 여부.
		/// </summary>
		private bool m_IsOpening;

		/// <summary>
		/// 닫는 중인지 여부.
		/// </summary>
		private bool m_IsClosing;

		/// <summary>
		/// 열거나 닫고 있는 중인지 여부 프로퍼티.
		/// </summary>
		public bool IsTransitioning => m_IsOpening || m_IsClosing;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIChain() : base()
		{
			m_Controllers = new LinkedList<UIController>();
			m_IsOpening = false;
			m_IsClosing = false;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 열기.
		/// </summary>
		public void Open(UIController controller)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			if (IsTransitioning)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Opening or Closing.");
				return;
			}

			if (m_Controllers.Contains(controller))
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Contains Chain.");
				return;
			}

			m_IsOpening = true;

			// 퇴장.
			var last = m_Controllers.Last;
			if (last != null)
			{
				last.Value.ViewWillDisappear();
				last.Value.ViewDidDisappear();
			}

			// 등장.
			last = m_Controllers.AddLast(controller);
			last.Value.ViewWillAppear();
			last.Value.ViewDidAppear();
			m_IsOpening = false;
		}

		/// <summary>
		/// 닫기.
		/// </summary>
		public void Close(UIController controller)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			if (IsTransitioning)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Opening or Closing.");
				return;
			}

			if (!m_Controllers.Contains(controller))
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Not Contains Chain.");
				return;
			}

			m_IsClosing = true;

			var destination = GetOpeningNode(controller);
			if (destination == null)
				destination = m_Controllers.Find(controller);

			// 순회.
			for (var current = m_Controllers.Last; current != destination; current = current.Previous)
			{
				if (current == null)
					break;

				// 퇴장.
				current.Value.ViewWillDisappear();
				current.Value.ViewDidDisappear();
				m_Controllers.Remove(current);
			}

			// 등장.
			var last = m_Controllers.Last;
			if (last != null)
			{
				last.Value.ViewWillAppear();
				last.Value.ViewDidAppear();
			}

			m_IsClosing = false;
		}

		/// <summary>
		/// 체인에 포함되어있는지 여부.
		/// </summary>
		public bool Contains(UIController controller)
		{
			return m_Controllers.Contains(controller);
		}

		/// <summary>
		/// 현재 노드가 연 노드.
		/// </summary>
		internal LinkedListNode<UIController> GetOpenedNode(UIController controller)
		{
			var node = m_Controllers.Find(controller);
			return node?.Next ?? null;
		}

		/// <summary>
		/// 현재 노드를 연 노드.
		/// </summary>
		internal LinkedListNode<UIController> GetOpeningNode(UIController controller)
		{
			var node = m_Controllers.Find(controller);
			return node?.Previous ?? null;
		}

		/// <summary>
		/// 현재 컨트롤러가 연 컨트롤러.
		/// </summary>
		public UIController GetOpenedController(UIController controller)
		{
			var node = GetOpenedNode(controller);
			return node?.Value ?? null;
		}

		/// <summary>
		/// 현재 컨트롤러를 연 컨트롤러.
		/// </summary>
		public UIController GetOpeningController(UIController controller)
		{
			var node = GetOpeningNode(controller);
			return node?.Value ?? null;
		}
	}
}