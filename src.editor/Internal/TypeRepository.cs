using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using UnityEditor;



namespace UnityEditorEx
{
	[InitializeOnLoad]
	public class TypeRepository
	{
		private static List<Type> m_TypeCache;
		private static Dictionary<Type, List<Tuple<Type, Attribute>>> m_TypeByAttributeCache;

		static TypeRepository()
		{
			m_TypeCache = new List<Type>();
			m_TypeByAttributeCache = new Dictionary<Type, List<Tuple<Type, Attribute>>>();
		}

		public static IEnumerable<Type> GetTypes()
		{
			if (m_TypeCache.Count == 0)
			{
				var assemblies = (Assembly[])typeof(Editor).Assembly.GetTypes().Where(t => t.Name == "EditorAssemblies").FirstOrDefault()
					.GetProperty("loadedAssemblies", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null);

				foreach (Assembly a in assemblies)
				{
					try
					{
						m_TypeCache.AddRange(a.GetTypes());
					}
					catch { }
				}
			}

			return m_TypeCache;
		}

		public static IEnumerable<Tuple<Type, TAttribute>> EnumTypesWithAttribute<TAttribute>()
			where TAttribute : Attribute
		{
			List<Tuple<Type, Attribute>> cachedTypes;
			if (m_TypeByAttributeCache.TryGetValue(typeof(TAttribute), out cachedTypes))
			{
				foreach (Tuple<Type, Attribute> tuple in cachedTypes)
				{
					yield return Tuple.Create(tuple.Item1, (TAttribute)tuple.Item2);
				}

				yield break;
			}

			cachedTypes = new List<Tuple<Type, Attribute>>();
			var assemblies = (Assembly[])typeof(Editor).Assembly.GetTypes().Where(t => t.Name == "EditorAssemblies").FirstOrDefault()
				.GetProperty("loadedAssemblies", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null);

			foreach (Type t in GetTypes())
			{
				TAttribute attr = t.GetAttribute<TAttribute>();
				if (attr != null)
				{
					cachedTypes.Add(Tuple.Create(t, (Attribute)attr));
					yield return Tuple.Create(t, attr);
				}
			}

			m_TypeByAttributeCache.Add(typeof(TAttribute), cachedTypes);
		}
	}
}
