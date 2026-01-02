using Crockhead.Core;
using System;
using System.Collections.Generic;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 네비게이션 컨트롤러.
	/// </summary>
	public class UINavigationController : UIController
	{
		public struct ViewConfiguration
		{
			public Type ViewType { set; get; }
			public string AssetPath { set; get; }
			public AssetPathType AssetPathType { set; get; }
		}

		/// <summary>
		/// 컨트롤러 목록.
		/// </summary>
		private Stack<UIController> m_Controllers;

		/// <summary>
		/// 스택 갯수 프로퍼티.
		/// </summary>
		public int Count => m_Controllers.Count;

		/// <summary>
		/// 컨트롤러 목록 프로퍼티.
		/// </summary>
		public IEnumerable<UIController> Controllers => m_Controllers;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UINavigationController() : base()
		{
			m_Controllers = new Stack<UIController>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 뷰 로드 시작됨.
		/// </summary>
		protected override UIViewLoadConfiguration OnViewWillLoad(Type viewType)
		{
			var viewLoadConfiguration = base.OnViewWillLoad(viewType);
			return viewLoadConfiguration;
		}

		/// <summary>
		/// 뷰 등장 완료됨.
		/// </summary>
		protected override void OnViewDidAppear()
		{
			base.OnViewDidAppear();
		}

		/// <summary>
		/// 뷰 퇴장 완료됨.
		/// </summary>
		protected override void OnViewDidDisappear()
		{
			base.OnViewDidDisappear();
		}

		/// <summary>
		/// 새로운 뷰를 맨 위에 표시되도록 목록에 추가.
		/// </summary>
		public void Push(UIController controller)
		{
			if (controller == null)
				return;

			if (m_Controllers.Contains(controller))
				return;

			m_Controllers.Push(controller);
		}

		/// <summary>
		/// 맨 위의 뷰를 더이상 표시하지 않도록 목록에서 제거.
		/// </summary>
		public UIController Pop()
		{
			if (m_Controllers.Count < 2)
				return default;

			var controller = m_Controllers.Pop();
			return controller;
		}
	}
}