using System.IO;
using UnityEditor;
using UnityEngine;


namespace Crockhead.Unity.Editor
{
	/// <summary>
	/// 프로젝트 유틸리티 클래스.
	/// </summary>
	public static class Projects
	{
		/// <summary>
		/// 역슬래시.
		/// </summary>
		public const char BackSlash = '\\';

		/// <summary>
		/// 슬래시.
		/// </summary>
		public const char Slash = '/';

		/// <summary>
		/// 프로젝트 루트 디렉토리.
		/// </summary>
		public static string RootDirectory
		{
			get
			{
				var path = Path.GetDirectoryName(Application.dataPath);
				path = path.Replace(Projects.BackSlash, Projects.Slash);
				return path;
			}
		}

		/// <summary>
		/// 엑셀 디렉토리.
		/// </summary>
		public static string ExcelDirectory
		{
			get
			{
				return $"{Projects.RootDirectory}/Excel";
			}
		}

		/// <summary>
		/// 유니티 루트 디렉토리.
		/// </summary>
		public static string AssetsDirectory
		{
			get
			{
				return $"{Projects.RootDirectory}/Assets";
			}
		}

		/// <summary>
		/// 플러그인 디렉토리.
		/// </summary>
		public static string PluginsDirectory
		{
			get
			{
				return $"{Projects.AssetsDirectory}/Plugins";
			}
		}

		/// <summary>
		/// 리소스 디렉토리.
		/// </summary>
		public static string ResourcesDirectory
		{
			get
			{
				return $"{Projects.AssetsDirectory}/Resources";
			}
		}

		/// <summary>
		/// 스크립트 디렉토리.
		/// </summary>
		public static string ScriptsDirectory
		{
			get
			{
				return $"{Projects.AssetsDirectory}/Scripts";
			}
		}

		/// <summary>
		/// 현재 선택된 애셋의 디렉토리 경로를 반환.
		/// <para>현재 선택된 애셋이 디렉토리가 아니면 그 부모 디렉토리가 있을 경우 해당 디렉토리를 반환.</para>
		/// </summary>
		public static string GetSelectedAssetDirectory()
		{
			var obj = Selection.activeObject;
			var path = AssetDatabase.GetAssetPath(obj);

			if (string.IsNullOrWhiteSpace(path))
				return string.Empty;

			if (Directory.Exists(path))
				return path;

			path = Path.GetDirectoryName(path);
			if (string.IsNullOrWhiteSpace(path))
				return string.Empty;

			path = path.Replace(Projects.BackSlash, Projects.Slash);
			return path;
		}

		/// <summary>
		/// 애셋 경로를 파일 경로로 변환하여 반환.
		/// </summary>
		public static string GetFilePath(string assetPath)
		{
			var filePath = Path.GetFullPath(assetPath);
			filePath = filePath.Replace(Projects.BackSlash, Projects.Slash);
			return filePath;
		}
	}
}