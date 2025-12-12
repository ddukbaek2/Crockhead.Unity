using UnityEditor;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 드래그 가능한 아이템 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIDraggableItemView), true)]
	[CanEditMultipleObjects]
	public class UIDraggableItemViewEditor : UIViewEditor
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