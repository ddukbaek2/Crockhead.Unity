using TMPro.EditorUtilities;
using UnityEditor;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 인풋 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIInputView), true)]
	[CanEditMultipleObjects]
	public class UIInputViewEditor : TMP_InputFieldEditor
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