using Crockhead.Core;
using System;
using System.Collections;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 오퍼레이션 핸들러. (yield)
	/// </summary>
	public class UnityOperation : Disposable, IOperation, IEnumerator
	{
		/// <summary>
		/// 작업.
		/// </summary>
		private Operation m_Operation;

		/// <summary>
		/// 진행 중 프로퍼티.
		/// </summary>
		public bool IsRunning => m_Operation.IsRunning;

		public bool IsStarted => m_Operation.IsStarted;

		public bool IsCompleted => m_Operation.IsCompleted;

		public bool IsSucceeded => m_Operation.IsSucceeded;

		public bool IsFaulted => m_Operation.IsFaulted;

		public bool IsCanceled => m_Operation.IsCanceled;

		public Exception Exception => m_Operation.Exception;

		public OperationStatus Status => m_Operation.Status;

		/// <summary>
		/// 열거자의 현재 요소 프로퍼티.
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
		public UnityOperation() : base()
		{
			m_Operation = new Operation();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Disposables.Dispose(m_Operation);
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetCompletion(Action<IOperation> completion)
		{
			m_Operation.SetCompletion(completion);
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetOperation(Action<IOperation> operation)
		{
			m_Operation.SetOperation(operation);
		}

		/// <summary>
		/// 재초기화.
		/// </summary>
		public void Reset()
		{
			m_Operation.Reset();
		}

		/// <summary>
		/// 시작.
		/// </summary>
		public void Start()
		{
			m_Operation.Start();
		}

		/// <summary>
		/// 성공.
		/// </summary>
		public void Success()
		{
			m_Operation.Success();
		}

		/// <summary>
		/// 취소.
		/// </summary>
		public void Cancel()
		{
			((IOperation)m_Operation).Cancel();
		}

		/// <summary>
		/// 실패.
		/// </summary>
		public void Fail(Exception exception)
		{
			m_Operation.Fail(exception);
		}

		/// <summary>
		/// 열거자의 다음 요소로 이동.
		/// </summary>
		bool IEnumerator.MoveNext()
		{
			return !m_Operation.IsCompleted;
		}
	}
}