using Crockhead.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 비동기 오퍼레이션. (yield, async, await)
	/// </summary>
	public class UnityAsyncOperation : Operation, IEnumerator
	{
		/// <summary>
		/// 대기자.
		/// </summary>
		public readonly struct Awaiter : ICriticalNotifyCompletion
		{
			/// <summary>
			/// 명령.
			/// </summary>
			private readonly UnityAsyncOperation m_Operation;

			/// <summary>
			/// 생성됨.
			/// </summary>
			public Awaiter(UnityAsyncOperation operation)
			{
				m_Operation = operation;
			}

			/// <summary>
			/// 완료 처리.
			/// </summary>
			void INotifyCompletion.OnCompleted(Action continuation)
			{
				if (continuation == null)
					return;

				if (m_Operation.IsCompleted)
				{
					UnityThreadDispatcher.Post(continuation);
				}
				else
				{
					m_Operation.m_Continuations.Add(continuation);
				}

				if (m_Operation.IsCompleted)
				{
					m_Operation.m_Continuations.Remove(continuation);
					UnityThreadDispatcher.Post(continuation);
				}
			}

			/// <summary>
			/// 완료 처리.
			/// </summary>
			void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
			{
				((INotifyCompletion)this).OnCompleted(continuation);
			}

			/// <summary>
			/// 결과 처리.
			/// </summary>
			void GetResult()
			{
				if (m_Operation.IsFaulted)
				{
					var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(m_Operation.Exception);
					exceptionDispatchInfo.Throw();
				}
			}
		}


		/// <summary>
		/// 대기자.
		/// </summary>
		private Awaiter m_Awaiter;

		/// <summary>
		/// 완료 처리자.
		/// </summary>
		private List<Action> m_Continuations;

		/// <summary>
		/// 현재 열거 요소 프로퍼티.
		/// </summary>
		object IEnumerator.Current
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public UnityAsyncOperation(Action<IOperation> operation) : base(operation, null)
		{
			m_Awaiter = new Awaiter(this);
			m_Continuations = new List<Action>();
			SetCompletion(OnCompleted);
		}

		/// <summary>
		/// 완료됨.
		/// </summary>
		private void OnCompleted(IOperation operation)
		{
			var continuations = m_Continuations.ToArray();
			foreach (var continuation in continuations)
			{
				UnityThreadDispatcher.Post(continuation);
			}
			m_Continuations.Clear();
		}

		/// <summary>
		/// 다음 요소로 이동.
		/// </summary>
		bool IEnumerator.MoveNext()
		{
			return !IsCompleted;
		}

		/// <summary>
		/// 대기자 반환.
		/// </summary>
		public Awaiter GetAwaiter()
		{
			return m_Awaiter;
		}
	}
}