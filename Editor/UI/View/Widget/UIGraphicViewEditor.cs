using System;
using UnityEditor;
using UnityEngine;
using InspectorEditor = UnityEditor.Editor;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 영역 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIGraphicView), true)]
	[CanEditMultipleObjects]
	public class UIGraphicViewEditor : InspectorEditor
	{
		/// <summary>
		/// 인스펙터 출력됨.
		/// </summary>
		public override void OnInspectorGUI()
		{
			//base.OnInspectorGUI();
			//DrawDefaultInspector();
			//DoDrawDefaultInspector();

			// 그리기.
			using (var localizationGroup = new LocalizationGroup(target))
			{
				DrawPropertyAsSerializedObject(serializedObject, OnVisibleProperty);
				var component = target as MonoBehaviour;
			}
		}

		/// <summary>
		/// 필터링.
		/// </summary>
		protected virtual bool OnVisibleProperty(SerializedProperty serializedProperty)
		{
			if (serializedProperty == null)
				return false;

			switch (serializedProperty.propertyPath)
			{
				case "m_Material":
				//case "m_Color":
				case "m_Maskable":
				case "m_OnCullStateChanged":
					return false;
				default:
					return true;
			}
		}

		/// <summary>
		/// 시리얼라이즈 오브젝트 그리기.
		/// </summary>
		public static bool DrawPropertyAsSerializedObject(SerializedObject serializedObject, Predicate<SerializedProperty> propertyFilter)
		{
			EditorGUI.BeginChangeCheck();
			serializedObject.UpdateIfRequiredOrScript();
			SerializedProperty iterator = serializedObject.GetIterator();
			bool enterChildren = true;
			while (iterator.NextVisible(enterChildren))
			{
				if (!propertyFilter?.Invoke(iterator) ?? false)
					continue;

				using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
				{
					EditorGUILayout.PropertyField(iterator, true);
				}

				enterChildren = false;
			}

			serializedObject.ApplyModifiedProperties();
			return EditorGUI.EndChangeCheck();
		}
	}
}