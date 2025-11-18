using Crockhead.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Crockhead.Table;


namespace Crockhead.Unity.Table
{
	/// <summary>
	/// 공유 테이블.
	/// </summary>
	public class SharedTable<TClass, TRecordable> : SharedClass<TClass>
		where TClass : SharedTable<TClass, TRecordable>, new()
		where TRecordable : IRecordable
	{
		/// <summary>
		/// 컬렉션.
		/// </summary>
		private Table<TRecordable> m_Collection;

		/// <summary>
		/// 컬렉션 프로퍼티.
		/// </summary>
		public Table<TRecordable> Collection => m_Collection;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SharedTable() : base()
		{
			m_Collection = null;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected override void OnCreate(params object[] arguments)
		{
			var tableClassType = typeof(TClass);
			if (Reflections.TryGetAttribute<AssetPathAttribute>(tableClassType, out var assetPath))
			{
				var jsonAssetPath = assetPath.Value;
				using var reader = new RecordArrayReader<TRecordable>(jsonAssetPath);
				reader.Read();
				var records = reader.Records;
				m_Collection = Table<TRecordable>.Create();
				m_Collection.AddRange(records);
				OnLoaded();
				Debug.Log($"[{tableClassType}] Load Complete. Path: \"{jsonAssetPath}\"");
			}
			else
			{
				Debug.LogError($"[{tableClassType}] Not Found AssetPath Attribute.");
			}
		}

		/// <summary>
		/// 테이블 로드 됨.
		/// </summary>
		protected virtual void OnLoaded()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Disposables.SafeDispose(ref m_Collection);

			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 조건에 맞는 레코드 반환.
		/// </summary>
		public TRecordable Find(int id)
		{
			return m_Collection.Find(id);
		}

		/// <summary>
		/// 조건에 맞는 레코드 반환.
		/// </summary>
		public TRecordable Find(Predicate<TRecordable> predicate)
		{
			return m_Collection.Find(predicate);
		}

		/// <summary>
		/// 조건에 맞는 모든 레코드 반환.
		/// </summary>
		public List<TRecordable> FindAll(Predicate<TRecordable> predicate)
		{
			return m_Collection.FindAll(predicate);
		}

		/// <summary>
		/// 모든 레코드 반환.
		/// </summary>
		public List<TRecordable> All()
		{
			return m_Collection.All();
		}
	}
}