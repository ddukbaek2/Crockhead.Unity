using UnityEditor;
using UnityEngine;


namespace Crockhead.Unity.Editor
{
	/// <summary>
	/// 하이어라키 확장.
	/// <para>다음의 특성을 메서드에 추가. [MenuItem("GameObject/Crockhead/{명령어}")]</para>
	/// </summary>
	public class HierarchyExtension
	{
		/// <summary>
		/// 컨텍스트 혹은 셀렉션으로부터 트랜스폼 반환.
		/// </summary>
		public static TTransform GetSelectionTransform<TTransform>(MenuCommand menuCommand = null) where TTransform : Transform
		{
			if (menuCommand != null && menuCommand.context != null && menuCommand.context is GameObject contextObj)
			{
				return contextObj.GetComponent<TTransform>();
			}
			else if (Selection.activeGameObject != null)
			{
				return Selection.activeGameObject.GetComponent<TTransform>();
			}

			return default;
		}

		/// <summary>
		/// 하이어라키에 새 게임오브젝트와 컴포넌트 추가.
		/// </summary>
		public static TComponent CreateComponentToHierarchy<TComponent>(string name, Transform parentTransform = null) where TComponent : Component
		{
			// 게임오브젝트 생성.
			var obj = new GameObject(name);
			Undo.RegisterCreatedObjectUndo(obj, "NewGameObject");

			// 부모 설정.
			if (parentTransform != null)
			{
				Undo.SetTransformParent(obj.transform, parentTransform, "SetTransformParent");
				obj.gameObject.layer = parentTransform.gameObject.layer;
			}

			// 트랜스폼 설정.
			Undo.RecordObject(obj.transform, "ResetTransform");
			TransformHelper.ResetTransform(obj.transform);

			// 컴포넌트 추가.
			var component = Undo.AddComponent<TComponent>(obj);
			return component;
		}
	}
}