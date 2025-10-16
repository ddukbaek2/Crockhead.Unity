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
		/// 프레젠테이션 상태.
		/// </summary>
		public enum UIPresentationState
		{
			/// <summary>
			/// 없음.
			/// </summary>
			None,

			/// <summary>
			/// 표시 진행 중.
			/// </summary>
			Presenting,

			/// <summary>
			/// 표시 완료.
			/// </summary>
			Presented,

			/// <summary>
			/// 표시 중단 진행 중.
			/// </summary>
			Retracting,

			/// <summary>
			/// 표시 중단 완료.
			/// </summary>
			Retracted,
		}


		/// <summary>
		/// 연결 리스트.
		/// </summary>
		private LinkedList<UIController> m_Controllers;

		/// <summary>
		/// 프레젠테이션 상태.
		/// </summary>
		private UIPresentationState m_State;

		/// <summary>
		/// 트랜지션 처리기.
		/// </summary>
		private UITransitionCoordinator m_TransitionCoordinator;

		/// <summary>
		/// 현재 조정자가 컨트롤러를 처리 중인지 여부 프로퍼티.
		/// </summary>
		public bool IsPresentingOrRetracting => m_State == UIPresentationState.Presenting || m_State == UIPresentationState.Retracting;

		/// <summary>
		/// 현재 조정자가 컨트롤러를 처리 중인지 여부 프로퍼티. (짧은 버전)
		/// </summary>
		public bool IsBusy => IsPresentingOrRetracting;

		/// <summary>
		/// 가장 나중에 추가된 컨트롤러. (Last)
		/// </summary>
		public UIController Top => m_Controllers.Last?.Value ?? null;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIPresentationCoordinator() : base()
		{
			m_Controllers = new LinkedList<UIController>();
			m_State = UIPresentationState.None;
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
		public async Task Present(UIController controller, bool animated)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			if (IsPresentingOrRetracting)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Appearing or Disappearing.");
				return;
			}

			if (Contains(controller))
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler in Chain.");
				return;
			}

			m_State = UIPresentationState.Presenting;
			try
			{
				// 퇴장.
				//m_Controllers.Last?.Value?.BeginAppearanceTransition(false, animated);
				//m_Controllers.Last?.Value?.EndAppearanceTransition();

				// 등장.
				//controller.BeginAppearanceTransition(true, animated);
				//controller.EndAppearanceTransition();

				// 트랜지션 처리.
				var from = m_Controllers.Last?.Value ?? null;
				var to = controller;
				await m_TransitionCoordinator.TransitAsync(from, to, animated);

				// 추가.
				m_Controllers.AddLast(to);
			}
			finally
			{
				m_State = UIPresentationState.Presented;
			}
		}

		/// <summary>
		/// 표시 중단. (철회)
		/// <para>가장 나중에 열린 객체부터 현재 객체까지 모든 열린 객체는 역순으로 닫힘.</para>
		/// <para>First 객체는 표시를 중단 할 수 없음.</para>
		/// </summary>
		public async Task RetractAsync(UIController controller, bool animated)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			// 현재 처리 중인 경우에는 처리 할 수 없음.
			if (IsPresentingOrRetracting)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Appearing or Disappearing.");
				return;
			}

			// 체인에 포함 되어있지 않은 객체는 처리 할 수 없음.
			if (!Contains(controller))
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Not in Chain.");
				return;
			}

			// 최소 2개는 있어야 처리 할 수 있음.
			if (m_Controllers.Count < 2)
			{
				Debug.LogError("[Crockhead.Unity.UI] Chain is Empty.");
				return;
			}

			// 시작 객체는 처리 불가능.
			if (m_Controllers.First.Value == controller)
			{
				Debug.LogError("[Crockhead.Unity.UI] This Controler is Chain First Node.");
				return;
			}

			m_State = UIPresentationState.Retracting;
			try
			{
				// 현재 대상을 표시한 객체.
				var presenting = GetPresenting(controller);

				// 순회.
				for (var current = m_Controllers.Last; current != presenting; current = current.Previous)
				{
					//// 퇴장.
					//current.Value.BeginAppearanceTransition(false, animated);
					//current.Value.EndAppearanceTransition();
					await m_TransitionCoordinator.TransitAsync(null, current.Value, animated);
					m_Controllers.Remove(current);
				}

				// 등장.
				//presenting.Value.BeginAppearanceTransition(true, animated);
				//presenting.Value.EndAppearanceTransition();

				// 트랜지션 처리.
				var from = default(UIController);
				var to = presenting.Value;
				await m_TransitionCoordinator.TransitAsync(from, to, animated);

			}
			finally
			{
				m_State = UIPresentationState.Retracted;
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
		/// 현재 노드가 연 노드.
		/// </summary>
		private LinkedListNode<UIController> GetPresented(UIController controller)
		{
			var node = m_Controllers.Find(controller);
			return node?.Next ?? null;
		}

		/// <summary>
		/// 현재 노드를 연 노드.
		/// </summary>
		private LinkedListNode<UIController> GetPresenting(UIController controller)
		{
			var node = m_Controllers.Find(controller);
			return node?.Previous ?? null;
		}

		/// <summary>
		/// 현재 컨트롤러가 연 컨트롤러.
		/// </summary>
		public UIController GetPresentedController(UIController controller)
		{
			var node = GetPresented(controller);
			return node?.Value ?? null;
		}

		/// <summary>
		/// 현재 컨트롤러를 연 컨트롤러.
		/// </summary>
		public UIController GetPresentingController(UIController controller)
		{
			var node = GetPresenting(controller);
			return node?.Value ?? null;
		}
	}
}