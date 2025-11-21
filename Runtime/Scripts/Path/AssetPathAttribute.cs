using System;


namespace Crockhead.Unity
{
	/// <summary>
	/// 연결된 애셋 경로를 수식하는 특성.
	/// </summary>
	public class AssetPathAttribute : Attribute
	{
		/// <summary>
		/// 타입.
		/// </summary>
		private AssetPathType m_Type;

		/// <summary>
		/// 값.
		/// </summary>
		private string m_Value;

		/// <summary>
		/// 사용 여부.
		/// </summary>
		private bool m_IsEnabled;

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public AssetPathType Type => m_Type;

		/// <summary>
		/// 값 프로퍼티.
		/// </summary>
		public string Value => m_Value;

		/// <summary>
		/// 사용 여부 프로퍼티.
		/// </summary>
		public bool IsEnabled => m_IsEnabled;

		/// <summary>
		/// 경로 프로퍼티.
		/// </summary>
		public virtual string Path
		{
			get
			{
				switch (m_Type)
				{
					case AssetPathType.Resources:
						{
							var value = AssetPathHelper.GetResourcePath(m_Value);
							return value;
						}

					case AssetPathType.Addressables:
						{
							// 저장한 그대로.
							var value = m_Value;
							return value;
						}

					default:
						{
							// 값은 유효하지 않음.
							throw new InvalidOperationException();
						}
				}
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetPathAttribute(string path, AssetPathType type = AssetPathType.Resources, bool enabled = true) : base()
		{
			m_Type = type;
			m_Value = path;
			m_IsEnabled = enabled;
		}
	}
}