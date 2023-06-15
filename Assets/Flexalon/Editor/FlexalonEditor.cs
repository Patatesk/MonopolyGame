using UnityEditor;
using UnityEngine;

namespace Flexalon.Editor
{
    [CustomEditor(typeof(Flexalon))]
    public class FlexalonEditor : UnityEditor.Editor
    {
        public static void Create()
        {
            if (FindObjectOfType<Flexalon>() == null)
            {
                var flexalon = Flexalon.GetOrCreate();
                Undo.RegisterCreatedObjectUndo(flexalon.gameObject, "Create Flexalon");
            }
        }

        public override void OnInspectorGUI()
        {
            if ((Application.isPlaying && !(target as Flexalon).UpdateInPlayMode) ||
                (!Application.isPlaying && !(target as Flexalon).UpdateInEditMode))
            {
                if (GUILayout.Button("Update"))
                {
                    // TODO: We probably need to record all dirty objects
                    Undo.RecordObject(target, "Update");
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                    var flexalon = (target as Flexalon);
                    flexalon.RecordFrameChanges = true;
                    flexalon.UpdateDirtyNodes();
                }
            }

            if (GUILayout.Button("Force Update"))
            {
                // TODO: We probably need to record all objects
                Undo.RecordObject(target, "Force Update");
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                var flexalon = (target as Flexalon);
                flexalon.RecordFrameChanges = true;
                flexalon.ForceUpdate();
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_updateInEditMode"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_updateInPlayMode"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_inputProvider"));

            if (serializedObject.ApplyModifiedProperties())
            {
                EditorApplication.QueuePlayerLoopUpdate();
            }

            EditorGUILayout.HelpBox("You should only have one Flexalon component in the scene. If you create a new one, disable and re-enable all flexalon components or restart Unity.", MessageType.Info);
        }
    }
}