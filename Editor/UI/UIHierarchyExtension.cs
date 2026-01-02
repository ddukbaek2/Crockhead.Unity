using Crockhead.Unity.Editor;
using TMPro;
using UnityEditor;
using UnityEngine;


namespace Crockhead.Unity.UI.Editor
{
	/// <summary>
	/// UI 하이어라키 확장.
	/// </summary>
	public class UIHierarchyExtension : HierarchyExtension
	{
		///// <summary>
		///// UIView 생성.
		///// </summary>
		//[MenuItem("GameObject/UI/Crockhead/UIView", priority = 0)]
		//public static void CreateUIViewToHierarchy(MenuCommand menuCommand)
		//{
		//	// 부모 설정.
		//	var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

		//	// 뷰 생성.
		//	var view = HierarchyExtension.CreateComponentToHierarchy<UIView>("UIView", parentTransform);

		//	// 트랜스폼 설정.
		//	var gameObject = view.gameObject;
		//	//view.RectTransform.ResetRectTransform();

		//	// 선택 처리.
		//	Selection.activeGameObject = gameObject;
		//	EditorGUIUtility.PingObject(gameObject);
		//}

		/// <summary>
		/// UILabel 생성.
		/// </summary>
		[MenuItem("GameObject/UI/Crockhead/UILabelView", priority = 1)]
		public static void CreateUILabelViewToHierarchy(MenuCommand menuCommand)
		{
			// 부모 설정.
			var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

			// 라벨 생성.
			var label = HierarchyExtension.CreateComponentToHierarchy<UILabelView>("UILabelView", parentTransform);

			// 트랜스폼 설정.
			var gameObject = label.gameObject;
			//view.RectTransform.ResetRectTransform();

			// 폰트 설정.
			Undo.RecordObject(label, "SetFont");
			//label.font = Resources.Load<TMP_FontAsset>("Font/KoPubWorld Dotum Light SDF");
			label.horizontalAlignment = HorizontalAlignmentOptions.Center;
			label.verticalAlignment = VerticalAlignmentOptions.Middle;

			// 텍스트 수정.
			Undo.RecordObject(label, "SetText");
			label.text = "New UILabelView";
			EditorUtility.SetDirty(label);

			// 선택 처리.
			Selection.activeGameObject = gameObject;
			EditorGUIUtility.PingObject(gameObject);
		}

		/// <summary>
		/// UIImageView 생성.
		/// </summary>
		[MenuItem("GameObject/UI/Crockhead/UIImageView", priority = 2)]
		public static void CreateUIImageViewToHierarchy(MenuCommand menuCommand)
		{
			// 부모 설정.
			var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

			// 이미지뷰 생성.
			var imageview = HierarchyExtension.CreateComponentToHierarchy<UIImageView>("UIImageView", parentTransform);

			// 트랜스폼 설정.
			var gameObject = imageview.gameObject;
			//imageview.RectTransform.ResetRectTransform();

			// 이미지 설정.
			Undo.RecordObject(imageview, "SetImage");
			imageview.sprite = null;// Resources.LoadFromFile<TMP_FontAsset>("Font/KoPubWorld Dotum Light SDF");
			UnityEditor.EditorUtility.SetDirty(imageview);

			// 선택 처리.
			Selection.activeGameObject = gameObject;
			EditorGUIUtility.PingObject(gameObject);
		}

		///// <summary>
		///// UIProgressView 생성.
		///// </summary>
		//[MenuItem("GameObject/UI/Crockhead/UIProgressView", priority = 3)]
		//public static void CreateUIProgressViewToHierarchy(MenuCommand menuCommand)
		//{
		//	// 부모 설정.
		//	var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

		//	// 프로그레스뷰 생성.
		//	var progressview = HierarchyExtension.CreateComponentToHierarchy<UIProgressView>("UIProgressView", parentTransform);

		//	// 트랜스폼 설정.
		//	var gameObject = progressview.gameObject;
		//	//progressview.RectTransform.ResetRectTransform();

