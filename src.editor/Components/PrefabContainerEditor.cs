using UnityEditor;
using UnityEngine;
using UnityEngineEx;

namespace UnityEditorEx.Components
{
	[CustomEditor(typeof(PrefabContainer))]
	class PrefabContainerEditor : Editor<PrefabContainer>
	{
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("Reset")) {
				target.DoReset();
			}

			base.OnInspectorGUI();
		}
	}
}
