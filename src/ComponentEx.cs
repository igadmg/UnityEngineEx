using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SystemEx;
using UnityEngine;



namespace UnityEngineEx
{
	/// <summary>
	/// Component extension functions.
	/// </summary>
	public static class ComponentEx
	{
		/// <summary>
		/// Construct prefab of given component type.
		/// </summary>
		/// <typeparam name="C"></typeparam>
		/// <param name="prefab"></param>
		/// <returns></returns>
		public static C Construct<C>(this C prefab)
			where C : Component
		{
			GameObject go = prefab.gameObject.Construct();
			return (C)go.GetComponent(prefab.GetType());
		}

		public static C Construct<C>(this C prefab, Component parent)
			where C : Component
			=> prefab.Construct(parent.gameObject);

		public static C Construct<C>(this C prefab, GameObject parent)
			where C : Component
		{
			GameObject go = prefab.gameObject.Construct(parent);
			return (C)go.GetComponent(prefab.GetType());
		}

		public static C Construct<C>(this C prefab, Component parent, params ActionContainer[] initializers)
			where C : Component
			=> prefab.Construct(parent.gameObject, initializers);

		public static C Construct<C>(this C prefab, GameObject parent, params ActionContainer[] initializers)
			where C : Component
		{
			GameObject go = prefab.gameObject.Construct(parent, initializers);
			return (C)go.GetComponent(prefab.GetType());
		}

		/// <summary>
		/// Construct prefab of given component type.
		/// Initialize component with fields from initializer object.
		/// </summary>
		/// <typeparam name="C"></typeparam>
		/// <param name="prefab"></param>
		/// <param name="initializer"></param>
		/// <returns></returns>
		public static C Construct<C>(this C prefab, object initializer)
			where C : Component
		{
			GameObject go = prefab.gameObject.Construct(Tuple.Create(typeof(C), initializer));
			return (C)go.GetComponent(prefab.GetType());
		}

		public static C Construct<C>(this C prefab, Component parent, object initializer)
			where C : Component
			=> prefab.Construct(parent.gameObject, initializer);

		public static C Construct<C>(this C prefab, GameObject parent, object initializer)
			where C : Component
		{
			GameObject go = prefab.gameObject.Construct(parent, Tuple.Create(typeof(C), initializer));
			return (C)go.GetComponent(prefab.GetType());
		}

		/// <summary>
		/// Adds GameObject as a child to another GameObject.
		/// Objects position and rotation are set to localPosition and localrotation.
		/// <seealso cref="TransformEx.Add"/>
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static GameObject Add(this Component parent, GameObject o)
		{
			return parent.transform.Add(o).gameObject;
		}

		/// <summary>
		/// Adds Component as a child to another GameObject.
		/// Objects position and rotation are set to localPosition and localrotation.
		/// <seealso cref="TransformEx.Add"/>
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static GameObject Add(this Component parent, Component o)
		{
			return parent.Add(o.gameObject);
		}


		public static T AddComponent<T>(this Component c) where T : Component
		{
			return c.gameObject.AddComponent<T>();
		}

		/// <summary>
		/// Add a Component to the GameObject setting SerializeFields to a parameters values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="c"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this Component c, IDictionary<string, object> parameters) where T : Component
		{
			return c.gameObject.AddComponent<T>(parameters);
		}

		/// <summary>
		/// Add a Component to the GameObject calling a ctor on it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="c">A</param>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this Component c, Action<T> ctor) where T : Component
		{
			return c.gameObject.AddComponent<T>(ctor);
		}

		/// <summary>
		/// Add a Component to the GameObject calling a ctor on it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="c"></param>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public static T AddComponent<T>(this Component c, ActionContainer ctor) where T : Component
		{
			return c.gameObject.AddComponent<T>(ctor);
		}

		public static T GetOrAddComponent<T>(this Component c) where T : Component
		{
			var r = c.GetComponent<T>();
			if (r != null)
				return r;

			return c.AddComponent<T>();
		}

		/// <summary>
		/// Finds GameObject by path name. And returns it's Component T if it exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="c"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static T Find<T>(this Component c, string name) where T : Component
		{
			return c.transform.Find<T>(name);
		}

		public static GameObject FindGameObject(this Component c, string name)
		{
			return c.transform.FindGameObject(name);
		}

		public static Component SetActive(this Component c, bool flag)
		{
			c.gameObject.SetActive(flag);
			return c;
		}

		public static Bounds GetBounds(this Component c)
		{
			return c.gameObject.GetBounds();
		}

#if !UNITY_EDITOR
		public static void Destroy(this Component c)
		{
			if (c == null)
				return;

			UnityEngine.Object.Destroy(c);
		}
#else
		public static void Destroy(this Component c)
		{
			if (c == null)
				return;

			if (UnityEditor.EditorApplication.isPlaying)
				UnityEngine.Object.Destroy(c);
			else
				UnityEngine.Object.DestroyImmediate(c);
		}
#endif

		public static IDisposable EnsureDisabled(this Behaviour c)
		{
			if (c.enabled)
			{
				c.enabled = false;
				return DisposableLock.Lock(() => c.enabled = true);
			}
			else
			{
				return DisposableLock.empty;
			}
		}

#if !UNITY_EDITOR
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IfGameIsPlaying(this Component c, Action action)
		{
			action();
		}
#else
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IfGameIsPlaying(this Component c, Action action)
		{
			if (UnityEditor.EditorApplication.isPlaying)
				action();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IfGameIsNotPlaying(this Component c, Action action)
		{
			if (!UnityEditor.EditorApplication.isPlaying)
				action();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IfIsPartOfCurrentPrefab(this Component c, Action action)
		{
			if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage().Elvis(cps => cps.IsPartOfPrefabContents(c.gameObject)))
				action();
		}
#endif
	}
}

