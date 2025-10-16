using Crockhead.Core;
using System;
using System.Collections;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 프로퍼티.
	/// </summary>
	public class UIProperty : Disposable, IEqualityComparer
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIProperty(string name, Type type) : base()
		{
			Name = name;
			Type = type;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 동일 여부 반환.
		/// </summary>
		bool IEqualityComparer.Equals(object x, object y)
		{
			return Equals(x, y);
		}

		/// <summary>
		/// 고유 해시 값 반환.
		/// </summary>
		int IEqualityComparer.GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}

		/// <summary>
		/// 값 반환.
		/// </summary>
		public static T GetValue<T>(UIProperty target)
		{
			if (target == null)
				return default;

			if (target is UIProperty<T> property)
			{
				return property.Value;
			}

			return default;
		}
	}
}