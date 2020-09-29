using UnityEditor;

namespace UnityEditorEx.Spyglasses
{
	[Spyglass(typeof(UnityEngine.Object), true)]
	public class ObjectSpyglass : ObjectSpyglass<UnityEngine.Object> { }

	public class ObjectSpyglass<T> : Editor<T>, ISpyglassEditor
		where T : UnityEngine.Object
	{
		public virtual void OnSpyglassGUI()
		{
			string newName = "";
			using (EditorGUIEx.ChangeCheck(() => {
				target.name = newName;
			}))
			{
				newName = EditorGUILayout.TextField("Name", target.name);
			}
		}
	}
}
