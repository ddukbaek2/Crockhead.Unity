using Crockhead.Core;
using System.Collections.Generic;


namespace Crockhead.Unity.UIKitLite
{
	/// <summary>
	/// UIKitLite 애플리케이션.
	/// </summary>
	public class UIApplication : Disposable
	{
		/// <summary>
		/// 공유.
		/// </summary>
		public static UIApplication SharedInstance => SharedInstances.Get<UIApplication>();

		/// <summary>
		/// 연결된 씬 목록.
		/// </summary>
		private List<UIScene> m_ConnectedScenes;

		/// <summary>
		/// 작업 큐.
		/// </summary>
		private UIDispatchQueue m_DispatchQueue;

		/// <summary>
		/// 연결된 씬 목록 프로퍼티.
		/// </summary>
		public IEnumerable<UIScene> Scenes => m_ConnectedScenes;

		/// <summary>
		/// 활성화 된 씬 프로퍼티.
		/// </summary>
		public UIScene ForegroundActiveScene
		{
			get
			{
				foreach (var connectedScene in m_ConnectedScenes)
				{
					if (connectedScene == null)
						continue;

					return connectedScene;
				}

				return null;
			}
		}

		/// <summary>
		/// 작업 큐 프로퍼티.
		/// </summary>
		public UIDispatchQueue DispatchQueue => m_DispatchQueue;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIApplication() : base()
		{
			m_ConnectedScenes = new List<UIScene>();
			m_DispatchQueue = new UIDispatchQueue();

			// 등록.
			if (!SharedInstances.IsSet<UIApplication>())
			{
				SharedInstances.Set(this);
			}
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			foreach (var scene in m_ConnectedScenes)
			{
				Disposables.Dispose(scene);
			}

			m_ConnectedScenes.Clear();

			// 등록 해제.
			if (SharedInstances.TryGet<UIApplication>(out var application))
			{
				if (this == application)
				{
					SharedInstances.Unset<UIApplication>();
				}
			}
		}

		/// <summary>
		/// 씬 연결.
		/// </summary>
		public void ConnectScene(UIScene scene)
		{
			if (scene == null)
				return;
			if (m_ConnectedScenes.Contains(scene))
				return;

			m_ConnectedScenes.Add(scene);
		}

		/// <summary>
		/// 씬 연결 해제.
		/// </summary>
		public void DisconnectScene(UIScene scene)
		{
			if (scene == null)
				return;
			if (!m_ConnectedScenes.Contains(scene))
				return;

			m_ConnectedScenes.Remove(scene);
		}

		/// <summary>
		/// 모든 씬 연결 해제.
		/// </summary>
		public void DisconnectAllScenes()
		{
			foreach (var connectedScene in m_ConnectedScenes.ToArray())
			{
				if (connectedScene == null)
					continue;

				//
			}

			m_ConnectedScenes.Clear();
		}

		/// <summary>
		/// 씬이 연결되어있는지 여부.
		/// </summary>
		public bool IsConnectedScene(UIScene scene)
		{
			if (scene == null)
				return false;

			return m_ConnectedScenes.Contains(scene);
		}
	}
}