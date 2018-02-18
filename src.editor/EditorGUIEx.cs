using System;
using UnityEditor;



namespace UnityEditorEx
{
    public static class EditorGUIEx
    {
        public static IDisposable ChangeCheck(Action action)
        {
            return new EditorGUIChangeCheck(action);
        }

        public static IDisposable DisabledScopeIf(bool isDiasbaled)
        {
            return new EditorGUI.DisabledScope(isDiasbaled);
        }
    }

    internal class EditorGUIChangeCheck : IDisposable
    {
        Action action;

        internal EditorGUIChangeCheck(Action action)
        {
            this.action = action;
            EditorGUI.BeginChangeCheck();
        }

        public void Dispose()
        {
            if (EditorGUI.EndChangeCheck() && action != null)
            {
                action.Invoke();
            }
        }
    }
}
