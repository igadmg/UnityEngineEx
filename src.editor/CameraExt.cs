using UnityEditor;
using UnityEngine;

namespace UnityEditorEx
{
	public class CameraExt
	{
		[MenuItem("GameObject/Align View with Main camera %&m", true)]
		private static bool AlignViewWithMainCameraValidate()
			=> Camera.main != null;

		[MenuItem("GameObject/Align View with Main camera %&m", false)]
		private static void AlignViewWithMainCamera()
			=> SceneView.lastActiveSceneView.AlignViewToObject(Camera.main.transform);
	}
}
