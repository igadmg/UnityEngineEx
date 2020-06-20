using UnityEditor;
using UnityEngine;



namespace UnityEditorEx.Components
{
	[Spyglass(typeof(ScriptableObject), true)]
	public class ScriptableObjectSpyglass : Editor<ScriptableObject>, ISpyglassEditor
	{
		public virtual void OnSpyglassGUI()
		{
			if (GUILayout.Button("Ping"))
			{
				EditorGUIUtility.PingObject(target);
			}
			/*
			if (targets.Length > 1)
			{
				GUILayout.BeginVertical();
				GUILayout.Label("Object distances:");
				foreach (Object[] o in targets.Tuples(2))
				{
					Transform[] t = o.convert<Transform>().ToArray();

					GUILayout.BeginHorizontal();
					EditorGUILayout.SelectableLabel("{0} <> {1}: {2}".format(t[0].name, t[1].name, Vector3.Distance(t[0].position, t[1].position)));
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}
			*/
		}
	}
}
