using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using UnityEditor;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx
{
	[InitializeOnLoad]
	public class SpyglassWindwoStartup
	{
		static SpyglassWindwoStartup()
		{
			SpyglassWindow.FindSpyglassEditors();
			if (SpyglassWindow.instance != null)
			{
				SpyglassWindow.instance.OnSelectionChange();
			}
		}
	}

	class SpyglassWindow : EditorWindow
	{
		internal static SpyglassWindow instance = null;
		private static Dictionary<Type, List<Type>> m_SpyglassEditors = new Dictionary<Type, List<Type>>();



		[MenuItem("Window/Spyglass")]
		public static void Init()
		{
			instance = EditorWindow.GetWindow<SpyglassWindow>(false, "Spyglass");
			instance.SelectGameObject(Selection.gameObjects);
			instance.Show();
		}


        private class ActiveSpyglassEditor
        {
            public ISpyglassEditor Item1;
            public bool Item2;
        }

		private GameObject[] m_ActiveGameObjects;
		private List<ActiveSpyglassEditor> m_ActiveSpyglassEditors = new List<ActiveSpyglassEditor>();
		private Vector2 m_ScrollPosition = Vector2.zero;


		void OnGUI()
		{
			if (m_ActiveGameObjects == null || m_ActiveGameObjects.Length == 0 || m_ActiveGameObjects[0] == null)
			{
				return;
			}

			using (EditorGUILayoutEx.ScrollView(ref m_ScrollPosition))
			{
				foreach (var i in m_ActiveSpyglassEditors)
				{
					using (GUILayoutEx.Vertical())
					{
						i.Item2 = EditorGUILayout.InspectorTitlebar(i.Item2, ((Editor)i.Item1).target);
						if (i.Item2)
						{
							i.Item1.OnSpyglassGUI();
						}
					}
				}
			}
		}

		public void OnSelectionChange()
		{
			SelectGameObject(Selection.gameObjects);
			Repaint();
		}

		private void ClearSelectedEditors()
		{
			foreach (ISpyglassEditor editor in m_ActiveSpyglassEditors.Select(i => i.Item1))
			{
				UnityEngine.Object.DestroyImmediate(editor as UnityEngine.Object);
			}
			m_ActiveSpyglassEditors.Clear();
		}

		private void SelectGameObject(GameObject[] gameObject)
		{
			FieldInfo m_ReferenceTargetIndex = typeof(Editor).GetField("m_ReferenceTargetIndex", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo m_Targets = typeof(Editor).GetField("m_Targets", BindingFlags.Instance | BindingFlags.NonPublic);

			ClearSelectedEditors();

			m_ActiveGameObjects = gameObject;
			if (m_ActiveGameObjects == null || m_ActiveGameObjects.Length == 0)
				return;

			{
				List<Type> editors = m_SpyglassEditors.Get(typeof(GameObject));
				if (editors != null)
				{
					m_ActiveSpyglassEditors.AddRange(editors.Select(et => {
						Editor e = (Editor)ScriptableObject.CreateInstance(et);
						m_ReferenceTargetIndex.SetValue(e, 0);
						m_Targets.SetValue(e, m_ActiveGameObjects);
                        return new ActiveSpyglassEditor { Item1 = (ISpyglassEditor)e, Item2 = true };
					}));
				}
			}

			/*
			Dictionary<Type, int> componentList = new Dictionary<Type, int>();

			foreach (Component component in m_ActiveGameObjects[0].GetComponents<Component>())
			{
				Type type = component.GetType();
				if (componentList.ContainsKey(type))
				{
					componentList[type]++;
				}
				else
				{
					componentList.Add(type, 1);
				}
			}
			*/

			foreach (Component component in m_ActiveGameObjects[0].GetComponents<Component>())
			{
				foreach (Type type in component.GetType().GetBaseTypes<Component>())
				{
					List<Type> editors = m_SpyglassEditors.Get(type);
					if (editors != null)
					{
						m_ActiveSpyglassEditors.AddRange(editors.Select(et => {
							Editor e = (Editor)ScriptableObject.CreateInstance(et);
							m_ReferenceTargetIndex.SetValue(e, 0);
							m_Targets.SetValue(e, new UnityEngine.Object[] { component });
							return new ActiveSpyglassEditor { Item1 = (ISpyglassEditor)e, Item2 = true };
                        }));
					}
				}
			}
		}

		internal static void FindSpyglassEditors()
		{
			m_SpyglassEditors.Clear();
			FieldInfo m_InspectedType = typeof(CustomEditor).GetField("m_InspectedType", BindingFlags.Instance | BindingFlags.NonPublic);
			var assemblies = (Assembly[])typeof(Editor).Assembly.GetTypes().Where(t => t.Name == "EditorAssemblies").FirstOrDefault()
				.GetProperty("loadedAssemblies", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null);

			foreach (Assembly a in assemblies)
			{
				foreach (Type t in a.GetTypes())
				{
					CustomEditor ce = t.GetAttribute<CustomEditor>();
					if (ce != null && t.HasInterface<ISpyglassEditor>())
					{
						Type inspectedType = (Type)m_InspectedType.GetValue(ce);
						List<Type> editors = m_SpyglassEditors.GetOrAdd(inspectedType, () => new List<Type>());

						editors.Add(t);
					}
					SpyglassAttribute sa = t.GetAttribute<SpyglassAttribute>();
					if (sa != null && t.HasInterface<ISpyglassEditor>())
					{
						m_SpyglassEditors.GetOrAdd(sa.inspectedType, () => new List<Type>()).Add(t);
					}
				}
			}
		}
	}
}
