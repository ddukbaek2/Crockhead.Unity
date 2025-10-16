using System;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 애셋을 찾지 못한 예외.
	/// </summary>
	public class UIAssetNotFoundException : UIException
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIAssetNotFoundException(string message) : base(message) { }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIAssetNotFoundException(string message, Exception innerException) : base(message) { }
	}
}