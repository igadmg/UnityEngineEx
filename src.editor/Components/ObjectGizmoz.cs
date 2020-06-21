using UnityEngine;
using UnityEngineEx;


namespace UnityEditorEx
{
	public class ObjectGizmoz : MonoBehaviour
	{
		void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Bounds b = gameObject.GetBounds();
			Gizmos.DrawWireCube(b.center, b.size);
		}
	}
}
