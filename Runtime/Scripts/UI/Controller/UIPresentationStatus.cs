namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 프레젠테이션 상태.
	/// </summary>
	public enum UIPresentationStatus
	{
		/// <summary>
		/// 없음.
		/// </summary>
		None = 0,

		/// <summary>
		/// 표시 진행 중.
		/// </summary>
		Presenting,

		/// <summary>
		/// 표시 완료.
		/// </summary>
		Presented,

		/// <summary>
		/// 표시 중단 진행 중.
		/// </summary>
		Dismissing,

		/// <summary>
		/// 표시 중단 완료.
		/// </summary>
		Dismissed,
	}
}