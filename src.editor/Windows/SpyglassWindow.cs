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
	public class SpyglassWindowStartup
	{
		static SpyglassWindowStartup()
		{
			SpyglassWindow.FindSpyglassEditors();
		}
	}

	internal class SpyglassWindow : EditorWindow
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

		[MenuItem("UnityEx/Find Spyglass Editors")]
		public static void MenuFindSpyglassEditors()
		{
			SpyglassWindow.FindSpyglassEditors();
		}


		private void OnEnable()
		{
			OnSelectionChange();
		}

		void OnFocus()
		{
			OnSelectionChange();
		}


		private class ActiveSpyglassEditor
		{
			public ISpyglassEditor Item1;
			public bool Item2;
		}

		private UnityEngine.Object[] m_ActiveGameObjects;
		private List<ActiveSpyglassEditor> m_ActiveSpyglassEditors = new List<ActiveSpyglassEditor>();
		private Vector2 m_ScrollPosition = Vector2.zero;

		private void OnGUI()
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
			if (!ActiveEditorTracker.sharedTracker.isLocked)
			{
				SelectGameObject(Selection.objects);
				Repaint();
			}
		}

		private void ClearSelectedEditors()
		{
			foreach (ISpyglassEditor editor in m_ActiveSpyglassEditors.Select(i => i.Item1))
			{
				UnityEngine.Object.DestroyImmediate(editor as UnityEngine.Object);
			}
			m_ActiveSpyglassEditors.Clear();
		}

		private void SelectGameObject(UnityEngine.Object[] objects)
		{
			FieldInfo m_ReferenceTargetIndex = typeof(Editor).GetField("m_ReferenceTargetIndex", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo m_Targets = typeof(Editor).GetField("m_Targets", BindingFlags.Instance | BindingFlags.NonPublic);

			ClearSelectedEditors();

			m_ActiveGameObjects = objects;
			if (m_ActiveGameObjects == null || m_ActiveGameObjects.Length == 0 || m_ActiveGameObjects[0] == null)
			{
				return;
			}

			{
				List<Type> editors = null;
				foreach (var baseType in m_ActiveGameObjects[0].GetType().GetBaseTypes<UnityEngine.Object>())
				{
					if (m_SpyglassEditors.TryGetValue(baseType, out editors))
						break;
				}
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

			var gameObject = m_ActiveGameObjects[0] as GameObject;
			if (gameObject != null)
			{
				foreach (Component component in gameObject.GetComponents<Component>())
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
		}

		internal static void FindSpyglassEditors()
		{
			m_SpyglassEditors.Clear();

			foreach (Type t in TypeRepository.GetTypes())
			{
				Type inspectedType = null;
				CustomEditor ce = t.GetAttribute<CustomEditor>();
				if (ce != null && t.HasInterface<ISpyglassEditor>())
				{
					inspectedType = ce.GetInspectedType();
					m_SpyglassEditors.GetOrAdd(inspectedType, key => new List<Type>()).Add(t);
				}
				SpyglassAttribute sa = t.GetAttribute<SpyglassAttribute>();
				if (sa != null && t.HasInterface<ISpyglassEditor>() && sa.inspectedType != inspectedType)
				{
					m_SpyglassEditors.GetOrAdd(sa.inspectedType, key => new List<Type>()).Add(t);
				}
			}
		}
	}
}
