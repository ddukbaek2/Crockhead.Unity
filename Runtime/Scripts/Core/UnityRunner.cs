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
		protected override void OnCreate()
		{
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			UnityMainThread.Initialize();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected override void OnInitialize()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		private void Update()
		{
		}
	}
}