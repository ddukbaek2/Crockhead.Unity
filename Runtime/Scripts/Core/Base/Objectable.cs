using Crockhead.Core;
using System;
using System.Threading.Tasks;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 기반 컴포넌트.
	/// </summary>
	public abstract class Objectable : MonoBehaviour
	{
		#region INSPECTOR
		#endregion

		/// <summary>
		/// 컴포넌트 타입의 이름 프로퍼티.
		/// </summary>
		public string ComponentTypeName { private set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void Awake()
		{
			var type = GetType();
			ComponentTypeName = type.Name;
			Debug.Log($"[{ComponentTypeName}] Awake()");

			OnCreate();
		}

		/// <summary>
		/// 시작됨.
		/// </summary>
		protected virtual void Start()
		{
			Debug.Log($"[{ComponentTypeName}] OnPrepare()");

			OnInitialize();
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		protected virtual void OnDestroy()
		{
			Debug.Log($"[{ComponentTypeName}] OnDestroy()");

			OnDispose();
		}

		/// <summary>
		/// 활성화됨.
		/// </summary>
		protected virtual void OnEnable()
		{
			Debug.Log($"[{ComponentTypeName}] OnEnable()");
		}

		/// <summary>
		/// 비활성화됨.
		/// </summary>
		protected virtual void OnDisable()
		{
			Debug.Log($"[{ComponentTypeName}] OnDisable()");
		}

		/// <summary>
		/// 재시작.
		/// </summary>
		protected virtual void Reset()
		{
			Debug.Log($"[{ComponentTypeName}] Reset()");
		}

		//protected virtual void OnRectTransformDimensionsChange()
		//{
		//	Debug.Log($"[{componentType.Name}] OnRectTransformDimensionsChange()");
		//}

		protected virtual void OnBeforeTransformParentChanged()
		{
			Debug.Log($"[{ComponentTypeName}] OnBeforeTransformParentChanged()");
		}

		protected virtual void OnTransformParentChanged()
		{
			Debug.Log($"[{ComponentTypeName}] OnTransformParentChanged()");
		}

		//protected virtual void OnCanvasGroupChanged()
		//{
		//	Debug.Log($"[{componentType.Name}] OnCanvasGroupChanged()");
		//}

		//protected virtual void OnCanvasHierarchyChanged()
		//{
		//	Debug.Log($"[{componentType.Name}] OnCanvasHierarchyChanged()");
		//}

		protected virtual void OnDidApplyAnimationProperties()
		{
			Debug.Log($"[{ComponentTypeName}] OnDidApplyAnimationProperties()");
		}

		/// <summary>
		/// 유효성 체크.
		/// </summary>
		protected virtual void OnValidate()
		{
			Debug.Log($"[{ComponentTypeName}] OnValidate()");
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected virtual void OnCreate()
		{
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected virtual void OnInitialize()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected virtual void OnDispose()
		{
		}

		/// <summary>
		/// 현재 객체가 파괴 되었는지 여부.
		/// </summary>
		public bool IsDestroyed()
		{
			if (this == null)
				return true;
			return false;
		}

		/// <summary>
		/// 활성화 여부.
		/// </summary>
		public virtual bool IsActive()
		{
			return isActiveAndEnabled;
		}

		/// <summary>
		/// 자식 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public TComponent GetOrAddComponent<TComponent>() where TComponent : Component
		{
			var component = GetOrAddComponent<TComponent>(string.Empty);
			return component;
		}

		/// <summary>
		/// 자식 트랜스폼에 대한 컴포넌트 반환 혹은 생성 후 반환.
		/// </summary>
		public TComponent GetOrAddComponent<TComponent>(string transformPath) where TComponent : Component
		{
			var component = TransformHelper.GetOrAddComponent<TComponent>(transform, transformPath);
			return component;
		}
	}
}