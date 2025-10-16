using Crockhead.Core;
using System.Collections;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 프로퍼티.
	/// </summary>
	public abstract class UIProperty : Disposable, IEqualityComparer
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIProperty(string name) : base()
		{
			Name = name;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 프로퍼티 변경됨.
		/// </summary>
		protected virtual void OnPropertyChanged(string propertyName)
		{
		}

		public void BeginProperty()
		{
		}

		public void EndProperty()
		{
		}

		bool IEqualityComparer.Equals(object x, object y)
		{
			return Equals(x, y);
		}

		int IEqualityComparer.GetHashCode(object obj)
		{
			return obj.GetHashCode();
		}
	}
}