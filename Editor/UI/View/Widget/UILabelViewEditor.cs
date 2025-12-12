using Crockhead.Unity.UI;
using TMPro.EditorUtilities;
using UnityEditor;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 레이블 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UILabelView), true)]
	[CanEditMultipleObjects]
	public class UILabelViewEditor : TMP_EditorPanelUI
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