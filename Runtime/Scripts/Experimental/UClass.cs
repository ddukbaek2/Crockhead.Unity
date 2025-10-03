using Crockhead.Core;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티에서 사용 할 기본 클래스 인스턴스.
	/// </summary>
	public abstract class UClass : Disposable
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public UClass() : base()
		{
		}

		/// <summary>
		/// 생성됨.
		/// <para>주의: UClasses.Create() 를 통해서 UClass 객체를 생성해야 정상 호출됨.</para>
		/// </summary>
		protected abstract void OnCreate();

		/// <summary>
		/// 해제됨.
		/// <para>Disposables.Dispose(this), UClass.Dispose(this)로 명시적 해제를 권고.</para>
		/// <para>명시적 해제가 아닌 상황에서는 GC에 의한 자동 해제시 호출됨.</para>
		/// </summary>
		/// <param name="explicitDisposing">명시적 해제 여부.</param>
		protected override void OnDispose(bool explicitDisposing)
		{
		}
	}
}