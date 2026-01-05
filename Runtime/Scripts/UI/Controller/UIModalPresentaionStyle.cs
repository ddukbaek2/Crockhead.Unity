namespace Crockhead.Unity.UI
{
	/// <summary>
	/// 모달 표시 스타일.
	/// </summary>
	public enum UIModalPresentaionStyle
	{
		/// <summary>
		/// 교체되는 식으로 표시.
		/// <para>이전 컨트롤러는 퇴장, 현재 컨트롤러는 등장. 전체 화면. 현재 컨트롤러의 배경을 투명하게 만들어도 이전 컨트롤러는 안보임.</para>
		/// </summary>
		Fullscreen,

		/// <summary>
		/// 덮이는 식으로 표시.
		/// <para>이전 컨트롤러는 방치, 현재 컨트롤러는 등장. 전체 화면. 현재 컨트롤러의 배경을 투명하게 만들면 이전 컨트롤러가 보임.</para>
		/// </summary>
		OverFullscreen,

		///// <summary>
		///// 덮이는 식으로 표시.
		///// <para>아래서 위로 올라오는 식으로 연출. 뒤에는 딤드가 깔림. 부분 화면 모달 다이얼로그 팝업. 드래그로 내려서 철회 가능.</para>
		///// </summary>
		//PageSheet,

		///// <summary>
		///// 덮이는 식으로 표시.
		///// <para>화면 중앙에 나타나는 식으로 연출. 뒤에는 딤드가 깔림. 메시지 박스같은 부분 화면 모달 다이얼로그 팝업.</para>
		///// </summary>
		//FormSheet,

		///// <summary>
		///// 덮이는 식으로 표시.
		///// <para>툴팁이나 상세팝업처럼 특정 뷰에 붙어있는 </para>
		///// </summary>
		//PopOver,

		//currentContext,
		//overCurrentContext

		//custom,
		//automatic,
		//none,
	}
}