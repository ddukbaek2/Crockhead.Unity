using Crockhead.Core;
using Crockhead.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity.Table
{
	/// <summary>
	/// 레코드 배열 리더.
	/// </summary>
	public class RecordArrayAssetReader<TRecordable> : Disposable, IRecordArrayReader<TRecordable> where TRecordable : IRecordable
	{
		/// <summary>
		/// 경로.
		/// </summary>
		private string m_AssetPath;

		/// <summary>
		/// 타입.
		/// </summary>
		private AssetPathType m_AssetPathType;

		/// <summary>
		/// 레코드 목록.
		/// </summary>
		private List<TRecordable> m_Records;

		/// <summary>
		/// 읽어들인 레코드 목록 프로퍼티.
		/// </summary>
		public TRecordable[] Records => m_Records.ToArray();

		IEnumerable<TRecordable> IRecordArrayReader<TRecordable>.Records => Records;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public RecordArrayAssetReader(object sharedTableInstance) : base()
		{
			if (sharedTableInstance == null)
				throw new ArgumentNullException(nameof(sharedTableInstance));

			var sharedTableType = sharedTableInstance.GetType();
			if (!Reflections.TryGetAttribute<AssetPathAttribute>(sharedTableType, out var assetPathAttribute))
				throw new ArgumentNullException(nameof(assetPathAttribute));

			InternalInitialize(assetPathAttribute);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public RecordArrayAssetReader(AssetPathAttribute assetPathAttribute) : base()
		{
			InternalInitialize(assetPathAttribute);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public RecordArrayAssetReader(string assetPath, AssetPathType assetPathType) : base()
		{
			InternalInitialize(assetPath, assetPathType);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 초기화.
		/// </summary>
		private void InternalInitialize(string assetPath, AssetPathType assetPathType)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(assetPath))
					throw new ArgumentNullException(nameof(assetPath));

				m_AssetPath = assetPath;
				m_AssetPathType = assetPathType;
				m_Records = new List<TRecordable>();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 초기화.
		/// </summary>
		private void InternalInitialize(AssetPathAttribute assetPathAttribute)
		{
			try
			{
				if (assetPathAttribute == null)
					throw new ArgumentNullException(nameof(assetPathAttribute));
				if (!assetPathAttribute.IsEnabled)
					throw new InvalidOperationException(nameof(assetPathAttribute.IsEnabled));

				var assetPathType = assetPathAttribute.Type;
				var assetPathValue = assetPathAttribute.Value;
				InternalInitialize(assetPathValue, assetPathType);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 불러오기.
		/// </summary>
		public IEnumerable<TRecordable> Read()
		{			
			try
			{
				using var assetLoader = new AssetLoader<TextAsset>(m_AssetPath, m_AssetPathType);
				assetLoader.Load();

				var textAsset = assetLoader.Asset;
				if (textAsset == null)
				{
					Debug.LogError($"[RecordArrayAssetReader] Not Found JSON File. ({m_AssetPath})");
					throw new FileNotFoundException(m_AssetPath);
				}

				var json = textAsset.text;
				var records = JsonConvert.DeserializeObject<TRecordable[]>(json);
				m_Records.Clear();
				if (records != null && records.Length > 0)
					m_Records.AddRange(records);
				return m_Records;
			}
			catch
			{
				Debug.LogError($"[RecordArrayAssetReader] JSON File Load Failed. ({m_AssetPath})");
				throw;
			}
			//finally
			//{
			//	if (textAsset != null)
			//	{
			//		Resources.UnloadAsset(textAsset);
			//		textAsset = null;
			//	}
			//}
		}

		/// <summary>
		/// 불러오기. (비동기)
		/// </summary>
		public async Task<IEnumerable<TRecordable>> ReadAsync()
		{
			try
			{
				using var assetLoader = new AssetLoader<TextAsset>(m_AssetPath, m_AssetPathType);
				return await UnityThreadDispatcher.PostAsync(async () =>
				{
					var asyncOperation = assetLoader.LoadAsync();
					await TaskHelper.WaitForCompletion(asyncOperation);

					var textAsset = assetLoader.Asset;
					if (textAsset == null)
					{
						Debug.LogError($"[RecordArrayAssetReader] Not Found JSON File. ({assetLoader})");
					}

					var json = textAsset.text;
					var records = JsonConvert.DeserializeObject<TRecordable[]>(json);
					m_Records.Clear();
					if (records != null && records.Length > 0)
						m_Records.AddRange(records);
					return m_Records;
				});
			}
			catch
			{
				throw;
			}
		}
	}
}