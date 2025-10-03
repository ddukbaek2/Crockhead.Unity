using Crockhead.Core;
using System.Collections;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 오퍼레이션. (yield)
	/// </summary>
	public class UnityOperation<TResult> : Operation<TResult>, IEnumerator
	{
		/// <summary>
		/// 현재.
		/// </summary>
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 다음 요소로 이동.
		/// </summary>
		bool IEnumerator.MoveNext()
		{
			return !IsCompleted;
		}
	}
}