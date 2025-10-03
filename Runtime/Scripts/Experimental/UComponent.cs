using Crockhead.Core;
using UnityEngine;
using IDotNetDisposable = System.IDisposable;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 컴포넌트.
	/// <para>IDotNetDisposable 인터페이스 구현체.</para>
	/// <para>IDisposable 인터페이스 구현체.</para>
	/// </summary>
	public abstract class UComponent : MonoBehaviour, IDisposable
	{
		/// <summary>
		/// 해제 되었는지 여부.
		/// </summary>
		private bool m_IsDisposed;

		/// <summary>
		/// 해제 되었는지 여부 프로퍼티.
		/// <para>IDisposable 인터페이스 구현.</para>
		/// </summary>
		bool IDisposable.IsDisposed => m_IsDisposed;

		/// <summary>
		/// 생성됨.
		/// </summary>
		internal virtual void Awake()
		{
			m_IsDisposed = false;

			OnCreate();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		internal virtual void Start()
		{
			OnInitialize();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		internal virtual void OnDestroy()
		{
			if (m_IsDisposed)
				return;

			m_IsDisposed = true;
			OnDispose(false);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected abstract void OnCreate();

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected abstract void OnInitialize();

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected abstract void OnDispose(bool explicitDisposing);

		/// <summary>
		/// 해제.
		/// <para>IDotNetDisposable 인터페이스 구현.</para>
		/// </summary>
		void IDotNetDisposable.Dispose()
		{
			if (this == null)
				return;

			if (m_IsDisposed)
				return;

			m_IsDisposed = true;
			OnDispose(true);
			GameObject.Destroy(gameObject);
		}
	}
}