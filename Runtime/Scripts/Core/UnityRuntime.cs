using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 실행시간 처리기.
	/// </summary>
	[ExecuteAlways]
	internal class UnityRuntime : SharedComponent<UnityRuntime>
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
			UnityThreadDispatcher.Create();
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