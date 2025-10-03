using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Crockhead.Table;
using Crockhead.Core;


namespace Crockhead.Unity.Table
{
	/// <summary>
	/// 레코드 배열 리더.
	/// </summary>
	public class RecordArrayReader<TRecordable> : Disposable where TRecordable : IRecordable
	{
		/// <summary>
		/// 경로.
		/// </summary>
		private string m_AssetPath;

		/// <summary>
		/// 레코드 목록.
		/// </summary>
		private List<TRecordable> m_Records;

		/// <summary>
		/// 읽어들인 레코드 목록 프로퍼티.
		/// </summary>
		public TRecordable[] Records => m_Records.ToArray();

		/// <summary>
		/// 생성됨.
		/// </summary>
		public RecordArrayReader(string assetPath) : base()
		{
			m_AssetPath = assetPath;
			m_Records = new List<TRecordable>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			//base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 읽기.
		/// </summary>
		private int Read(string assetPath)
		{
			m_AssetPath = assetPath;
			using var assetReader = new AssetReader<TextAsset>(assetPath);
			assetReader.Read();

			var textAsset = assetReader.Result;
			if (textAsset == null)
			{
				var type = typeof(TRecordable);
				Debug.LogError($"[RecordArrayReader] '{type}' JSON File Load Failed.");
				throw new FileNotFoundException(assetPath);
			}

			var json = textAsset.text;
			try
			{
				var records = JsonConvert.DeserializeObject<TRecordable[]>(json);
				m_Records.Clear();
				if (records != null && records.Length > 0)
					m_Records.AddRange(records);
				return m_Records.Count;
			}
			catch (Exception exception)
			{
				Debug.LogError($"[RecordArrayReader] '{assetPath}' JSON File Load Failed.");
				throw exception;
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
		/// 읽기.
		/// </summary>
		public int Read()
		{
			return Read(m_AssetPath);
		}
	}
}