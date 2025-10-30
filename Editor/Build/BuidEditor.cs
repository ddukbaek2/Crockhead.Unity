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
		/// 안드로이드 빌드.
		/// </summary>
		[MenuItem("Window/Crockhead/Build/Android", priority = 100000)]
		public static void BuildForAndroid()
		{
			Builder.BuildForAndroid();
		}
	}
}
