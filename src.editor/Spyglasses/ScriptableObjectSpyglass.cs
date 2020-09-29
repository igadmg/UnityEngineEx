using UnityEditor;
using UnityEngine;



namespace UnityEditorEx.Spyglasses
{
	[Spyglass(typeof(ScriptableObject), true)]
	public class ScriptableObjectSpyglass : ScriptableObjectSpyglass<ScriptableObject> { }

	public class ScriptableObjectSpyglass<T> : ObjectSpyglass<T>, ISpyglassEditor
		where T : ScriptableObject
	{
		public override void OnSpyglassGUI()
		{
			base.OnSpyglassGUI();

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
