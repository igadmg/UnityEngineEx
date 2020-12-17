using System.IO;
using UnityEditorInternal;
using UnityEngine;



namespace UnityEditorEx
{
	public class UnityEditorExSettings : ScriptableObject
	{
		protected static string m_SettingsPath = Path.GetFullPath("ProjectSettings/UnityEditorEx.asset");
		private static UnityEditorExSettings m_Instance = null;
		public static UnityEditorExSettings instance
		{
			get
			{
				if (m_Instance == null)
				{
					UnityEngine.Object[] objectArray = InternalEditorUtility.LoadSerializedFileAndForget(m_SettingsPath);
					if (objectArray != null && objectArray.Length > 0)
					{
						m_Instance = objectArray[0] as UnityEditorExSettings;
					}
					if (m_Instance == null)
					{
						m_Instance = CreateInstance<UnityEditorExSettings>();
						m_Instance.hideFlags = HideFlags.DontSave;
						m_Instance.name = "UnityEditorEx Settings";
						m_Instance.namespaceName = Application.productName;
						InternalEditorUtility.SaveToSerializedFileAndForget(new UnityEngine.Object[1] { m_Instance }, m_SettingsPath, true);
					}
				}

				return m_Instance;
			}
		}
		public void Save()
		{
			InternalEditorUtility.SaveToSerializedFileAndForget(new UnityEngine.Object[1] { this }, m_SettingsPath, true);
		}

		public string namespaceName;
		public string editorScriptsPath;

		public string GetNamespaceName(string path)
			=> Path.GetDirectoryName(path).Replace('/', '.').Replace('\\', '.');
	}
}
