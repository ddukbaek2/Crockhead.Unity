using System.Numerics;

namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 제네릭 프로퍼티.
	/// </summary>
	public class UIProperty<T> : UIProperty
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public T Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIProperty(string name, T value) : base(name, typeof(T))
		{
			Value = value;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}
	}
}