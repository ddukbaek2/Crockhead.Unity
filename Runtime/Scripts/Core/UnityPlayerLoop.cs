using Crockhead.Core;
using Crockhead.Scripting;
using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;


namespace Crockhead.Unity
{
	/// <summary>
	/// 유니티 플레이어 루프 처리기.
	/// <para>
	/// PlayerLoop
	///		- Initialization
	///		- EarlyUpdate
	///		- FixedUpdate
	///		- PreUpdate
	///		- Update
	///			- ScriptRunBehaviourUpdate
	///				- ... MonoBehaviour.Update ...
	///			- ... Etc ...
	///			- *추가*
	///		- PreLateUpdate
	///		- LateUpdate
	///		- Rendering
	///		- PostLateUpdate
	/// </para>
	/// </summary>
	public static class UnityPlayerLoop
	{
		/// <summary>
		/// 업데이트 이벤트 콜백 추가.
		/// (현재는 UnityEngine.PlayerLoop.Update 단계에만 추가)
		/// </summary>
		public static void Add(PlayerLoopSystem.UpdateFunction updateFunction)
		{
			var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			for (var i = 0; i < currentPlayerLoop.subSystemList.Length; ++i)
			{
				var playerLoopSystem = currentPlayerLoop.subSystemList[i];
				if (playerLoopSystem.type != typeof(Update))
					continue;

				var updateLoopSystem = playerLoopSystem;
				var updateLoopList = updateLoopSystem.subSystemList;
				if (updateLoopList == null)
					updateLoopList = Array.Empty<PlayerLoopSystem>();

				Array_Add(ref updateLoopList, 1);

				var newUpdateLoop = new PlayerLoopSystem
				{
					type = typeof(UnityPlayerLoop),
					updateDelegate = updateFunction,
				};

				updateLoopList[updateLoopList.Length - 1] = newUpdateLoop;
				updateLoopSystem.subSystemList = updateLoopList;
				currentPlayerLoop.subSystemList[i] = updateLoopSystem;
				break;
			}
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		/// <summary>
		/// 업데이트 이벤트 콜백 제거.
		/// </summary>
		public static void Remove(PlayerLoopSystem.UpdateFunction updateFunction)
		{
			var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			for (var i = 0; i < currentPlayerLoop.subSystemList.Length; ++i)
			{
				var playerLoopSystem = currentPlayerLoop.subSystemList[i];
				if (playerLoopSystem.type != typeof(Update))
					continue;

				var updateLoopSystem = playerLoopSystem;
				var updateLoopList = updateLoopSystem.subSystemList;
				if (updateLoopList == null || updateLoopList.Length == 0)
					continue;

				for (var j = 0; j < updateLoopList.Length; ++j)
				{
					var updateLoop = updateLoopList[j];
					if (updateLoop.updateDelegate != updateFunction)
						continue;

					Array_RemoveAt(ref updateLoopList, j);
					updateLoopSystem.subSystemList = updateLoopList;
					currentPlayerLoop.subSystemList[i] = updateLoopSystem;
					break;
				}
				break;
			}
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		/// <summary>
		/// 배열 추가.
		/// </summary>
		private static void Array_Add(ref PlayerLoopSystem[] array, int addCount)
		{
			if (array == null)
				array = Array.Empty<PlayerLoopSystem>();

			Array.Resize(ref array, array.Length + addCount);
		}

		/// <summary>
		/// 배열 특정 요소 제거.
		/// </summary>
		private static void Array_RemoveAt(ref PlayerLoopSystem[] array, int index)
		{
			if (array == null || array.Length == 0)
				return;

			if (index < 0 || index >= array.Length)
				return;

			if (array.Length == 1)
			{
				array = Array.Empty<PlayerLoopSystem>();
				return;
			}

			var newArray = new PlayerLoopSystem[array.Length - 1];
			for (var i = 0; i < index; ++i)
			{
				newArray[i] = array[i];
			}
			for (var i = index + 1; i < array.Length; ++i)
			{
				newArray[i - 1] = array[i];
			}

			array = newArray;
		}
	}
}