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
			
			IsImmediateUpdate = true;

			m_Controllers = new List<UIController>();
			m_ReorderableList = new ReorderableList(m_Controllers, typeof(UIController), false, true, false, false);
			m_ReorderableList.drawHeaderCallback = OnDrawHeader;
			m_ReorderableList.drawElementCallback = OnDrawElement;
			m_ReorderableList.elementHeight = EditorGUIUtility.singleLineHeight + 4;
		}

		/// <summary>
		/// 헤더 표시.
		/// </summary>
		private void OnDrawHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Presentation");
		}

		/// <summary>
		/// 항목 표시.
		/// </summary>
		private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			if (index < 0 || index >= m_Controllers.Count)
				return;

			rect.y += 2;

			var controller = m_Controllers[index];
			if (controller != null)
			{
				EditorGUI.LabelField(rect, $"{controller}");
			}
			else
			{
				EditorGUI.LabelField(rect, $"<null>");
			}
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
			//EditorGUILayout.LabelField("Presentation", EditorStyles.boldLabel);

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
				EditorGUILayout.HelpBox("Presentation List is Playmode Only.", MessageType.Warning);
			}
		}
	}
}