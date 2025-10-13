using System;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 예외.
	/// </summary>
	public class UIException : Exception
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIException(string message) : base(message)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}


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