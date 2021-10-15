using SystemEx;
using UnityEngine;
using UnityEngineEx.Attributes;

namespace UnityEditorEx.Spyglasses
{
	[Spyglass(typeof(MonoBehaviour), true)]
	public class MonoBehaviourSpyglass : MonoBehaviourSpyglass<MonoBehaviour> { }

	public class MonoBehaviourSpyglass<T> : Editor<T>, ISpyglassEditor
		where T : MonoBehaviour
	{
		public void OnSpyglassGUI()
		{
			foreach (var method in target.GetType().GetMethods<EditorFunctionAttribute>())
			{
				if (GUILayout.Button(method.Name))
				{
					method.Invoke(target, new object[] { });
				}
			}
		}
	}
}
