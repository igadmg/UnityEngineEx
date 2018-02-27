using System.Linq;
using SystemEx;
using UnityEditor;
using UnityEngine;



namespace UnityEditorEx.Components
{
    //[CustomEditor(typeof(Transform))]
    [CanEditMultipleObjects]
    public class Inspector_Transform : Editor<Transform>
    {
        private const float FIELD_WIDTH = 212.0f;
        private const bool WIDE_MODE = true;

        private const float POSITION_MAX = 100000.0f;

        private static GUIContent positionGUIContent = new GUIContent(LocalString("Position")
                                                                     , LocalString("The local position of this Game Object relative to the parent."));
        private static GUIContent rotationGUIContent = new GUIContent(LocalString("Rotation")
                                                                     , LocalString("The local rotation of this Game Object relative to the parent."));
        private static GUIContent scaleGUIContent = new GUIContent(LocalString("Scale")
                                                                     , LocalString("The local scaling of this Game Object relative to the parent."));

        private static string positionWarningText = LocalString("Due to floating-point precision limitations, it is recommended to bring the world coordinates of the GameObject within a smaller range.");

        private SerializedProperty positionProperty;
        private SerializedProperty rotationProperty;
        private SerializedProperty scaleProperty;

        private static string LocalString(string text)
        {
            return text; //LocalizationDatabase.GetLocalizedString(text);
        }

        public void OnEnable()
        {
            this.positionProperty = this.serializedObject.FindProperty("m_LocalPosition");
            this.rotationProperty = this.serializedObject.FindProperty("m_LocalRotation");
            this.scaleProperty = this.serializedObject.FindProperty("m_LocalScale");
        }

        public override void OnInspectorGUI()
        {
            if (!EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = WIDE_MODE;
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - FIELD_WIDTH; // align field to right of inspector
            }

            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.positionProperty, positionGUIContent);
            this.RotationPropertyField(this.rotationProperty, rotationGUIContent);
            EditorGUILayout.PropertyField(this.scaleProperty, scaleGUIContent);

            if (!ValidatePosition(((Transform)this.target).position))
            {
                EditorGUILayout.HelpBox(positionWarningText, MessageType.Warning);
            }

            this.serializedObject.ApplyModifiedProperties();

            if (targets.Length > 1)
            {
                GUILayout.BeginVertical();
                GUILayout.Label("Object distances:");
                foreach (Object[] o in targets.Tuples(2))
                {
                    Transform[] t = o.convert<Transform>().ToArray();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.SelectableLabel("{0} <> {1}: {2}".format(t[0].name, t[1].name, Vector3.Distance(t[0].position, t[1].position)));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
        }

        private bool ValidatePosition(Vector3 position)
        {
            if (Mathf.Abs(position.x) > POSITION_MAX) return false;
            if (Mathf.Abs(position.y) > POSITION_MAX) return false;
            if (Mathf.Abs(position.z) > POSITION_MAX) return false;
            return true;
        }

        private void RotationPropertyField(SerializedProperty rotationProperty, GUIContent content)
        {
            Transform transform = (Transform)this.targets[0];
            Quaternion localRotation = transform.localRotation;
            foreach (UnityEngine.Object t in (UnityEngine.Object[])this.targets)
            {
                if (!SameRotation(localRotation, ((Transform)t).localRotation))
                {
                    EditorGUI.showMixedValue = true;
                    break;
                }
            }

            EditorGUI.BeginChangeCheck();

            Vector3 eulerAngles = EditorGUILayout.Vector3Field(content, localRotation.eulerAngles);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(this.targets, "Rotation Changed");
                foreach (UnityEngine.Object obj in this.targets)
                {
                    Transform t = (Transform)obj;
                    t.localEulerAngles = eulerAngles;
                }
                rotationProperty.serializedObject.SetIsDifferentCacheDirty();
            }

            EditorGUI.showMixedValue = false;
        }

        private bool SameRotation(Quaternion rot1, Quaternion rot2)
        {
            if (rot1.x != rot2.x) return false;
            if (rot1.y != rot2.y) return false;
            if (rot1.z != rot2.z) return false;
            if (rot1.w != rot2.w) return false;
            return true;
        }
    }
}