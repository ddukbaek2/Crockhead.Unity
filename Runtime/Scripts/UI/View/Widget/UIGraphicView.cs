using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 터치가 가능한 자유 렌더링 영역 뷰.
	/// </summary>
	[RequireComponent(typeof(CanvasRenderer))]
	public class UIGraphicView : MaskableGraphic, IUIView, IPointerClickHandler
	{
		/// <summary>
		/// 텍스쳐 오프셋. (CW)
		/// </summary>
		public readonly static Vector2[] UV = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
		};


		#region INSPECTOR
		[SerializeField] private RectTransform m_RectTransform;
		#endregion

		/// <summary>
		/// 렉트 트랜스폼 프로퍼티.
		/// </summary>
		public RectTransform RectTransform { get; private set; }

		/// <summary>
		/// 클릭 이벤트 프로퍼티.
		/// </summary>
		public Action<PointerEventData> OnClickEvent { set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		protected sealed override void Awake()
		{
			base.Awake();

			if (m_RectTransform == null)
			{
				m_RectTransform = GetComponent<RectTransform>();
			}

			OnCreate();
		}

		/// <summary>
		/// 초기화됨.
		/// </summary>
		protected sealed override void Start()
		{
			base.Start();

			OnInitialize();
		}

		/// <summary>
		/// 파괴됨/
		/// </summary>
		protected sealed override void OnDestroy()
		{
			OnDispose();

			base.OnDestroy();
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
		/// 메쉬 셋팅됨
		/// </summary>
		protected override void OnPopulateMesh(VertexHelper vertexHelper)
		{
			// 기본: 메시를 그리지 않음.
			vertexHelper.Clear();
		}

		/// <summary>
		/// 클릭됨.
		/// </summary>
		void IPointerClickHandler.OnPointerClick(PointerEventData pointerEventData)
		{
			OnClickEvent?.Invoke(pointerEventData);
		}

		/// <summary>
		/// 사각형 추가. (LB < LT < RT < RB)
		/// </summary>
		protected void AddQuad(VertexHelper vertexHelper, Vector3[] position)
		{
			UIGraphicView.AddQuad(vertexHelper, position, UIGraphicView.UV, color);
		}

		/// <summary>
		/// 사각형 추가. (LB < LT < RT < RB)
		/// </summary>
		protected static void AddQuad(VertexHelper vertexHelper, Vector3[] position, Vector2[] uv, Color color)
		{
			if (position == null || position.Length < 4)
				throw new ArgumentException(nameof(position));

			if (uv == null)
				uv = UIGraphicView.UV;

			// 정점 추가.
			var vertexStartIndex = vertexHelper.currentVertCount;
			var vertex = UIVertex.simpleVert;
			vertex.color = color;

			for (var i = 0; i < position.Length; ++i)
			{
				vertex.position = position[i];
				vertex.uv0 = uv[i];

				vertexHelper.AddVert(vertex);
			}
	
			// 삼각형 인덱스 추가.
			vertexHelper.AddTriangle(vertexStartIndex + 0, vertexStartIndex + 1, vertexStartIndex + 2);
			vertexHelper.AddTriangle(vertexStartIndex + 0, vertexStartIndex + 2, vertexStartIndex + 3);
		}
	}
}