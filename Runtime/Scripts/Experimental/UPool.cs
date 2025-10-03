using UnityEngine;
using Crockhead.Experimental.Collections;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 풀. (게임오브젝트)
	/// </summary>
	public class UPool<TComponent> : Pool<TComponent> where TComponent : Behaviour
	{
		private MonoBehaviour m_Owner;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UPool(MonoBehaviour owner) : base()
		{
			m_Owner = owner;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 풀에 넣어짐.
		/// </summary>
		protected override void OnPushed(TComponent component)
		{
			base.OnPushed(component);

			component.gameObject.SetActive(true);
		}

		/// <summary>
		/// 풀에서 빼내짐.
		/// </summary>
		protected override void OnPopped(TComponent component)
		{
			component.gameObject.SetActive(false);

			base.OnPopped(component);
		}

		/// <summary>
		/// 자동생성됨.
		/// </summary>
		protected override TComponent OnGenerated()
		{
			var component = GameObjects.CreateGameObjectWithComponent<TComponent>();
			return component;
		}
	}
}