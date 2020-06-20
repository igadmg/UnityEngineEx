using UnityEngine;
using UnityEngineEx;


namespace UnityEditorEx
{
	public class ObjectGizmoz : MonoBehaviour
	{
		void Awake()
		{
		}

		void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Bounds b = gameObject.GetBounds();
			Gizmos.DrawWireCube(b.center, b.size);
		}
	}
}
