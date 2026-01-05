using Crockhead.Unity.Editor;
using System;
using UnityEditor;
using UnityEngine;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// 영역 뷰 인스펙터 확장.
	/// </summary>
	[CustomEditor(typeof(UIGraphicView), true)]
	[CanEditMultipleObjects]
	public class UIGraphicViewEditor : BaseEditor
	{
		/// <summary>
		/// 인스펙터 출력됨.
		/// </summary>
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
		}

		/// <summary>
		/// 필터링.
		/// </summary>
		protected override bool OnVisibleProperty(SerializedProperty serializedProperty)
		{
			var isVisible = base.OnVisibleProperty(serializedProperty);
			if (isVisible)
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

			return isVisible;
		}
	}
}