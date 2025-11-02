using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Crockhead.Unity.Editor
{
	/// <summary>
	/// 프로젝트 애플리케이션 패키징 클래스.
	/// </summary>
	public static class Builder
	{
		/// <summary>
		/// 빌드 이름. (앱이름_v버전)
		/// </summary>
		public static string BuildName
		{
			get
			{
				var buildName = $"{Application.productName}_v{Application.version}";
				return buildName;
			}
		}

		/// <summary>
		/// 빌드 저장 경로.
		/// </summary>
		public static string GetLocationPathName(BuildTarget buildTarget)
		{
			switch (buildTarget)
			{
				case BuildTarget.Android:
					{
						var locationPathName = $"Build/{Builder.BuildName}.aab";
						return locationPathName;
					}

				default:
					{
						var locationPathName = $"Build/{Builder.BuildName}";
						return locationPathName;
					}
			}
		}

		/// <summary>
		/// 안드로이드 빌드.
		/// </summary>
		public static void BuildForAndroid()
		{
			var enabledScenes = new List<string>();
			foreach (var scene in EditorBuildSettings.scenes)
			{
				if (!scene.enabled)
					continue;

				enabledScenes.Add(scene.path);
			}

			var levels = enabledScenes.ToArray();
			var buildTarget = BuildTarget.Android;
			var buildOptions = BuildOptions.None;
			var locationPathName = Builder.GetLocationPathName(buildTarget);
			var buildReport = BuildPipeline.BuildPlayer(levels, locationPathName, buildTarget, buildOptions);
		}
	}
}