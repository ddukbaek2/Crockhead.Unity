using System;


namespace Crockhead.Unity.UI.Deprecated
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
}