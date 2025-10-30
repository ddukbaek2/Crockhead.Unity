using UnityEngine;
using UnityEditor;


namespace Crockhead.Unity.Editor
{
	/// <summary>
	/// 빌드 에디터.
	/// </summary>
	public static class BuildEditor
	{
		/// <summary>
		/// 안드로이드 빌드 메뉴 문자열.
		/// </summary>
		public const string BuildForAndroidMenuString = "Window/Crockhead/Build/Build Android";

		/// <summary>
		/// 안드로이드 빌드 여부.
		/// </summary>
		[MenuItem(BuildEditor.BuildForAndroidMenuString, priority = 100000, validate = true)]
		public static bool IsBuildForAndroid()
		{
#if UNITY_ANDROID
			return true;
#else
			return false;
#endif
		}

		/// <summary>
		/// 안드로이드 빌드.
		/// </summary>
		[MenuItem(BuildEditor.BuildForAndroidMenuString, priority = 100000)]
		public static void BuildForAndroid()
		{
#if UNITY_ANDROID
			Builder.BuildForAndroid();
#endif
		}
	}
}
