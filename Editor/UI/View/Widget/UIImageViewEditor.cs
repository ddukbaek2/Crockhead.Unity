using UnityEditor;
using UnityEditor.UI;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 이미지 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIImageView), true)]
	[CanEditMultipleObjects]
	public class UIImageViewEditor : ImageEditor
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