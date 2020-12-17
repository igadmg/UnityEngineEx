using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemEx;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx
{
	class AnimatorWindow : EditorWindow
	{
		[MenuItem("Window/Animators")]
		public static void ShowWindow()
		{
			AnimatorWindow window = EditorWindow.GetWindow<AnimatorWindow>(false, "Animators");
		}

		private void OnEnable()
		{
			EditorApplication.playModeStateChanged += onPlayModeStateChanged;
		}


		struct AnimatorDescription
		{
			public Animator animator;
			public AnimatorController controller;
			public string path;
		}

		private static Type m_AnimatorControllerToolType = typeof(IEdgeGUI).Assembly.GetTypes().Where(t => t.Name == "AnimatorControllerTool").FirstOrDefault();
		private List<AnimatorDescription> animators = new List<AnimatorDescription>();
		private Vector2 m_ScrollPosition = Vector2.zero;



		void OnGUI()
		{
			if (animators.Count == 0)
			{
				FindAnimationControllers();
			}

			using (EditorGUILayoutEx.ScrollView(ref m_ScrollPosition))
			{
				using (GUILayoutEx.Horizontal())
				{
					GUILayout.Label(EditorApplication.isPlaying ? "Runtime Animator Controllers:" : "Project Animator Controllers:", EditorStyles.boldLabel);
					if (GUILayout.Button("Refresh", GUILayout.Width(80)))
					{
						FindAnimationControllers();
					}
				}

				FieldInfo m_PreviewAnimator = m_AnimatorControllerToolType.GetField("m_PreviewAnimator", BindingFlags.Instance | BindingFlags.NonPublic);
				PropertyInfo m_AnimatorController = m_AnimatorControllerToolType.GetProperty("animatorController", BindingFlags.Instance | BindingFlags.Public);

				foreach (var a in animators)
				{
					using (GUILayoutEx.Horizontal())
					{
						if (a.animator == null)
						{
							if (GUILayout.Button("{0}".format(a.controller.name), GUILayout.Height(20)))
							{
								EditorWindow m_AnimatorControllerTool = GetWindow(m_AnimatorControllerToolType);
								m_AnimatorController.SetValue(m_AnimatorControllerTool, a.controller, null);
								m_AnimatorControllerTool.Repaint();
							}
							if (GUILayout.Button(new GUIContent("S", "Show animator in hierarchy view."), GUILayout.Width(20)))
							{
								EditorGUIUtility.PingObject(AssetDatabase.LoadMainAssetAtPath(a.path));
							}
						}
						else
						{
							if (GUILayout.Button("{0}: {1}".format(a.animator.name, a.controller.name), GUILayout.Height(20)))
							{
								EditorWindow m_AnimatorControllerTool = GetWindow(m_AnimatorControllerToolType);
								m_PreviewAnimator.SetValue(m_AnimatorControllerTool, a.animator);
								m_AnimatorController.SetValue(m_AnimatorControllerTool, a.controller, null);
								m_AnimatorControllerTool.Repaint();
							}
							if (GUILayout.Button(new GUIContent("S", "Show animator in hierarchy view."), GUILayout.Width(20)))
							{

							}
						}
					}
				}
			}
		}

		void OnDestroy()
		{
			EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
		}

		private void FindAnimationControllers()
		{
			animators.Clear();

			if (EditorApplication.isPlaying)
			{
				animators.AddRange(FindObjectsOfType<Animator>().Select(a =>
					new AnimatorDescription {
						animator = a,
						controller = AnimatorControllerEx.GetEffectiveAnimatorController(a),
						path = AssetDatabase.GetAssetPath(a)
					}));
			}
			else
			{
				animators.AddRange(AssetDatabase.FindAssets("t:AnimatorController").Select(guid =>
					new AnimatorDescription {
						animator = null,
						controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(guid)),
						path = AssetDatabase.GUIDToAssetPath(guid)
					}));
			}
		}

		public void onPlayModeStateChanged(PlayModeStateChange change)
		{
			FindAnimationControllers();
			Repaint();
		}
	}
}
