using Crockhead.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 컨트롤러 프레젠테이션 조정자.
	/// <para>동일 도메인에서 체인에 의해 유지됨.</para>
	/// </summary>
	public class UIPresentationCoordinator : Disposable
	{
		/// <summary>
		/// 연결 리스트.
		/// </summary>
		private LinkedList<UIController> m_Controllers;

		/// <summary>
		/// 프레젠테이션 상태.
		/// </summary>
		private UIPresentationStatus m_PresentationStatus;

		/// <summary>
		/// 트랜지션 처리기.
		/// </summary>
		private UITransitionCoordinator m_TransitionCoordinator;

		/// <summary>
		/// 현재 조정자가 컨트롤러를 처리 중인지 여부 프로퍼티.
		/// </summary>
		public bool IsPresentingOrDismissing => m_PresentationStatus == UIPresentationStatus.Presenting || m_PresentationStatus == UIPresentationStatus.Dismissing;

		/// <summary>
		/// 현재 조정자가 컨트롤러를 처리 중인지 여부 프로퍼티. (짧은 버전)
		/// </summary>
		public bool IsBusy => IsPresentingOrDismissing;

		/// <summary>
		/// 가장 처음에 추가된 컨트롤러 프로퍼티. (First)
		/// </summary>
		public UIController First => m_Controllers.First?.Value ?? null;

		/// <summary>
		/// 가장 나중에 추가된 컨트롤러 프로퍼티. (Last)
		/// </summary>
		public UIController Last => m_Controllers.Last?.Value ?? null;

		/// <summary>
		/// 컨트롤러 목록 프로퍼티.
		/// </summary>
		public IEnumerable<UIController> Controllers => m_Controllers;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIPresentationCoordinator() : base()
		{
			m_Controllers = new LinkedList<UIController>();
			m_PresentationStatus = UIPresentationStatus.None;
			m_TransitionCoordinator = new UITransitionCoordinator();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 표시.
		/// <para>프레젠테이션 체인에서 가장 뒤에 추가됨.</para>
		/// </summary>
		public async Task PresentAsync(UIController controller, bool animated)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			// 현재 처리 중인 경우에는 처리 할 수 없음.
			if (m_PresentationStatus == UIPresentationStatus.Presenting)
			{
				Debug.LogError($"[UIPresentationCoordinator] This controller is currently presenting.");
				return;
			}

			// 현재 처리 중인 경우에는 처리 할 수 없음.
			if (m_PresentationStatus == UIPresentationStatus.Dismissing)
			{
				Debug.LogError($"[UIPresentationCoordinator] This controller is currently dismissing.");
				return;
			}

			// 이미 체인에 포함 된 객체는 처리 할 수 없음.
			if (Contains(controller))
			{
				Debug.LogError("[UIPresentationCoordinator] The specified controller is already in the presentation stack.");
				return;
			}

			m_PresentationStatus = UIPresentationStatus.Presenting;
			try
			{
				// 트랜지션 처리.
				var from = m_Controllers.Last?.Value ?? null;
				var to = controller;

				// 프레젠테이션 체인에 추가.
				m_Controllers.AddLast(to);
				to.AddToPresentationStack();

				// 퇴장, 등장 시작.
				if (from != null) from.BeginAppearanceTransition(false, animated);
				to.BeginAppearanceTransition(true, animated);

				// 효과 처리.
				await m_TransitionCoordinator.TransitionAsync(from, to, animated);

				// 퇴장, 등장 완료.
				if (from != null) from.EndAppearanceTransition();
				to.EndAppearanceTransition();
			}
			finally
			{
				m_PresentationStatus = UIPresentationStatus.Appeared;
			}
		}

		/// <summary>
		/// 표시 중단. (철회)
		/// <para>가장 나중에 열린 객체부터 현재 객체까지 모든 열린 객체는 역순으로 닫힘.</para>
		/// <para>First 객체는 표시를 중단 할 수 없음.</para>
		/// </summary>
		public async Task DismissAsync(UIController controller, bool animated)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			// 현재 처리 중인 경우에는 처리 할 수 없음.
			if (m_PresentationStatus == UIPresentationStatus.Presenting)
			{
				Debug.LogError($"[UIPresentationCoordinator] This controller is currently presenting.");
				return;
			}

			// 현재 처리 중인 경우에는 처리 할 수 없음.
			if (m_PresentationStatus == UIPresentationStatus.Dismissing)
			{
				Debug.LogError($"[UIPresentationCoordinator] This controller is currently dismissing.");
				return;
			}

			// 최소 2개는 있어야 처리 할 수 있음.
			if (m_Controllers.Count < 2)
			{
				Debug.LogError("[UIPresentationCoordinator] The presentation stack must contain at least two controllers.");
				return;
			}

			// 시작 객체는 처리 불가능.
			if (m_Controllers.First.Value == controller)
			{
				Debug.LogError("[UIPresentationCoordinator] The specified controller is the root of the presentation stack.");
				return;
			}

			// 체인에 포함 되어있지 않은 객체는 처리 할 수 없음.
			if (!Contains(controller))
			{
				Debug.LogError("[UIPresentationCoordinator] The specified controller is not in the presentation stack.");
				return;
			}

			m_PresentationStatus = UIPresentationStatus.Dismissing;
			try
			{
				// 현재 대상을 표시한 객체.
				var presenting = GetPresenting(controller);
				var from = default(UIController);
				var to = default(UIController);

				// 마지막 요소가 아닐 경우는 현재 요소가 마지막 요소가 될 떄까지 마지막부터 역으로 돌아가며 제거.
				var current = m_Controllers.Last;
				while (current.Value != controller)
				{
					// 트랜지션 처리.
					from = current.Value;
					to = null;

					// 퇴장 시작.
					from.BeginAppearanceTransition(false, animated);

					// 효과 처리.
					await m_TransitionCoordinator.TransitionAsync(from, to, animated);

					// 퇴장 완료.
					from.EndAppearanceTransition();

					// 프레젠테이션 체인에서 제거.
					current = current.Previous;
					m_Controllers.Remove(current);
					to.RemoveFromPresentationStack();
				}

				// 트랜지션 처리.
				from = m_Controllers.Last.Value; // 인자로 받은 controller와 동일.
				to = presenting.Value;

				// 퇴장, 등장 시작.
				from.BeginAppearanceTransition(false, animated);
				to.BeginAppearanceTransition(true, animated);

				// 효과 처리.
				await m_TransitionCoordinator.TransitionAsync(from, to, animated);

				// 퇴장, 등장 완료.
				from.EndAppearanceTransition();
				to.EndAppearanceTransition();

				// 프레젠테이션 체인에서 제거.
				m_Controllers.Remove(from);
				from.RemoveFromPresentationStack();
			}
			finally
			{
				m_PresentationStatus = UIPresentationStatus.Dismissed;
			}
		}

		/// <summary>
		/// 체인에 포함되어있는지 여부.
		/// </summary>
		public bool Contains(UIController controller)
		{
			if (controller == null)
				return false;

			return m_Controllers.Contains(controller);
		}

		/// <summary>
		/// 현재 노드가 연 노드. (Next)
		/// </summary>
		internal LinkedListNode<UIController> GetPresented(UIController controller)
		{
			var node = m_Controllers.Find(controller);
			return node?.Next ?? null;
		}

		/// <summary>
		/// 현재 노드를 연 노드. (Previous)
		/// </summary>
		internal LinkedListNode<UIController> GetPresenting(UIController controller)
		{
			var node = m_Controllers.Find(controller);
			return node?.Previous ?? null;
		}

		/// <summary>
		/// 현재 컨트롤러가 연 컨트롤러. (Next)
		/// </summary>
		public UIController GetPresentedController(UIController controller)
		{
			var node = GetPresented(controller);
			return node?.Value ?? null;
		}

		/// <summary>
		/// 현재 컨트롤러를 연 컨트롤러. (Previous)
		/// </summary>
		public UIController GetPresentingController(UIController controller)
		{
			var node = GetPresenting(controller);
			return node?.Value ?? null;
		}
	}
}