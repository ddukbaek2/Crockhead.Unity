using Crockhead.Unity.UI;
using UnityEditor;
using UnityEditor.UI;


namespace Crockhead.Unity.Editor
{
	/// <summary>
	/// 버튼 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIButtonView), true)]
	[CanEditMultipleObjects]
	public class UIButtonEditor : ButtonEditor
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