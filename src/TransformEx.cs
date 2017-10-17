using System;
using System.Collections.Generic;
using UnityDissolve;
using UnityEngine;



namespace UnityEngineEx
{
	/// <summary>
	/// Transfrom extensionf functions.
	/// </summary>
	public static class TransformEx
	{
		/// <summary>
		/// Align transform wit otherTransform. 
		/// Synchronize transfrom positions and rotations.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="otherTransform"></param>
		/// <returns></returns>
		public static Transform Align(this Transform transform, Transform otherTransform)
		{
			transform.position = otherTransform.position;
			transform.rotation = otherTransform.rotation;
			return transform;
		}

		/// <summary>
		/// Adds GameObject as a child to the Transform.
		/// Objects position and rotation are set to localPosition and localRotation.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="child"></param>
		/// <returns></returns>
		public static Transform Add(this Transform transform, GameObject child)
		{
			var po = child.transform.position;
			var ro = child.transform.rotation;
			var sc = child.transform.localScale;
			child.transform.parent = transform;
			child.transform.localPosition = po;
			child.transform.localRotation = ro;
			child.transform.localScale = sc;
			return transform;
		}

		/// <summary>
		/// Adds Transform as a child to the Transform.
		/// Objects position and rotation are set to localPosition and localRotation.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="child"></param>
		/// <returns></returns>
		public static Transform Add(this Transform transform, Transform child)
		{
			var po = child.position;
			var ro = child.rotation;
			var sc = child.localScale;
			child.parent = transform;
			child.localPosition = po;
			child.localRotation = ro;
			child.localScale = sc;
			return transform;
		}

		/// <summary>
		/// Removes all child GameObjects.
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static Transform Clear(this Transform transform)
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying) {
				List<GameObject> objects = new List<GameObject>();
				foreach (Transform child in transform) {
					objects.Add(child.gameObject);
				}
				foreach (GameObject o in objects) {
					GameObjectEx.Destroy(o);
				}
			}
			else
#endif
			{
				foreach (Transform child in transform) {
					GameObjectEx.Destroy(child.gameObject);
				}
			}

			return transform;
		}

		/// <summary>
		/// Removes all child GameObjects filtered by f.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name = "f"></param>
		/// <returns></returns>
		public static Transform Clear(this Transform transform, Func<Transform, bool> f)
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying) {
				List<GameObject> objects = new List<GameObject>();
				foreach (Transform child in transform.Find(f)) {
					objects.Add(child.gameObject);
				}
				foreach (GameObject o in objects) {
					GameObjectEx.Destroy(o);
				}
			}
			else
#endif
			{
				foreach (Transform child in transform.Find(f)) {
					GameObjectEx.Destroy(child.gameObject);
				}
			}
			return transform;
		}

		/// <summary>
		/// Unlinks all child GameObjects.
		/// </summary>
		/// <param name="transform"></param>
		/// <returns></returns>
		public static Transform Unlink(this Transform transform)
		{
			foreach (Transform child in transform) {
				child.parent = null;
			}

			return transform;
		}

		public static Transform SetActive(this Transform transform, bool flag)
		{
			transform.gameObject.SetActive(flag);
			return transform;
		}

		/// <summary>
		/// Finds GameObject by path name. And returns it's Component T if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="transform"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static T Find<T>(this Transform transform, string name) where T : Component
		{
			var t = transform.Find(name);

			if (t != null)
				return t.gameObject.GetComponent<T>();

			Debug.LogWarning(string.Format("No child GameObject '{0}' found.", name));

			return null;
		}

		public static GameObject FindGameObject(this Transform transform, string name)
		{
			var t = name != null ? transform.Find(name) : transform;

			if (t != null)
				return t.gameObject;

			Debug.LogWarning(string.Format("No child GameObject '{0}' found.", name));

			return null;
		}

		public static object Find(this Transform transform, string name, Type type)
		{
			var t = name != null ? transform.Find(name) : transform;

			if (t != null)
				return t.gameObject.GetComponentOrThis(type);

			Debug.LogWarning(string.Format("No child GameObject '{0}' found.", name));

			return null;
		}

		public static IEnumerable<Transform> Find(this Transform transform, Func<Transform, bool> f)
		{
			foreach (Transform child in transform) {
				if (f(child))
					yield return child;
			}

			yield break;
		}
	}
}
