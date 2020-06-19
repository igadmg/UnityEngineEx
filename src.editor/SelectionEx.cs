using System;
using SystemEx;
using UnityEditor;
using UnityEngine;

namespace UnityEngineEx
{
	public static class SelectionEx
	{
		public static IDisposable SelectGameObject<T>(this T c) where T : Component
			=> SelectGameObject(c.gameObject);

		public static IDisposable SelectGameObject(this GameObject go)
		{
			var ago = Selection.activeGameObject;
			Selection.activeGameObject = go;
			return DisposableLock.Lock(() => {
				Selection.activeGameObject = ago;
			});
		}
	}
}
