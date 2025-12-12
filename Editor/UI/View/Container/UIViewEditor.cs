using UnityEditor;
using InspectorEditor = UnityEditor.Editor;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 기본 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIView), true)]
	[CanEditMultipleObjects]
	public class UIViewEditor : InspectorEditor
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