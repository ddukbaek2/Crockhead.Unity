using System;
using UnityEngine;


namespace Crockhead.Unity.Exceptions
{
	/// <summary>
	/// 트랜스폼을 찾을 수 없음.
	/// </summary>
	public class NotFoundTransformException : UnityException
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public NotFoundTransformException() : base()
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NotFoundTransformException(string message) : base(message)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NotFoundTransformException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NotFoundTransformException(Transform transform, string transformPath) : this(CreateMessage(transform, transformPath))
		{
		}

		/// <summary>
		/// 생성자 체이닝 안에서 조건에 따른 메시지 생성.
		/// </summary>
		private static string CreateMessage(Transform transform, string transformPath)
		{
			var message = string.Empty;
			if (transform == null)
			{
				message = $"Not Found Transform: Target is Null.";
			}
			else if (string.IsNullOrWhiteSpace(transformPath))
			{
				message = $"Not Found Transform: Path is Null.";
			}
			else
			{
				message = $"Not Found Transform: Target={transform}, Path={transformPath}";
			}

			return message;
		}
	}
}