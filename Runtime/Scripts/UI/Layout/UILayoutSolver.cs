using Crockhead.Core;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 레이아웃 정렬 처리기.
	/// </summary>
	public class UILayoutSolver : Disposable
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UILayoutSolver() : base()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 처리.
		/// </summary>
		public void Solve()
		{
			// 1. 모든 수집.
			// 2. 수식 생성.
			// 3. 해 도출.
			// 4. 값 반영.
		}
	}
}