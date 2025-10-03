using Crockhead.Core;
using System;
using System.Text.RegularExpressions;
using DotNetPath = System.IO.Path;


namespace Crockhead.Unity
{
	/// <summary>
	/// 애셋 경로 유틸리티.
	/// </summary>
	public static class AssetPaths
	{
		/// <summary>
		/// 슬래시.
		/// </summary>
		public const char Slash = '/';

		/// <summary>
		/// 백슬래시.
		/// </summary>
		public const char BackSlash = '\\';

		/// <summary>
		/// 컴포넌트로부터 바인딩된 프리팹 경로 반환.
		/// </summary>
		public static bool TryGetAssetPath(Type targetType, out AssetPathType type, out string value)
		{
			if (Reflections.TryGetAttribute<AssetPathAttribute>(targetType, out var attribute))
			{
				type = attribute.Type;
				value = attribute.Value;
				return true;
			}
			else
			{
				type = AssetPathType.None;
				value = string.Empty;
				return false;
			}
		}

		/// <summary>
		/// 값으로 애셋 경로 타입을 추정하여 반환.
		/// </summary>
		public static AssetPathType GetInferAssetPathType(string value)
		{
			var assetPathType = AssetPathType.None;

			// 값이 없다면 타입도 없다.
			if (string.IsNullOrWhiteSpace(value))
			{
				assetPathType = AssetPathType.None;
			}
			// 리소스 디렉토리 안에 있다는 것은 유니티 디렉토리 룰 기준으로 앱 내장이라는 것.
			// 따라서 리소스 타입으로 간주한다.
			else if (value.Contains("Resources/"))
			{
				assetPathType = AssetPathType.Resources;
			}
			// 루트 디렉토리가 없다는 것은 번들타입에서의 번들 키 규칙인 `Assets/Bundles/....` 네이밍 에서 벗어난다는 것.
			// 따라서 리소스 타입으로 간주한다.
			else if (!value.Contains("Assets/"))
			{
				assetPathType = AssetPathType.Resources;
			}
			// 그 외는 번들 타입.
			else
			{
				assetPathType = AssetPathType.Addressables;
			}

			 return assetPathType;
		}

		/// <summary>
		/// 리소스 경로 양식에 맞게 경로를 변환하여 반환.
		/// </summary>
		public static string GetResourcePath(string path)
		{
			// 빈 문자열일 경우.
			if (string.IsNullOrWhiteSpace(path))
				return string.Empty;

			// 확장자 제거.
			// Path.ChangeExtension() 함수의 규칙이 이상해서 `string.Empty`를 넣으면 뒤에 `.`을 붙임.
			// null을 넣어야만 완벽히 확장자를 제거함.
			path = DotNetPath.ChangeExtension(path, null);

			// 경로 구분자를 슬래시로 대체.
			path = path.Replace(AssetPaths.BackSlash, AssetPaths.Slash);

			// 제거 할 `/Resources/` 까지의 경로 찾기.
			var match = Regex.Match(path, "(^|/)Resources(/|$)", RegexOptions.IgnoreCase);
			if (!match.Success)
				return path;

			// 제거 할 경로 문자열 길이 구하기.
			var startIndex = match.Index + match.Length;
			if (startIndex > path.Length)
				return string.Empty;

			// 제거 할 경로 문자열 제거.
			path = path.Substring(startIndex);
			return path;
		}
	}
}