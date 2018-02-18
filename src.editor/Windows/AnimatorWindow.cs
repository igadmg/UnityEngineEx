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

            EditorApplication.playModeStateChanged += window.onPlayModeStateChanged;
        }



        private static Type m_AnimatorControllerToolType = typeof(IEdgeGUI).Assembly.GetTypes().Where(t => t.Name == "AnimatorControllerTool").FirstOrDefault();
        private List<Tuple<Animator, AnimatorController>> animators = new List<Tuple<Animator, AnimatorController>>();



        void OnGUI()
        {
            if (animators.Count == 0)
            {
                FindAnimationControllers();
            }

            EditorGUILayout.Space();
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

            foreach (Tuple<Animator, AnimatorController> tuple in animators)
            {
                if (tuple.Item1 == null)
                {
                    if (GUILayout.Button("{0}".format(tuple.Item2.name), GUILayout.Height(20)))
                    {
                        EditorWindow m_AnimatorControllerTool = GetWindow(m_AnimatorControllerToolType);
                        m_AnimatorController.SetValue(m_AnimatorControllerTool, tuple.Item2, null);
                        m_AnimatorControllerTool.Repaint();
                    }
                }
                else
                {
                    if (GUILayout.Button("{0}: {1}".format(tuple.Item1.name, tuple.Item2.name), GUILayout.Height(20)))
                    {
                        EditorWindow m_AnimatorControllerTool = GetWindow(m_AnimatorControllerToolType);
                        m_PreviewAnimator.SetValue(m_AnimatorControllerTool, tuple.Item1);
                        m_AnimatorController.SetValue(m_AnimatorControllerTool, tuple.Item2, null);
                        m_AnimatorControllerTool.Repaint();
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
                    Tuple.Create<Animator, AnimatorController>(a, AnimatorControllerEx.GetEffectiveAnimatorController(a))));
            }
            else
            {
                animators.AddRange(AssetDatabase.FindAssets("t:AnimatorController").Select(guid =>
                    Tuple.Create<Animator, AnimatorController>(null, AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GUIDToAssetPath(guid)))));
            }
        }

        public void onPlayModeStateChanged(PlayModeStateChange state)
        {
            FindAnimationControllers();
        }
    }
}
