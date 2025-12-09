using Crockhead.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 스케쥴러.
	/// </summary>
	public class Scheduler : Disposable
	{
		/// <summary>
		/// 업데이트 리스트.
		/// </summary>
		private List<Action> m_Schedules;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Scheduler() : base()
		{
			UnityPlayerLoop.Add(OnUpdate);

			m_Schedules = new List<Action>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			UnityPlayerLoop.Remove(OnUpdate);

			if (m_Schedules != null)
			{
				m_Schedules.Clear();
				m_Schedules = null;
			}
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		protected virtual void OnUpdate()
		{
			if (m_Schedules == null || m_Schedules.Count == 0)
				return;

			var snapshot = m_Schedules.ToArray();
			foreach (var action in snapshot)
			{
				try
				{
					action();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		/// <summary>
		/// 모두 비우기.
		/// </summary>
		public void Clear()
		{
			m_Schedules.Clear();
		}

		/// <summary>
		/// 추가.
		/// </summary>
		public bool Add(Action action)
		{
			if (action == null)
				return false;

			if (m_Schedules.Contains(action))
				return false;

			m_Schedules.Add(action);
			return true;
		}


		/// <summary>
		/// 제거.
		/// </summary>
		public bool Remove(Action action)
		{
			if (action == null)
				return false;

			var removed = m_Schedules.Remove(action);
			return removed;
		}
	}
}