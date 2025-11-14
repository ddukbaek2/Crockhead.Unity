namespace Crockhead.Unity
{
	/// <summary>
	/// 사운드 종류. (오디오 믹서 그룹 분류 겸용)
	/// </summary>
	public enum SoundType
	{
		/// <summary>
		/// 없음.
		/// </summary>
		None = 0,

		/// <summary>
		/// 마스터.
		/// </summary>
		Master,

		/// <summary>
		/// 백그라운드 음악.
		/// </summary>
		Background,

		/// <summary>
		/// 효과.
		/// </summary>
		Effect,

		/// <summary>
		/// 음성.
		/// </summary>
		Voice,

		/// <summary>
		/// 인터페이스.
		/// </summary>
		UI,
	}
}