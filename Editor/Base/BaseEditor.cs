using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using InspectorEditor = UnityEditor.Editor;


namespace Crockhead.Unity.Editor
{
	/// <summary>
	/// 인스펙터 확장.
	/// </summary>
	//[CustomEditor(typeof(UIWindow), true)]
	[CanEditMultipleObjects]
	public abstract class BaseEditor : InspectorEditor
	{
		/// <summary>
		/// 즉시 업데이트 여부 프로퍼티.
		/// </summary>
		public bool IsImmediateUpdate { protected set; get; }

		/// <summary>
		/// 활성화됨.
		/// </summary>
		protected virtual void OnEnable()
		{
			IsImmediateUpdate = false;
			EditorApplication.update += OnEditorUpdate;
		}

		/// <summary>
		/// 비활성화됨.
		/// </summary>
		protected virtual void OnDisable()
		{
			EditorApplication.update -= OnEditorUpdate;
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		private void OnEditorUpdate()
		{
			if (target == null)
				return;
			if (!IsImmediateUpdate)
				return;

			OnUpdate();
			Repaint();
		}

		/// <summary>
		/// 갱신됨.
		/// </summary>
		protected virtual void OnUpdate()
		{
		}

		/// <summary>
		/// 인스펙터 출력됨.
		/// </summary>
		public override void OnInspectorGUI()
		{
			//base.OnInspectorGUI();
			//DrawDefaultInspector();
			//DoDrawDefaultInspector();

			if (target == null)
				return;

			// 그리기.
			using (var localizationGroup = new LocalizationGroup(target))
			{
				DrawPropertyAsSerializedObject(serializedObject, OnVisibleProperty, OnReadonlyProperty);
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

		/// <summary>
		/// 시리얼라이즈 오브젝트 그리기.
		/// </summary>
		public static bool DrawPropertyAsSerializedObject(SerializedObject serializedObject, Predicate<SerializedProperty> propertyVisibleFilter, Predicate<SerializedProperty> propertyReadonlyFilter)
		{
			EditorGUI.BeginChangeCheck();
			serializedObject.UpdateIfRequiredOrScript();
			SerializedProperty iterator = serializedObject.GetIterator();
			bool enterChildren = true;
			while (iterator.NextVisible(enterChildren))
			{
				var isVisible = propertyVisibleFilter?.Invoke(iterator) ?? true;
				if (!isVisible)
					continue;

				var isReadonly = propertyReadonlyFilter?.Invoke(iterator) ?? false;
				using (new EditorGUI.DisabledScope(isReadonly))
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