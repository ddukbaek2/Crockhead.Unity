using System;


namespace Crockhead.Unity
{
	/// <summary>
	/// 연결된 리소스 애셋 경로를 수식하는 특성.
	/// </summary>
	public class ResourcePathAttribute : AssetPathAttribute
	{
		/// <summary>
		/// 경로 프로퍼티.
		/// </summary>
		public override string Path
		{
			get
			{
				var path = AssetPaths.GetResourcePath(Value);
				return path;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ResourcePathAttribute(string path, bool enabled = true) : base(path, AssetPathType.Resources, enabled)
		{
		}
	}
}