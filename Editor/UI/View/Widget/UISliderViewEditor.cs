using Crockhead.Unity.Editor;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 슬라이더 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UISliderView), true)]
	[CanEditMultipleObjects]
	public class UISliderViewEditor : SliderEditor
	{
		/// <summary>
		/// 인스펙터 출력됨.
		/// </summary>
		public override void OnInspectorGUI()
		{
			//base.OnInspectorGUI();

			if (target == null)
				return;

			// 그리기.
			using (var localizationGroup = new LocalizationGroup(target))
			{
				BaseEditor.DrawPropertyAsSerializedObject(serializedObject, OnVisibleProperty, OnReadonlyProperty);
				var component = target as MonoBehaviour;
			}
		}

		/// <summary>
		/// 필터링: 표시 될 프로퍼티. (기본값: 표시)
		/// </summary>
		protected virtual bool OnVisibleProperty(SerializedProperty serializedProperty)
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
		protected virtual bool OnReadonlyProperty(SerializedProperty serializedProperty)
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