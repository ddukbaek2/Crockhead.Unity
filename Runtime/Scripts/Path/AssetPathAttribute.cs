using Crockhead.Core;
using System;


namespace Crockhead.Unity
{
	/// <summary>
	/// 연결된 애셋 경로를 수식하는 특성.
	/// </summary>
	public class AssetPathAttribute : FilePathAttribute
	{
		/// <summary>
		/// 타입.
		/// </summary>
		private AssetPathType m_Type;

		/// <summary>
		/// 타입 프로퍼티.
		/// </summary>
		public AssetPathType Type => m_Type;

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
							var value = AssetPathHelper.GetResourcePath(Value);
							return value;
						}

					case AssetPathType.Addressables:
						{
							// 저장한 그대로.
							var value = Value;
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
		public AssetPathAttribute(string path, AssetPathType type, bool enabled) : base(path, enabled)
		{
			m_Type = type;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetPathAttribute(string path, AssetPathType type) : this(path, type, true)
		{
			m_Type = type;
		}
	}
}