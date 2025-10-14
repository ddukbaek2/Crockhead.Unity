using Crockhead.Core;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 유틸리티.
	/// </summary>
	public static class UIHelper
	{
		/// <summary>
		/// 임시 문자열 생성기.
		/// </summary>
		private static StringBuilder s_TemporaryStringBuilder = new StringBuilder();

		/// <summary>
		/// 트랜스폼 경로의 대상 반환.
		/// </summary>
		public static Transform GetTransform(string transformPath)
		{
			if (string.IsNullOrWhiteSpace(transformPath))
				return null;

			var paths = transformPath.Split('/', System.StringSplitOptions.RemoveEmptyEntries);
			var frontPath = paths[0];
			var obj = GameObject.Find(frontPath);
			if (obj == null)
				return null;

			var current = obj.transform;
			for (var i = 1; i < paths.Length; ++i)
			{
				var path = paths[i];
				current = current.Find(path);
				if (current == null)
					return null;
			}

			return current;
		}

		/// <summary>
		/// 트랜스폼 경로 반환.
		/// </summary>
		public static string GetTransformPath(Transform target)
		{
			if (target == null)
				return string.Empty; // "/"

			var current = target;
			s_TemporaryStringBuilder.Clear();
			s_TemporaryStringBuilder.Append(current.name);

			while (current.parent != null)
			{
				current = current.parent;
				s_TemporaryStringBuilder.Insert(0, $"/{current.name}");
			}

			var transformPath = s_TemporaryStringBuilder.ToString();
			return transformPath;
		}

		/// <summary>
		/// UIKitLite 컴포넌트 파괴 여부.
		/// </summary>
		public static bool IsDestroyed(UIBehaviour behaviour)
		{
			return behaviour == null || behaviour.IsDestroyed();
		}

		/// <summary>
		/// 캔버스 뷰 생성.
		/// </summary>
		public static UICanvasView CreateCanvasView(Vector2Int size)
		{
			var width = size.x;
			var height = size.y;
			var obj = new GameObject("UICanvasView");
			obj.layer = LayerMask.NameToLayer("UIKitLite");
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
			var window = obj.AddComponent<UICanvasView>();
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
				throw new UIAssetNotFoundException(path);

			var obj = GameObject.Instantiate<GameObject>(asset);
			var name = Path.GetFileNameWithoutExtension(path);
			obj.name = name;
			return obj;
		}


		/// <summary>
		/// 뷰 생성.
		/// </summary>
		public static UIView CreateView(string path, AssetPathType type)
		{
			try
			{
				var obj = LoadGameObject(path, type);
				var view = obj.GetComponent<UIView>();
				if (view == null)
					throw new MissingComponentException($"[Crockhead.Unity.UIKitLite] CreateView(): Path=`{path}`, Type=`{type}`");
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
		public static UIView CreateView(Type controllerType)
		{
			try
			{
				// UIController 타입 체크.
				if (!Reflections.IsBaseClass(controllerType, typeof(UIController)))
					throw new InvalidCastException($"[Crockhead.Unity.UI] CreateView(): {controllerType}");

				// AssetPathAttribute 특성 체크.
				if (!Reflections.TryGetAttribute<AssetPathAttribute>(controllerType, out var assetPathAttribute))
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
		/// 뷰 생성.
		/// </summary>
		public static TUIView CreateView<TUIView>(Type controllerType) where TUIView : UIView
		{
			try
			{
				var view = UIHelper.CreateView(controllerType);
				return (TUIView)view;
			}
			catch
			{
				throw;
			}
		}
	}
}