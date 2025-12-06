using Crockhead.Core;


namespace Crockhead.Unity
{
	/// <summary>
	/// 로컬 데이터 매니저.
	/// </summary>
	public abstract class LocalDataManager<TLocalDataManager, TLocalData> : SharedClass<TLocalDataManager>
		where TLocalDataManager : LocalDataManager<TLocalDataManager, TLocalData>, new()
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public LocalDataManager() : base()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose()
		{
			base.OnDispose();
		}

		/// <summary>
		/// 비우기.
		/// </summary>
		public void Clear()
		{

		}

		/// <summary>
		/// 불러오기.
		/// </summary>
		public bool Load()
		{
			
			return false;
		}

		/// <summary>
		/// 저장.
		/// </summary>
		public void Save()
		{
		}
	}
}