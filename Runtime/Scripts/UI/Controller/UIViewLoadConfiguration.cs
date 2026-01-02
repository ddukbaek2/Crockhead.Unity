using System;

namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 뷰 로드 설정.
	/// <para>(Type ViewType, string AssetPath, AssetPathType AssetPathType)</para>
	/// </summary>
	public struct UIViewLoadConfiguration
	{
		/// <summary>
		/// 뷰 종류.
		/// </summary>
		public Type ViewType { set; get; }

		/// <summary>
		/// 애셋 경로.
		/// </summary>
		public string AssetPath { set; get; }

		/// <summary>
		/// 애셋 경로 타입.
		/// </summary>
		public AssetPathType AssetPathType { set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UIViewLoadConfiguration((Type ViewType, string AssetPath, AssetPathType AssetPathType) value)
		{
			ViewType = value.ViewType;
			AssetPath = value.AssetPath;
			AssetPathType = value.AssetPathType;
		}

		/// <summary>
		/// 튜플 변환.
		/// </summary>
		public (Type ViewType, string AssetPath, AssetPathType AssetPathType) ToValue()
		{
			return (ViewType, AssetPath, AssetPathType);
		}

		/// <summary>
		/// 변환.
		/// </summary>
		public static implicit operator (Type ViewType, string AssetPath, AssetPathType AssetPathType)(UIViewLoadConfiguration viewLoadConfiguration)
		{
			return viewLoadConfiguration.ToValue();
		}

		/// <summary>
		/// 변환.
		/// </summary>
		public static implicit operator UIViewLoadConfiguration((Type ViewType, string AssetPath, AssetPathType AssetPathType) value)
		{
			var viewLoadConfiguration = new UIViewLoadConfiguration(value);
			return viewLoadConfiguration;
		}
	}
}