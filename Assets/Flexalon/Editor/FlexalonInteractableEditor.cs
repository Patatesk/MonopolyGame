using UnityEditor;
using UnityEngine;

namespace Flexalon.Editor
{
    [CustomEditor(typeof(FlexalonInteractable)), CanEditMultipleObjects]
    public class FlexalonInteractableEditor : UnityEditor.Editor
    {
        private static bool _showEvents = false;

        public override void OnInspectorGUI()
        {
            var clickable = serializedObject.FindProperty("_clickable");
            EditorGUILayout.PropertyField(clickable);

            if (clickable.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxClickTime"), new GUIContent("  Max Click Time"));
            }

            var draggable = serializedObject.FindProperty("_draggable");
            EditorGUILayout.PropertyField(draggable);

            if (draggable.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_interpolationSpeed"));

                var restriction = serializedObject.FindProperty("_restriction");
                EditorGUILayout.PropertyField(restriction);
                if (restriction.enumValueIndex == (int)FlexalonInteractable.RestrictionType.Plane)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_planeNormal"), new GUIContent("  Normal"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_localSpaceRestriction"), new GUIContent("  Local Space"));
                }
                else if (restriction.enumValueIndex == (int)FlexalonInteractable.RestrictionType.Line)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_lineDirection"), new GUIContent("  Direction"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_localSpaceRestriction"), new GUIContent("  Local Space"));
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_holdOffset"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_localSpaceOffset"), new GUIContent("  Local Space"));

                var rotateOnGrab = serializedObject.FindProperty("_rotateOnDrag");
                EditorGUILayout.PropertyField(rotateOnGrab);
                if (rotateOnGrab.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_holdRotation"), new GUIContent("  Rotation"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_localSpaceRotation"), new GUIContent("  Local Space"));
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_hideCursor"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_bounds"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_layerMask"));
            }

            _showEvents = EditorGUILayout.Foldout(_showEvents, "Events");
            if (_showEvents)
            {
                if (clickable.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_clicked"));
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_hoverStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_hoverEnd"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("_selectStart"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_selectEnd"));

                if (draggable.boolValue)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_dragStart"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_dragEnd"));
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}