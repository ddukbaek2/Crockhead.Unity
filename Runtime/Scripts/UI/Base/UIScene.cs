using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// UI 씬.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class UIScene : UIBehaviour
	{
		/// <summary>
		/// 활성화 상태.
		/// </summary>
		public enum UIActivationState
		{
			/// <summary>
			/// 화면이 표시되고, 터치 입력이 가능하며, 알림 등이 가리지 않는 상태.
			/// </summary>
			ForegroundActive,

			/// <summary>
			/// 화면이 표시되지만, 입력이 막히거나, 알림 등이 가리는 상태.
			/// </summary>
			ForegroundInactive,

			/// <summary>
			/// 화면에 표시되지 않는 상태.
			/// </summary>
			Background,

			/// <summary>
			/// 연결되지 않은 상태.
			/// </summary>
			Unattached,
		}


		/// <summary>
		/// 렉트 트랜스폼.
		/// </summary>
		private RectTransform m_RectTransform;

		/// <summary>
		/// 윈도우 목록.
		/// </summary>
		private List<UIWindow> m_Windows;

		/// <summary>
		/// 렉트 트랜스폼 프로퍼티.
		/// </summary>
		public RectTransform RectTransform => m_RectTransform;

		/// <summary>
		/// 윈도우 목록 프로퍼티.
		/// </summary>
		public IEnumerable<UIWindow> Windows => m_Windows;

		/// <summary>
		/// 활성화 상태 프로퍼티.
		/// </summary>
		public UIActivationState ActivationState
		{
			get
			{
				if (!UIApplication.SharedInstance.IsConnectedScene(this))
					return UIActivationState.Unattached;
				return UIActivationState.ForegroundActive;
			}
		}


		/// <summary>
		/// 생성됨.
		/// </summary>
		protected sealed override void Awake()
		{
			m_Windows = new List<UIWindow>();
			m_RectTransform = GetComponent<RectTransform>();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected sealed override void OnDestroy()
		{
			foreach (var window in m_Windows)
			{
				if (window == null || window.IsDestroyed())
					continue;

				GameObject.Destroy(window.gameObject);
			}

			m_Windows.Clear();
		}

		/// <summary>
		/// 윈도우 추가.
		/// </summary>
		public void AddWindow(UIWindow window)
		{
			if (window == null)
				return;
			if (m_Windows.Contains(window))
				return;

			m_Windows.Add(window);
			window.RectTransform.SetParent(m_RectTransform);
		}

		/// <summary>
		/// 윈도우 제거.
		/// </summary>
		public void RemoveWindow(UIWindow window)
		{
			if (window == null)
				return;
			if (!m_Windows.Contains(window))
				return;

			m_Windows.Remove(window);
			window.RectTransform.SetParent(null);
		}
	}
}