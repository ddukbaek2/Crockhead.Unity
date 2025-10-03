using Crockhead.Core;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 클래스 유틸리티.
	/// </summary>
	public static class UClasses
	{
		/// <summary>
		/// 생성.
		/// </summary>
		public static TUClass Create<TUClass>(params object[] arguments) where TUClass : UClass
		{
			var obj = Reflections.CreateInstance<TUClass>(arguments);
			Reflections.InvokeInstanceMethod(obj, "OnCreate");
			return obj;
		}

		/// <summary>
		/// 해제.
		/// </summary>
		public static void Dispose(UClass obj)
		{
			Disposables.Dispose(obj);
		}

		/// <summary>
		/// 안전한 해제.
		/// </summary>
		public static void SafeDispose<TUClass>(ref TUClass obj) where TUClass : UClass
		{
			Disposables.SafeDispose(ref obj);
		}
	}
}