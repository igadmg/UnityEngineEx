using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SystemEx;
using UnityEditor;
using UnityEditor.SceneManagement;
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
			instance.SelectGameObject(Selection.activeGameObject);
			instance.Show();
		}



		private GameObject m_ActiveGameObject;
		private List<Tuple<ISpyglassEditor, bool>> m_ActiveSpyglassEditors = new List<Tuple<ISpyglassEditor, bool>>();
		private Vector2 m_ScrollPosition = Vector2.zero;


		void OnGUI()
		{
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
			SelectGameObject(Selection.activeGameObject);
			Repaint();
		}

		private void SelectGameObject(GameObject gameObject)
		{
			FieldInfo m_ReferenceTargetIndex = typeof(Editor).GetField("m_ReferenceTargetIndex", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo m_Targets = typeof(Editor).GetField("m_Targets", BindingFlags.Instance | BindingFlags.NonPublic);

			m_ActiveSpyglassEditors.Clear();
			m_ActiveGameObject = gameObject;
			if (m_ActiveGameObject == null)
				return;

			{
				List<Type> editors = m_SpyglassEditors.Get(typeof(GameObject));
				if (editors != null)
				{
					m_ActiveSpyglassEditors.AddRange(editors.Select(et => {
						Editor e = (Editor)Activator.CreateInstance(et);
						m_ReferenceTargetIndex.SetValue(e, 0);
						m_Targets.SetValue(e, new UnityEngine.Object[] { gameObject });
						return Tuple.Create((ISpyglassEditor)e, true);
					}));
				}
			}

			foreach (Component component in m_ActiveGameObject.GetComponents<Component>())
			{
				foreach (Type type in component.GetType().GetBaseTypes<Component>())
				{
					List<Type> editors = m_SpyglassEditors.Get(type);
					if (editors != null)
					{
						m_ActiveSpyglassEditors.AddRange(editors.Select(et => {
							Editor e = (Editor)Activator.CreateInstance(et);
							m_ReferenceTargetIndex.SetValue(e, 0);
							m_Targets.SetValue(e, new UnityEngine.Object[] { component });
							return Tuple.Create((ISpyglassEditor)e, true);
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
