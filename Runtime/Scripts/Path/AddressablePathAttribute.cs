using System;


namespace Crockhead.Unity
{
	/// <summary>
	/// 연결된 어드레서블 애셋 경로를 수식하는 특성.
	/// </summary>
	public class AddressablePathAttribute : AssetPathAttribute
	{
		/// <summary>
		/// 경로 프로퍼티.
		/// </summary>
		public override string Path
		{
			get
			{
				var path = Value;
				return path;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AddressablePathAttribute(string path, bool enabled) : base(path, AssetPathType.Addressables, enabled)
		{
		}
	}
}