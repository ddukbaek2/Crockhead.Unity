using System;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 텍스쳐 유틸리티.
	/// </summary>
	public static class TextureHelper
	{
		/// <summary>
		/// 빈 컬러.
		/// </summary>
		public static readonly Color32 Clear = new Color32(0, 0, 0, 0);

		/// <summary>
		/// 텍스쳐 생성. (기본)
		/// </summary>
		public static Texture2D CreateTexture(Vector2Int size, Func<Vector2Int, Texture2D> creation, Func<Vector2Int, Color32> fill, Action<Texture2D> completion)
		{
			if (size.x <= 0 || size.y <= 0)
			{
				throw new ArgumentException(nameof(size));
			}

			if (creation == null)
			{
				throw new ArgumentNullException(nameof(creation));
			}

			if (fill == null)
			{
				throw new ArgumentNullException(nameof(fill));
			}

			if (completion == null)
			{
				throw new ArgumentNullException(nameof(completion));
			}

			// 생성.
			var texture = creation.Invoke(size);
			if (texture == null)
			{
				throw new InvalidOperationException("creation returned null.");
			}
			else if (texture.width != size.x || texture.height != size.y)
			{
				throw new InvalidOperationException("creation returned texture with mismatched size.");
			}

			// 칠하기.
			var length = size.x * size.y;
			var colors = new Color32[length];
			//for (var i = 0; i < length; ++i)
			//{
			//	colors[i] = TextureHelper.Clear;
			//}
			for (var i = 0; i < length; ++i)
			{
				var x = i % size.x;
				var y = i / size.x;
				var position = new Vector2Int(x, y);
				colors[i] = fill.Invoke(position);
			}

			// 저장.
			texture.SetPixels32(colors);
			texture.Apply();
			completion.Invoke(texture);

			return texture;
		}

		/// <summary>
		/// 내부용 텍스쳐 생성.
		/// </summary>
		private static Texture2D CreateEmptyTextureAsInternal(Vector2Int size)
		{
			var width = size.x;
			var height = size.y;
			var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
			return texture;
		}

		/// <summary>
		/// 내부용 텍스쳐 완료 클로저.
		/// </summary>
		private static void CompletionTextureAsInternal(Texture2D texture)
		{
		}

		/// <summary>
		/// 텍스쳐 생성.
		/// </summary>
		public static Texture2D CreateTexture(Vector2Int size, Func<Vector2Int, Color32> fill)
		{
			var texture = TextureHelper.CreateTexture(size, TextureHelper.CreateEmptyTextureAsInternal, fill, TextureHelper.CompletionTextureAsInternal);
			return texture;
		}
		/// <summary>
		/// 텍스쳐 생성.
		/// </summary>
		public static Texture2D CreateTexture(Vector2Int size, Func<Vector2Int, Color32> fill, Action<Texture2D> completion)
		{
			var texture = TextureHelper.CreateTexture(size, TextureHelper.CreateEmptyTextureAsInternal, fill, completion);
			return texture;
		}

		/// <summary>
		/// 기본 사각형 텍스쳐 생성.
		/// </summary>
		public static Texture2D CreateTexture(int width, int height, Color color)
		{
			Color32 OnFill(Vector2Int position)
			{
				return color;
			}

			var size = new Vector2Int(width, height);
			var texture = TextureHelper.CreateTexture(size, OnFill);
			texture.filterMode = FilterMode.Point;
			return texture;
		}

		/// <summary>
		/// 모서리가 둥근 사각형 텍스쳐 생성.
		/// </summary>
		public static Texture2D CreateRoundedTexture(int width, int height, int radius, Color color)
		{
			var r2 = radius * radius;
			Color32 OnFill(Vector2Int position)
			{
				var x = position.x;
				var y = position.y;
				var inside = true;

				// 좌측 하단.
				if (x < radius && y < radius)
				{
					var dx = x - radius;
					var dy = y - radius;
					inside = (dx * dx + dy * dy <= r2);
				}
				// 우측 하단.
				else if (x >= width - radius && y < radius)
				{
					var dx = x - (width - radius - 1);
					var dy = y - radius;
					inside = (dx * dx + dy * dy <= r2);
				}
				// 좌측 상단.
				else if (x < radius && y >= height - radius)
				{
					var dx = x - radius;
					var dy = y - (height - radius - 1);
					inside = (dx * dx + dy * dy <= r2);
				}
				// 우측 상단.
				else if (x >= width - radius && y >= height - radius)
				{
					var dx = x - (width - radius - 1);
					var dy = y - (height - radius - 1);
					inside = (dx * dx + dy * dy <= r2);
				}

				return inside ? color : TextureHelper.Clear;
			}

			var size = new Vector2Int(width, height);
			var texture = TextureHelper.CreateTexture(size, OnFill);
			texture.filterMode = FilterMode.Point;
			return texture;
		}

		/// <summary>
		/// 모서리가 부드럽게 둥근 사각형 텍스쳐 생성.
		/// </summary>
		public static Texture2D CreateSmoothRoundedTexture(int width, int height, float radius, float feather, Color color)
		{
			radius = Mathf.Min(radius, Mathf.Min(width, height) / 2);
			var hw = (width - 1) * 0.5f;
			var hh = (height - 1) * 0.5f;
			var rx = Mathf.Max(0.0001f, radius);
			var ry = rx;
			var kx = hw - rx;
			var ky = hh - ry;

			Color32 OnFill(Vector2Int position)
			{
				var px = position.x - hw;
				var py = position.y - hh;
				var ax = Mathf.Abs(px);
				var ay = Mathf.Abs(py);
				var dx = ax - kx;
				var dy = ay - ky;
				var ox = Mathf.Max(dx, 0f);
				var oy = Mathf.Max(dy, 0f);
				var outsideCorner = Mathf.Sqrt(ox * ox + oy * oy);
				var insideEdge = Mathf.Min(Mathf.Max(dx, dy), 0f);
				var distance = outsideCorner + insideEdge - rx;
				var a = 1f - Mathf.Clamp01(distance / Mathf.Max(feather, 1e-5f));
				a = Mathf.Clamp01(a);
				a = a * a * (3f - 2f * a);
				var c = color;				
				c.a = c.a *= a;
				return c.a > 0.001f ? (Color32)c : TextureHelper.Clear;
			}

			var size = new Vector2Int(width, height);
			var texture = TextureHelper.CreateTexture(size, OnFill);
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Bilinear;
			return texture;
		}
	}
}