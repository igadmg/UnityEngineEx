using UnityEngine;

namespace UnityEngineEx
{
	public static class SocketEx
	{
		public static T GetSocket<T>(this Component c)
			where T: Component
		{
			Transform t = c.transform.parent;
			while (t != null && t.gameObject.tag == "Socket")
			{
				T r = t.GetComponent<T>();
				if (r != null)
					return r;

				t = t.parent;
			}

			return null;
		}
	}
}
