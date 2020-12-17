using SystemEx;
using UnityEditor;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx.Spyglasses
{
	[Spyglass(typeof(Renderer), true)]
	class RendererSpyglass : Editor<Renderer>, ISpyglassEditor
	{
		public void OnSpyglassGUI()
		{
			GUILayout.Label("Bounds:");
			Bounds bounds = target.GetBounds();
			EditorGUILayout.SelectableLabel("{0}".format(bounds));
		}
	}
}
