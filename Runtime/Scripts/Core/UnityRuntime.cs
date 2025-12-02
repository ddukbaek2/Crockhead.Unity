using Crockhead.Core;
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
		protected override void OnCreate()
		{
			base.OnCreate();

			gameObject.hideFlags = HideFlags.HideAndDontSave;
			UnityThreadDispatcher.Create();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose()
		{
			UnityThreadDispatcher.Destroy();

			base.OnDispose();
		}
	}
}