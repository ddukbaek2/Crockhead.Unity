using System;
using UnityEngine;


namespace Crockhead.Unity.Exceptions
{
	/// <summary>
	/// 예외.
	/// </summary>
	public class UIException : UnityException
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIException() : base()
		{
		}

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