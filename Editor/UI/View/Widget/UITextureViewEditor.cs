using Crockhead.Unity.UI;
using UnityEditor;
using UnityEditor.UI;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 텍스쳐 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UITextureView), true)]
	[CanEditMultipleObjects]
	public class UITextureViewEditor : RawImageEditor
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