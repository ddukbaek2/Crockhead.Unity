using Crockhead.Core;
using System;
using UnityEngine;
using Asset = UnityEngine.Object;


namespace Crockhead.Unity
{
	/// <summary>
	/// 애셋 로더.
	/// </summary>
	public class AssetLoader<TAsset> : Disposable where TAsset : Asset
	{
		/// <summary>
		/// 불러온 애셋.
		/// </summary>
		private TAsset m_Asset;

		/// <summary>
		/// 애셋 경로 타입.
		/// </summary>
		private AssetPathType m_AssetPathType;

		/// <summary>
		/// 애셋 경로 값.
		/// </summary>
		private string m_AssetPathValue;

		/// <summary>
		/// 애셋이 로드 되었는지 여부 프로퍼티.
		/// </summary>
		public bool IsAssetLoaded => m_Asset != null;

		/// <summary>
		/// 애셋 로드 프로퍼티.
		/// </summary>
		public TAsset Asset
		{
			get
			{
				if (m_Asset == null)
				{
					Load();
				}

				return m_Asset;
			}
		}

		/// <summary>
		/// 불러온 결과 프로퍼티.
		/// </summary>
		public TAsset LoadedAsset => m_Asset;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetLoader(string assetPath, AssetPathType assetPathType) : base()
		{
			if (string.IsNullOrWhiteSpace(assetPath))
				throw new ArgumentNullException(nameof(assetPath));

			m_Asset = null;
			m_AssetPathType = assetPathType;

			switch (assetPathType)
			{
				case AssetPathType.Resources:
					{
						m_AssetPathValue = AssetPathHelper.GetResourcePath(assetPath);
						break;
					}

				case AssetPathType.Addressables:
					{
						m_AssetPathValue = assetPath;
						break;
					}

				default:
					{
						m_AssetPathValue = string.Empty;
						break;
					}
			}		
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetLoader(AssetPathAttribute assetPathAttribute) : this(assetPathAttribute.Value, assetPathAttribute.Type)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AssetLoader(string assetPath) : this(assetPath, AssetPathHelper.GetInferAssetPathType(assetPath))
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
					// 사용이 끝났다 치고 해제 시도. (이미 다른 곳에 참조되어있다면 해제되지 않음)
					Resources.UnloadAsset(m_Asset);
				}

				m_Asset = null;
			}
		}

		/// <summary>
		/// 로드 전 호출됨.
		/// </summary>
		protected virtual void OnAssetWillLoad(string assetPathValue, AssetPathType assetPathType)
		{
		}

		/// <summary>
		/// 로드됨.
		/// </summary>
		protected virtual void OnAssetDidLoad()
		{
			// 로드된 후 제거.
			Disposables.Dispose(this);
		}

		/// <summary>
		/// 애셋 동기 로드.
		/// </summary>
		public TAsset Load()
		{
			if (m_Asset != null)
			{
				return m_Asset;
			}

			try
			{
				OnAssetWillLoad(m_AssetPathValue, m_AssetPathType);
				switch (m_AssetPathType)
				{
					case AssetPathType.Resources:
						{
							m_Asset = Resources.Load<TAsset>(m_AssetPathValue);
							OnAssetDidLoad();
							return m_Asset;
						}

					case AssetPathType.Addressables:
						{
							//Addressables.
							// 아직 번들 지원 안함.
							throw new InvalidOperationException("[AssetLoader] Not Supported Addressables.");
						}

					default:
						{
							throw new InvalidOperationException("[AssetLoader] Unknown Error.");
						}
				}
			}
			catch (OperationCanceledException exception)
			{
				Debug.LogException(exception);
				throw;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				throw;
			}
			//finally
			//{
			//	OnAssetDidLoad();
			//}
		}

		/// <summary>
		/// 애셋 비동기 로드. (YieldInstruction ==> AsyncOperation로 코루틴 대기 가능)
		/// </summary>
		public AsyncOperation LoadAsync()
		{
			if (m_Asset != null)
				return null;

			try
			{
				OnAssetWillLoad(m_AssetPathValue, m_AssetPathType);
				switch (m_AssetPathType)
				{
					case AssetPathType.Resources:
						{
							var resourceRequest = default(ResourceRequest);
							void OnCompleted(AsyncOperation asyncOperation)
							{
								m_Asset = resourceRequest.asset as TAsset;
								OnAssetDidLoad();
							}

							resourceRequest = Resources.LoadAsync<TAsset>(m_AssetPathValue);
							resourceRequest.completed += OnCompleted;
							return resourceRequest;
						}

					case AssetPathType.Addressables:
						{
							//Addressables.
							// 아직 번들 지원 안함.
							throw new InvalidOperationException("[AssetLoader] Not Supported Addressables.");
						}

					default:
						{
							throw new InvalidOperationException("[AssetLoader] Unknown Error.");
						}
				}
			}
			catch (OperationCanceledException exception)
			{
				Debug.LogException(exception);
				throw;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				throw;
			}
		}
	}
}