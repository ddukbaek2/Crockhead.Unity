using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Crockhead.Unity.Editor
{
	/// <summary>
	/// 프로젝트 빌드 클래스.
	/// </summary>
	public static class Builder
	{
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
			var locationPathName = $"Build/{Application.productName}_v{Application.version}.aab";
			var buildTarget = BuildTarget.Android;
			var buildOptions = BuildOptions.None;
			var buildReport = BuildPipeline.BuildPlayer(levels, locationPathName, buildTarget, buildOptions);
		}
	}
}