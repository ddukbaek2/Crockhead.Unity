using Crockhead.Core;
using Crockhead.Unity.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 영역 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIWindow), true)]
	//[CanEditMultipleObjects]
	public class UIWindowEditor : BaseEditor
	{
		private List<UIController> m_Controllers;
		private ReorderableList m_ReorderableList;

		/// <summary>
		/// 활성화됨.
		/// </summary>
		protected override void OnEnable()
		{
			base.OnEnable();

			m_Controllers = new List<UIController>();
			m_ReorderableList = new ReorderableList(m_Controllers, typeof(UIController), false, true, false, false);
			m_ReorderableList.drawHeaderCallback = OnDrawHeader;
			m_ReorderableList.drawElementCallback = OnDrawElement;
			m_ReorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 4;
		}

		private void OnDrawHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Stack (Top → Bottom)");
		}

		private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.y += 2;
			EditorGUI.LabelField(rect, m_Controllers[index]?.ToString() ?? "<null>");
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		protected override void OnUpdate()
		{
			base.OnUpdate();
		}

		/// <summary>
		/// 출력됨.
		/// </summary>
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var component = target as UIWindow;

			//EditorGUILayout.Space();
			EditorGUILayout.LabelField("Presentation", EditorStyles.boldLabel);

			if (Application.isPlaying)
			{
				var presentationCoordinator = Reflections.GetFieldValue<UIPresentationCoordinator>(component, "m_PresentationCoordinator");
				if (presentationCoordinator == null)
					return;

				m_Controllers.Clear();
				m_Controllers.AddRange(presentationCoordinator.Controllers);
				m_ReorderableList.list = m_Controllers;
				m_ReorderableList.DoLayoutList();
			}
			else
			{
				EditorGUILayout.HelpBox("Presentation History is Playmode Only.", MessageType.Warning);
			}
		}

		/// <summary>
		/// 필터링.
		/// </summary>
		protected override bool OnVisibleProperty(SerializedProperty serializedProperty)
		{
			var visibled = base.OnVisibleProperty(serializedProperty);
			if (visibled)
			{
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

			return visibled;
		}
	}
}