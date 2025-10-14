using Crockhead.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 컨트롤러 발표 조정자.
	/// </summary>
	public class UIPresentationCoordinator : Disposable
	{
		/// <summary>
		/// 연결 리스트.
		/// </summary>
		private LinkedList<UIController> m_PresentationControllers;

		/// <summary>
		/// 여는 중인지 여부.
		/// </summary>
		private bool m_IsBeingPresent;

		/// <summary>
		/// 닫는 중인지 여부.
		/// </summary>
		private bool m_IsBeingRetract;

		/// <summary>
		/// 현재 조정자가 컨트롤러를 열거나 닫고 있는 중인지 여부 프로퍼티.
		/// </summary>
		public bool IsTransitioning => m_IsBeingPresent || m_IsBeingRetract;

		/// <summary>
		/// 가장 나중에 추가된 컨트롤러.
		/// </summary>
		public UIController Top => m_PresentationControllers.Last?.Value ?? null;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIPresentationCoordinator() : base()
		{
			m_PresentationControllers = new LinkedList<UIController>();
			m_IsBeingPresent = false;
			m_IsBeingRetract = false;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 발표.
		/// </summary>
		public void Present(UIController controller, bool animated)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			if (IsTransitioning)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Appearing or Disappearing.");
				return;
			}

			if (m_PresentationControllers.Contains(controller))
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler in Chain.");
				return;
			}

			m_IsBeingPresent = true;

			try
			{
				// 퇴장.
				var last = m_PresentationControllers.Last;
				if (last != null)
				{
					last.Value.BeginAppearanceTransition(false, animated);
					last.Value.EndAppearanceTransition();
				}

				// 등장.
				last = m_PresentationControllers.AddLast(controller);
				last.Value.BeginAppearanceTransition(true, animated);
				last.Value.EndAppearanceTransition();
			}
			finally
			{
				m_IsBeingPresent = false;
			}
		}

		/// <summary>
		/// 발표 철회.
		/// </summary>
		public void Retract(UIController controller, bool animated)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			if (IsTransitioning)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Appearing or Disappearing.");
				return;
			}

			if (!m_PresentationControllers.Contains(controller))
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Not in Chain.");
				return;
			}

			if (m_PresentationControllers.Count == 0)
			{
				Debug.LogError("[Crockhead.Unity.UI] Chain is Empty.");
				return;
			}

			if (m_PresentationControllers.First.Value == controller)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Chain First Node.");
				return;
			}

			m_IsBeingRetract = true;

			try
			{
				var destination = GetOpeningNode(controller);
				// 순회.
				for (var current = m_PresentationControllers.Last; current != destination; current = current.Previous)
				{
					// 퇴장.
					current.Value.BeginAppearanceTransition(false, animated);
					current.Value.EndAppearanceTransition();
					m_PresentationControllers.Remove(current);
				}

				// 등장.
				var last = m_PresentationControllers.Last;
				if (last != null)
				{
					last.Value.BeginAppearanceTransition(true, animated);
					last.Value.EndAppearanceTransition();
				}
			}
			finally
			{
				m_IsBeingRetract = false;
			}
		}

		/// <summary>
		/// 체인에 포함되어있는지 여부.
		/// </summary>
		public bool Contains(UIController controller)
		{
			return m_PresentationControllers.Contains(controller);
		}

		/// <summary>
		/// 현재 노드가 연 노드.
		/// </summary>
		internal LinkedListNode<UIController> GetOpenedNode(UIController controller)
		{
			var node = m_PresentationControllers.Find(controller);
			return node?.Next ?? null;
		}

		/// <summary>
		/// 현재 노드를 연 노드.
		/// </summary>
		internal LinkedListNode<UIController> GetOpeningNode(UIController controller)
		{
			var node = m_PresentationControllers.Find(controller);
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