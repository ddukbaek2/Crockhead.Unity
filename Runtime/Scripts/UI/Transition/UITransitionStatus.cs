namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 트랜지션 상태.
	/// </summary>
	public enum UITransitionStatus
	{
		/// <summary>
		/// 없음.
		/// </summary>
		None = 0,

		/// <summary>
		/// 등장 진행 중.
		/// </summary>
		Appearing,

		/// <summary>
		/// 등장 완료.
		/// </summary>
		Appeared,

		/// <summary>
		/// 퇴장 진행 중.
		/// </summary>
		Disappearing,

		/// <summary>
		/// 퇴장 완료.
		/// </summary>
		Disappeared,
	}
}