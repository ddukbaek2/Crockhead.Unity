using Crockhead.Core;
using System.Text;
using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 트랜스폼 경로.
	/// <para></para>
	/// </summary>
	public class TransformPath : Disposable
	{
		/// <summary>
		/// 문자열 생성기.
		/// </summary>
		private static StringBuilder s_TemporaryStringBuilder = new StringBuilder();

		/// <summary>
		/// 경로의 주체.
		/// </summary>
		private Transform m_Start;

		/// <summary>
		/// 주체에 기반한 상대 경로.
		/// </summary>
		private string m_RelativePath;

		/// <summary>
		/// 출발 트랜스폼 프로퍼티. (경로의 주체)
		/// </summary>
		public Transform Start => m_Start;

		/// <summary>
		/// 도착 트랜스폼 프로퍼티.
		/// </summary>
		public Transform Destination => TransformPath.GetDestinationTransform(AbsolutePath);

		/// <summary>
		/// 출발 트랜스폼을 제외한 나머지 상대 경로 프로퍼티.
		/// </summary>
		public string RelativePath => m_RelativePath;

		/// <summary>
		/// 절대 경로 프로퍼티.
		/// </summary>
		public string AbsolutePath
		{
			get
			{
				var rootPath = TransformPath.GetTransformPath(Start);
				var absolutePath = $"{rootPath}/{m_RelativePath}";
				return absolutePath;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public TransformPath() : base()
		{
			m_Start = null;
			m_RelativePath = string.Empty;
		}

		/// <summary>
		/// 생성됨. (주체 + 상대경로)
		/// </summary>
		public TransformPath(Transform root, string transformPath) : this()
		{
			m_Start = root;
			m_RelativePath = transformPath;
		}

		/// <summary>
		/// 생성됨. (절대경로)
		/// </summary>
		public TransformPath(string transformPath) : this()
		{
			m_Start = TransformPath.GetStartTransform(transformPath);
			var rootPath = TransformPath.GetTransformPath(m_Start);
			m_RelativePath = transformPath.Remove(0, rootPath.Length);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 문자열 변환. (절대경로)
		/// </summary>
		public override string ToString()
		{
			return AbsolutePath;
		}

		/// <summary>
		/// 절대 경로가 가리키는 출발 트랜스폼 반환.
		/// </summary>
		public static Transform GetStartTransform(string transformPath)
		{
			if (string.IsNullOrWhiteSpace(transformPath))
				return null;

			var paths = transformPath.Split('/', System.StringSplitOptions.RemoveEmptyEntries);
			var startPath = paths[0];
			var obj = GameObject.Find(startPath);
			if (obj == null)
				return null;

			var current = obj.transform;
			return current;
		}

		/// <summary>
		/// 절대 경로가 가리키는 도착 트랜스폼 반환.
		/// </summary>
		public static Transform GetDestinationTransform(string transformPath)
		{
			if (string.IsNullOrWhiteSpace(transformPath))
				return null;

			var paths = transformPath.Split('/', System.StringSplitOptions.RemoveEmptyEntries);
			var startPath = paths[0];
			var obj = GameObject.Find(startPath);
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
		/// 절대 경로 반환.
		/// </summary>
		public static string GetTransformPath(Transform target)
		{
			if (target == null)
				return string.Empty;

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

	}
}