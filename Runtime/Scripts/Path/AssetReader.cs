using Crockhead.Core;
using System;
using UnityEngine;
using Asset = UnityEngine.Object;


namespace Crockhead.Unity
{
	/// <summary>
	/// 애셋 리더.
	/// </summary>
	public class AssetReader<TAsset> : Disposable where TAsset : Asset
	{
		/// <summary>
		/// 애셋 경로 타입.
		/// </summary>
		private AssetPathType m_Type;

		/// <summary>
		/// 애셋 경로 값.
		/// </summary>
		private string m_Path;

		/// <summary>
		/// 읽어들인 애셋.
		/// </summary>
		private TAsset m_Asset;

		/// <summary>
		/// 읽어들인 결과 프로퍼티.
		/// </summary>
		public TAsset Result => m_Asset;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetReader(string path, AssetPathType type) : base()
		{
			m_Type = type;

			switch (type)
			{
				case AssetPathType.Resources:
					{
						m_Path = AssetPaths.GetResourcePath(path);
						break;
					}

				case AssetPathType.Addressables:
					{
						m_Path = path;
						break;
					}

				default:
					{
						m_Path = string.Empty;
						break;
					}
			}

			m_Asset = null;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetReader(AssetPathAttribute assetPath) : this(assetPath.Value, assetPath.Type)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetReader(string path) : this(path, AssetPaths.GetInferAssetPathType(path))
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			if (m_Asset != null)
			{
				if (m_Asset is GameObject)
				{
					// 게임오브젝트는 삭제하면 안됨.
				}
				else
				{
					Resources.UnloadAsset(m_Asset);
				}
				m_Asset = null;
			}
		}

		/// <summary>
		/// 읽기.
		/// </summary>
		public Operation<TAsset> Read()
		{	
			void OnOperation(Operation<TAsset> operation)
			{
				try
				{
					switch (m_Type)
					{
						case AssetPathType.Resources:
							{
								m_Asset = Resources.Load<TAsset>(m_Path);
								operation.Success(m_Asset);
								break;
							}

						case AssetPathType.Addressables:
							{
								// 아직 번들 지원 안함.
								operation.Fail(new InvalidOperationException("[AssetReader] Not Supported Addressables."));
								break;
							}

						default:
							{
								operation.Fail(new InvalidOperationException("[AssetReader] Unknown Error."));
								break;
							}
					}
				}
				catch (OperationCanceledException)
				{
					operation.Cancel();
				}
				catch (Exception exception)
				{
					operation.Fail(exception);
					Debug.LogException(exception);
					throw;
				}
			}

			var operation = new Operation<TAsset>(OnOperation);
			operation.Start();
			return operation;
		}
	}
}