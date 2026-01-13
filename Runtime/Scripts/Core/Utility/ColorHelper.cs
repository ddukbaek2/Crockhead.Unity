using UnityEngine;


namespace Crockhead.Unity
{
	/// <summary>
	/// 색상 유틸리티.
	/// </summary>
	public static class ColorHelper
	{
		/// <summary>
		/// 무작위 색상 반환.
		/// </summary>
		public static Color GetRandomColor()
		{
			var red = (byte)Random.Range(0, 255);
			var green = (byte)Random.Range(0, 255);
			var blue = (byte)Random.Range(0, 255);

			var color = new Color32(red, green, blue, 255);
			return color;
		}
	}
}