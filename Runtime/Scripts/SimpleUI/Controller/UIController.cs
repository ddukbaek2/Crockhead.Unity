using Crockhead.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 반복자.
	/// </summary>
	public class UIController : Disposable
	{
		/// <summary>
		/// 뷰.
		/// </summary>
		private UIView m_View;

		/// <summary>
		/// 체인.
		/// </summary>
		private UIChain m_Chain;

		/// <summary>
		/// 뷰가 로드 되었는지 여부 프로퍼티.
		/// </summary>
		public bool IsViewLoaded => m_View != null;

		/// <summary>
		/// 뷰 프로퍼티.
		/// </summary>
		public UIView View
		{
			get
			{
				if (IsViewLoaded)
					return ViewIfLoaded;

				m_View = OnViewWillLoad();
				OnViewDidLoad();
				return m_View;
			}
		}

		/// <summary>
		/// 로드 된 뷰 프로퍼티.
		/// </summary>
		public UIView ViewIfLoaded => m_View;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIController() : base()
		{
			m_View = null;
			m_Chain = null;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 뷰 로드 시작됨.
		/// </summary>
		protected virtual UIView OnViewWillLoad()
		{
			if (IsViewLoaded)
				return m_View;

			var type = GetType();
			var view = UIHelper.CreateView(type);
			view.gameObject.SetActive(false);
			return view;
		}

		/// <summary>
		/// 뷰 로드 완료됨.
		/// </summary>
		protected virtual void OnViewDidLoad()
		{
		}

		/// <summary>
		/// 뷰 등장 시작됨.
		/// </summary>
		protected virtual void OnViewWillAppear()
		{
		}

		/// <summary>
		/// 뷰 등장 완료됨.
		/// </summary>
		protected virtual void OnViewDidAppear()
		{
		}

		/// <summary>
		/// 뷰 퇴장 시작됨.
		/// </summary>
		protected virtual void OnViewWillDisappear()
		{
		}

		/// <summary>
		/// 뷰 퇴장 완료됨.
		/// </summary>
		protected virtual void OnViewDidDisappear()
		{
		}

		/// <summary>
		/// 열기.
		/// <para>체인에서 가장 뒤에 추가됨.</para>
		/// </summary>
		public void Open(UIController controller)
		{
			if (controller == null)
				throw new ArgumentNullException(nameof(controller));

			try
			{
				controller.m_Chain = m_Chain;
				controller.m_Chain.Open(controller);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 닫기.
		/// <para>가장 나중에 열린 객체부터 현재 객체까지 모든 열린 객체는 역순으로 닫힘.</para>
		/// </summary>
		public void Close()
		{
			if (m_Chain == null)
				return;

			try
			{
				m_Chain.Close(this);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 체인 설정. (내부용)
		/// </summary>
		internal void SetChain(UIChain chain)
		{
			m_Chain = chain;
		}

		/// <summary>
		/// 뷰 등장 시작됨. (내부용)
		/// </summary>
		internal void ViewWillAppear()
		{
			OnViewWillAppear();
		}

		/// <summary>
		/// 뷰 등장 완료됨. (내부용)
		/// </summary>
		internal void ViewDidAppear()
		{
			OnViewDidAppear();
		}

		/// <summary>
		/// 뷰 퇴장 시작됨. (내부용)
		/// </summary>
		internal void ViewWillDisappear()
		{
			OnViewWillDisappear();
		}


		/// <summary>
		/// 뷰 퇴장 완료됨. (내부용)
		/// </summary>
		internal void ViewDidDisappear()
		{
			OnViewDidDisappear();
		}
	}
}