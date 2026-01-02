using Crockhead.Core;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 프로퍼티.
	/// </summary>
	public class UIProperty : Disposable
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