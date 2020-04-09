using System.Linq;
using SystemEx;
using UnityEditor;
using UnityEngine;



namespace UnityEditorEx.Components
{
	[Spyglass(typeof(Transform), true)]
	public class TransformSpyglass : Editor<Transform>, ISpyglassEditor
	{
		public virtual void OnSpyglassGUI()
		{
			if (targets.Length > 1)
			{
				using (EditorGUILayoutEx.Vertical())
				{
					GUILayout.Label("Object distances:");
					foreach (Object[] o in targets.Tuples(2))
					{
						Transform[] t = o.convert<Transform>().ToArray();

						using (EditorGUILayoutEx.Horizontal())
						{
							EditorGUILayout.SelectableLabel("{0} <> {1}: {2}".format(t[0].name, t[1].name, Vector3.Distance(t[0].position, t[1].position)));
						}
					}
				}
			}
		}
	}
}
