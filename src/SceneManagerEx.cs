using System;
using SystemEx;
using UnityEngine.SceneManagement;

namespace UnityEngineEx
{
	public static class SceneManagerEx
	{
		public static IDisposable SetActiveScene(Scene scene)
		{
			var currentScene = SceneManager.GetActiveScene();
			SceneManager.SetActiveScene(scene);
			return DisposableLock.Lock(() => SceneManager.SetActiveScene(currentScene));
		}
	}
}
