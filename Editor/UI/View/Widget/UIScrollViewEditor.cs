using Crockhead.Unity.UI;
using UnityEditor;
using UnityEditor.UI;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 스크롤 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIScrollView), true)]
	[CanEditMultipleObjects]
	public class UIScrollViewEditor : ScrollRectEditor
	{
		/// <summary>
		/// 인스펙터 출력됨.
		/// </summary>
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}
	}
}