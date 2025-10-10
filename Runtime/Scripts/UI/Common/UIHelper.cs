using Crockhead.Core;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// UI 유틸리티.
	/// </summary>
	public static class UIHelper
	{
		/// <summary>
		/// 애플리케이션 생성.
		/// </summary>
		public static UIApplication CreateApplication()
		{
			if (SharedInstances.TryGet<UIApplication>(out var application))
				return application;

			application = new UIApplication();
			CreateEventSystem();
			return application;
		}

		/// <summary>
		/// 씬 생성.
		/// </summary>
		public static UIScene CreateScene()
		{
			var obj = new GameObject("UIScene");
			obj.layer = LayerMask.NameToLayer("UI");
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.transform.localEulerAngles = Vector3.zero;
			GameObject.DontDestroyOnLoad(obj);
			var scene = obj.AddComponent<UIScene>();
			return scene;
		}

		/// <summary>
		/// 윈도우 생성.
		/// </summary>
		public static UIWindow CreateWindow(Vector2Int size)
		{
			var width = size.x;
			var height = size.y;
			var obj = new GameObject("UIWindow");
			obj.layer = LayerMask.NameToLayer("UI");
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.transform.localEulerAngles = Vector3.zero;
			var canvas = obj.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.vertexColorAlwaysGammaSpace = true;
			var canvasScaler = obj.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(width, height);
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			canvasScaler.matchWidthOrHeight = width > height ? 1f : 0f; // 가로가 길면 세로 기준, 세로가 길면 가로 기준.
			canvasScaler.referencePixelsPerUnit = 100f;
			var graphicRaycaster = obj.AddComponent<GraphicRaycaster>();			
			var window = obj.AddComponent<UIWindow>();
			return window;
		}

		/// <summary>
		/// 이벤트 시스템 생성.
		/// </summary>
		public static EventSystem CreateEventSystem()
		{
			var obj = new GameObject("EventSystem");
			GameObject.DontDestroyOnLoad(obj);
			var eventSystem = obj.AddComponent<EventSystem>();
			var inputSystemUIInputModule = obj.AddComponent<InputSystemUIInputModule>();
			return eventSystem;
		}

		/// <summary>
		/// 게임 오브젝트 로드.
		/// </summary>
		public static GameObject LoadGameObject(string path, AssetPathType type)
		{
			using var assetReader = new AssetReader<GameObject>(path, type);
			assetReader.Read();
			var asset = assetReader.Result;
			if (asset == null)
				throw new FileNotFoundException(path);

			var obj = GameObject.Instantiate<GameObject>(asset);
			var name = Path.GetFileNameWithoutExtension(path);
			obj.name = name;
			return obj;
		}

		/// <summary>
		/// 뷰 생성.
		/// </summary>
		public static IUIView CreateView(string path, AssetPathType type)
		{
			try
			{
				var obj = LoadGameObject(path, type);
				var view = obj.GetComponent<IUIView>();
				if (view == null)
					throw new MissingComponentException($"[Crockhead.Unity.UI] CreateView(): Path=`{path}`, Type=`{type}`");
				return view;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				throw;
			}
		}

		/// <summary>
		/// 뷰 생성.
		/// </summary>
		public static IUIView CreateView(Type viewControllerType)
		{
			try
			{
				// UIViewController 타입 체크.
				if (!Reflections.IsBaseClass(viewControllerType, typeof(UIViewController)))
					throw new InvalidCastException($"[Crockhead.Unity.UI] CreateView(): {viewControllerType}");

				// AssetPathAttribute 특성 체크.
				if (!Reflections.TryGetAttribute<AssetPathAttribute>(viewControllerType, out var assetPathAttribute))
					throw new InvalidOperationException("[Crockhead.Unity.UI] CreateView(): Not found AssetPathAttribute.");

				var view = CreateView(assetPathAttribute.Value, assetPathAttribute.Type);
				return view;
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				throw;
			}
		}

		/// <summary>
		/// 뷰 어댑터 반환.
		/// </summary>
		internal static UIViewAdapter GetViewAdapter(this IUIView view)
		{
			if (view == null)
				return null;

			switch (view)
			{
				case UIView:
					return ((UIView)view).m_ViewAdapter;
				case UIViewBehaviour:
					return ((UIViewBehaviour)view).m_ViewAdapter;
				//case UILabel:
				//	return ((UILabel)view).m_ViewAdapter;
				//case UIButton:
				//	return ((UIButton)view).m_ViewAdapter;
				//case UIImageView:
				//	return ((UIImageView)view).m_ViewAdapter;
				//case UIGraphic:
				//	return ((UIGraphic)view).m_ViewAdapter;
				//case UISlider:
				//	return ((UISlider)view).m_ViewAdapter;
				//case UISwitch:
				//	return ((UISwitch)view).m_ViewAdapter;
				//case UIProgressView:
				//	return ((UIProgressView)view).m_ViewAdapter;
				//case UIFakeView:
				//	return ((UIFakeView)view).m_ViewAdapter;
				default:
					return null;
			}
		}

		/// <summary>
		/// 상위 뷰에 하위 뷰로 추가가 가능한지 여부.
		/// </summary>
		public static bool IsAvailableAddChild(this IUIView superview, IUIView subview)
		{
			if (superview == null || subview == null || subview == superview)
				return false;

			// 이미 현재 뷰의 하위 뷰로 존재하는 상황. (비용으로 인해 외부에서 처리)
			//if (superview.Subviews.Contains(subview))
			//	return false;

			// 하위 뷰의 조상 중에 상위 뷰가 있는지 확인.
			for (var view = subview.Superview; view != null; view = view.Superview)
			{
				if (view == superview)
					return false;
			}

			// 상위 뷰의 조상 중에 하위 뷰가 있는지 확인.
			for (var view = superview.Superview; view != null; view = superview.Superview)
			{
				if (view == subview)
					return false;
			}

			return true;
		}
	}
}