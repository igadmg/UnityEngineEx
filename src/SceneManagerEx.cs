using System;
using SystemEx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityEngineEx
{
	public static class SceneManagerEx
	{
		public static IDisposable SetActiveScene(Scene scene)
		{
			var currentScene = SceneManager.GetActiveScene();
			SceneManager.SetActiveScene(scene);
			return DisposableLock.Lock(() => { try { SceneManager.SetActiveScene(currentScene); } catch (Exception e) { Debug.LogException(e); } });
		}

		public static IDisposable SaveActiveScene()
		{
			var currentScene = SceneManager.GetActiveScene();
			return DisposableLock.Lock(() => { try { SceneManager.SetActiveScene(currentScene); } catch (Exception e) { Debug.LogException(e); } });
		}

#if UNITY_EDITOR
		public static Scene CreateScene(string sceneName, CreateSceneParameters parameters)
		{
			if (UnityEditor.EditorApplication.isPlaying)
			{
				return SceneManager.CreateScene(sceneName, new CreateSceneParameters { localPhysicsMode = LocalPhysicsMode.Physics3D });
			}
			else
			{
				using (SaveActiveScene())
				{
					var localScene = SceneManager.GetSceneByName(sceneName);
					localScene = localScene.isLoaded ? localScene : UnityEditor.SceneManagement.EditorSceneManager.NewScene(UnityEditor.SceneManagement.NewSceneSetup.EmptyScene, UnityEditor.SceneManagement.NewSceneMode.Additive);
					localScene.name = sceneName;
					return localScene;
				}
			}
		}
#else
		public static Scene CreateScene(string sceneName, CreateSceneParameters parameters)
		{
			return SceneManager.CreateScene(sceneName, new CreateSceneParameters { localPhysicsMode = LocalPhysicsMode.Physics3D });
		}
#endif

#if UNITY_EDITOR
		public static void UnloadSceneAsync(Scene scene)
		{
			if (UnityEditor.EditorApplication.isPlaying)
			{
				SceneManager.UnloadSceneAsync(scene);
			}
			else
			{
				UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
			}
		}
#else
		public static void UnloadSceneAsync(Scene scene)
		{
			SceneManager.UnloadSceneAsync(scene);
		}
#endif
	}
}
