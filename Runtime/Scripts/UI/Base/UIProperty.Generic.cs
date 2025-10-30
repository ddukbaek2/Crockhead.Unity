using System.Collections.Generic;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 제네릭 프로퍼티.
	/// </summary>
	public class UIProperty<TValue> : UIProperty
	{
		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public TValue Value { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIProperty(string name, TValue value) : base(name)
		{
			Value = value;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 비교.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is not UIProperty<TValue> property)
				return false;
			return Equals(this, property);
		}

		/// <summary>
		/// 비교.
		/// </summary>
		public bool Equals(UIProperty<TValue> property)
		{
			return EqualityComparer<TValue>.Default.Equals(Value, property.Value);
		}

		/// <summary>
		/// 고유 해시값 반환.
		/// </summary>
		public override int GetHashCode()
		{
			//return base.GetHashCode();
			return Value.GetHashCode();
		}
	}
}