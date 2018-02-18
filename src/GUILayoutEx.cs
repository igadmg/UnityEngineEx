using System;
using UnityEngine;



namespace UnityEngineEx
{
    public static class GUILayoutEx
    {
        public static IDisposable Area(Rect screenRect)
        {
            return new GUILayoutArea(screenRect);
        }

        public static IDisposable Horizontal(params GUILayoutOption[] options)
        {
            return new GUILayoutHorizontal(options);
        }

        public static IDisposable Vertical(params GUILayoutOption[] options)
        {
            return new GUILayoutVertical(options);
        }
    }

    internal class GUILayoutArea : IDisposable
    {
        internal GUILayoutArea(Rect screenRect)
        {
            GUILayout.BeginArea(screenRect);
        }

        public void Dispose()
        {
            GUILayout.EndArea();
        }
    }

    internal class GUILayoutHorizontal : IDisposable
    {
        internal GUILayoutHorizontal(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }

    internal class GUILayoutVertical : IDisposable
    {
        internal GUILayoutVertical(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }
}
