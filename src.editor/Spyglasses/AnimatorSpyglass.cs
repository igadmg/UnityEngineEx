using SystemEx;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditorEx;
using UnityEngine;
using UnityEngineEx;



namespace UnityEditorEx
{
	[Spyglass(typeof(Animator), true)]
	class AnimatorSpyglass : Editor<Animator>, ISpyglassEditor
	{
		private Vector2 m_ScrollPosition = Vector2.zero;

		public void OnSpyglassGUI()
		{
			var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(target.runtimeAnimatorController));

			using (EditorGUILayoutEx.ScrollView(ref m_ScrollPosition))
			{
				using (GUILayoutEx.Vertical())
				{
					foreach (AnimatorControllerParameter parameter in controller.parameters)
					{
						switch (parameter.type)
						{
							case AnimatorControllerParameterType.Float:
							{
								float value = target.GetFloat(parameter.name);
								using (EditorGUIEx.ChangeCheck(() => target.SetFloat(parameter.name, value)))
								{
									value = EditorGUILayout.FloatField(parameter.name, value);
								}
							}
								break;
							case AnimatorControllerParameterType.Int:
							{
								int value = target.GetInteger(parameter.name);
								using (EditorGUIEx.ChangeCheck(() => target.SetInteger(parameter.name, value)))
								{
									value = EditorGUILayout.IntField(parameter.name, value);
								}
							}
								break;
							case AnimatorControllerParameterType.Bool:
							{
								bool value = target.GetBool(parameter.name);
								using (EditorGUIEx.ChangeCheck(() => target.SetBool(parameter.name, value)))
								{
									value = EditorGUILayout.Toggle(parameter.name, value);
								}
							}
								break;
							case AnimatorControllerParameterType.Trigger:
								if (GUILayout.Button(parameter.name))
								{
									target.SetTrigger(parameter.name);
								}
								break;
						}
					}
				}
			}
		}
	}
}
