using Crockhead.Unity.Editor;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 프로그레스 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIProgressView), true)]
	[CanEditMultipleObjects]
	public class UIProgressViewEditor : UISliderViewEditor
	{
		/// <summary>
		/// 필터링: 표시 될 프로퍼티. (기본값: 표시)
		/// </summary>
		protected override bool OnVisibleProperty(SerializedProperty serializedProperty)
		{
			if (serializedProperty == null)
				return false;

			switch (serializedProperty.propertyPath)
			{
				//case "m_Material":
				////case "m_Color":
				//case "m_Maskable":
				//case "m_OnCullStateChanged":
				//	return false;
				default:
					return true;
			}
		}

		/// <summary>
		/// 필터링: 읽기전용 프로퍼티. (기본값: 쓰기가능)
		/// </summary>
		protected override bool OnReadonlyProperty(SerializedProperty serializedProperty)
		{
			if (serializedProperty == null)
				return false;

			switch (serializedProperty.propertyPath)
			{
				case "m_Script":
					return true;
				default:
					return false;
			}
		}
	}
}