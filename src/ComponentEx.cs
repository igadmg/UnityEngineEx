using System;
using System.Collections.Generic;
using System.Reflection;
using SystemEx;
using UnityEngine;

namespace UnityEngineEx
{
	public static class ComponentEx
	{
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
	}
}