		//	// 설정.
		//	//Undo.RecordObject(progressview, "SetTexture");
		//	progressview.interactable = false;
		//	progressview.transition = Selectable.TransitionAsync.None;
		//	UnityEditor.EditorUtility.SetDirty(progressview);

		//	// 이미지뷰 생성.
		//	var backgroundImageview = HierarchyExtension.CreateComponentToHierarchy<UIImageView>("Background", progressview.transform);
		//	backgroundImageview.RectTransform.anchorMin = Vector2.zero;
		//	backgroundImageview.RectTransform.anchorMax = Vector2.one;
		//	backgroundImageview.RectTransform.anchoredPosition = Vector2.zero;
		//	backgroundImageview.RectTransform.sizeDelta = Vector2.zero;
		//	backgroundImageview.color = Color.black;
		//	var fillImageview = HierarchyExtension.CreateComponentToHierarchy<UIImageView>("Fill", progressview.transform);
		//	fillImageview.RectTransform.anchorMin = Vector2.zero;
		//	fillImageview.RectTransform.anchorMax = Vector2.one;
		//	fillImageview.RectTransform.anchoredPosition = Vector2.zero;
		//	fillImageview.RectTransform.sizeDelta = Vector2.zero;
		//	fillImageview.color = Color.white;
		//	progressview.fillRect = fillImageview.RectTransform;

		//	// 선택 처리.
		//	Selection.activeGameObject = gameObject;
		//	EditorGUIUtility.PingObject(gameObject);
		//}

		///// <summary>
		///// UISlider 생성.
		///// </summary>
		//[MenuItem("GameObject/UI/Crockhead/UISlider", priority = 4)]
		//public static void CreateUISliderToHierarchy(MenuCommand menuCommand)
		//{
		//	// 부모 설정.
		//	var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

		//	// 슬라이더 생성.
		//	var slider = HierarchyExtension.CreateComponentToHierarchy<UISlider>("UISlider", parentTransform);

		//	// 트랜스폼 설정.
		//	var gameObject = slider.gameObject;
		//	//progressview.RectTransform.ResetRectTransform();

		//	//// 이미지 설정.
		//	//Undo.RecordObject(progressview, "SetTexture");
		//	//progressview.progressview = null;
		//	//UnityEditor.EditorUtility.SetDirty(progressview);

		//	// 선택 처리.
		//	Selection.activeGameObject = gameObject;
		//	EditorGUIUtility.PingObject(gameObject);
		//}

		///// <summary>
		///// UIGraphic 생성.
		///// </summary>
		//[MenuItem("GameObject/UI/Crockhead/Unity/UIGraphic")]
		//public static void CreateUIGraphicToHierarchy(MenuCommand menuCommand)
		//{
		//	// 부모 설정.
		//	var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

		//	// 그래픽 생성.
		//	var graphic = HierarchyExtension.CreateComponentToHierarchy<UIGraphic>("UIGraphic", parentTransform);

		//	// 트랜스폼 설정.
		//	var gameObject = graphic.gameObject;
		//	//progressview.RectTransform.ResetRectTransform();

		//	//// 이미지 설정.
		//	//Undo.RecordObject(progressview, "SetTexture");
		//	//progressview.progressview = null;
		//	//EditorUtility.SetDirty(progressview);

		//	// 선택 처리.
		//	Selection.activeGameObject = gameObject;
		//	EditorGUIUtility.PingObject(gameObject);
		//}

		/// <summary>
		/// UITexture 생성.
		/// </summary>
		[MenuItem("GameObject/UI/Crockhead/UITextureView")]
		public static void CreateUITextureViewToHierarchy(MenuCommand menuCommand)
		{
			// 부모 설정.
			var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

			// 텍스쳐 생성.
			var texture = HierarchyExtension.CreateComponentToHierarchy<UITextureView>("UITextureView", parentTransform);

			// 트랜스폼 설정.
			var gameObject = texture.gameObject;
			//progressview.RectTransform.ResetRectTransform();

			// 이미지 설정.
			Undo.RecordObject(texture, "SetTexture");
			texture.texture = null;
			EditorUtility.SetDirty(texture);

			// 선택 처리.
			Selection.activeGameObject = gameObject;
			EditorGUIUtility.PingObject(gameObject);
		}

