namespace UnityEngineEx {
	public static class ApplicationEx {
		public static void Quit() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
