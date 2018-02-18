using System;
using System.Linq;
using System.Reflection;
using UnityEditor;



namespace UnityEditorEx
{
    public static class ProjectBrowserExt
    {
        private static Type m_ProjectBrowserType = null;

        public static EditorWindow GetProjectBrowserWindow()
        {
            if (m_ProjectBrowserType == null)
            {
                m_ProjectBrowserType = typeof(Editor).Assembly.GetTypes().Where(t => t.Name == "ProjectBrowser").FirstOrDefault();
            }

            return m_ProjectBrowserType == null ? null : EditorWindow.GetWindow(m_ProjectBrowserType);
        }

        public static string GetSelectedPath()
        {
            EditorWindow projectBrowser = GetProjectBrowserWindow();
            FieldInfo searchFilterField = m_ProjectBrowserType.GetField("m_SearchFilter", BindingFlags.Instance | BindingFlags.NonPublic);
            object searchFilter = searchFilterField.GetValue(projectBrowser);

            if (searchFilter != null)
            {
                FieldInfo foldersField = searchFilter.GetType().GetField("m_Folders", BindingFlags.Instance | BindingFlags.NonPublic);
                Array folders = (Array)foldersField.GetValue(searchFilter);

                if (folders.Length > 0)
                {
                    return (string)folders.GetValue(0);
                }
            }

            return null;
        }
    }
}