		/// <summary>
		/// UIButton 생성.
		/// </summary>
		[MenuItem("GameObject/UI/Crockhead/UIButtonView", priority = 5)]
		public static void CreateUIButtonViewToHierarchy(MenuCommand menuCommand)
		{
			// 부모 설정.
			var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

			// 버튼 생성.
			var button = HierarchyExtension.CreateComponentToHierarchy<UIButtonView>("UIButtonView", parentTransform);

			// 트랜스폼 설정.
			var gameObject = button.gameObject;
			//switch.RectTransform.ResetRectTransform();

			// 이미지 생성.
			var image = Undo.AddComponent<UIImageView>(gameObject);

			// 이미지 설정.
			Undo.RecordObject(button, "SetTargetGraphic");
			button.image = image;
			EditorUtility.SetDirty(button);

			// 라벨 생성.
			var label = HierarchyExtension.CreateComponentToHierarchy<UILabelView>("UILabelView", button.transform);

			// 트랜스폼 설정.
			//view.RectTransform.ResetRectTransform();

			// 폰트 설정.
			Undo.RecordObject(label, "SetFont");
			//label.font = Resources.Load<TMP_FontAsset>("Font/KoPubWorld Dotum Light SDF");
			label.horizontalAlignment = HorizontalAlignmentOptions.Center;
			label.verticalAlignment = VerticalAlignmentOptions.Middle;

			// 텍스트 수정.
			Undo.RecordObject(label, "SetText");
			label.text = "New UIButton";
			EditorUtility.SetDirty(label);

			// 선택 처리.
			Selection.activeGameObject = gameObject;
			EditorGUIUtility.PingObject(gameObject);
		}

		///// <summary>
		///// UISwitch 생성.
		///// </summary>
		//[MenuItem("GameObject/UI/Crockhead/UISwitchView", priority = 6)]
		//public static void CreateUISwitchToHierarchy(MenuCommand menuCommand)
		//{
		//	// 부모 설정.
		//	var parentTransform = HierarchyExtension.GetSelectionTransform<Transform>(menuCommand);

		//	// 스위치 생성.
		//	var @switch = HierarchyExtension.CreateComponentToHierarchy<UISwitch>("UISwitch", parentTransform);

		//	// 트랜스폼 설정.
		//	var gameObject = @switch.gameObject;
		//	//switch.RectTransform.ResetRectTransform();

		//	// 이미지 생성.
		//	var image = Undo.AddComponent<UIImageView>(gameObject);

		//	// 이미지 설정.
		//	Undo.RecordObject(@switch, "SetTargetGraphic");
		//	@switch.image = image;
		//	EditorUtility.SetDirty(@switch);

		//	// 라벨 생성.
		//	var label = HierarchyExtension.CreateComponentToHierarchy<UILabelView>("UILabelView", @switch.transform);

		//	// 트랜스폼 설정.
		//	//view.RectTransform.ResetRectTransform();

		//	// 폰트 설정.
		//	Undo.RecordObject(label, "SetFont");
		//	label.font = Resources.Load<TMP_FontAsset>("Font/KoPubWorld Dotum Light SDF");
		//	label.horizontalAlignment = HorizontalAlignmentOptions.Center;
		//	label.verticalAlignment = VerticalAlignmentOptions.Middle;

		//	// 텍스트 수정.
		//	Undo.RecordObject(label, "SetText");
		//	label.text = "New UIButton";
		//	UnityEditor.EditorUtility.SetDirty(label);

		//	// 선택 처리.
		//	Selection.activeGameObject = gameObject;
		//	EditorGUIUtility.PingObject(gameObject);
		//}
	}
}
