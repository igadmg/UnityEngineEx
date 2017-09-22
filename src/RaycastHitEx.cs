using UnityEngine;



namespace UnityEngineEx
{
	/// <summary>
	/// RaycastHit extension methods.
	/// </summary>
	public static class RaycastHitEx
	{
		/// <summary>
		/// GetComponent from GameObject being hit.
		/// </summary>
		/// <typeparam name="C"></typeparam>
		/// <param name="hit"></param>
		/// <returns></returns>
		public static C GetComponent<C>(this RaycastHit hit)
		{
			if (hit.transform == null || hit.transform.gameObject == null)
				return default(C);

			return hit.transform.gameObject.GetComponent<C>();
		}

		/// <summary>
		/// Returns component from the Root object in the hierarchy.
		/// Root object is first parent object not marked with tag SubObject.
		/// Useful if you have complex objects with multiple traceable subparts, but you want to get component from root object.
		/// </summary>
		/// <typeparam name="C"></typeparam>
		/// <param name="hit"></param>
		/// <returns></returns>
		public static C GetRootComponent<C>(this RaycastHit hit)
		{
			if (hit.transform == null)
				return default(C);

			GameObject go = hit.transform.gameObject.GetRootObject();
			if (go == null)
				return default(C);

			return go.GetComponent<C>();
		}
	}
}
