using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 처리기.
	/// </summary>
	[ExecuteAlways]
	internal class UnityRunner : SharedComponent<UnityRunner>
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void Awake()
		{
			base.Awake();

			if (IsDestroyed())
				return;

			gameObject.hideFlags = HideFlags.HideAndDontSave;
			UnityMainThread.Initialize();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}
	}
}