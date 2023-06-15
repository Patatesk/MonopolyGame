using UnityEditor;
using UnityEngine;

namespace Flexalon.Editor
{
    [CustomEditor(typeof(FlexalonRandomModifier)), CanEditMultipleObjects]
    public class FlexalonRandomModifierEditor : FlexalonComponentEditor
    {
        private bool _showPosition = true;
        private bool _showRotation = true;

        private void CreateItem(string label, string enableName, string minName, string maxName)
        {
            var enableProp = serializedObject.FindProperty(enableName);
            EditorGUILayout.BeginHorizontal();
            var labelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 30.0f;
            EditorGUILayout.PropertyField(enableProp, new GUIContent(label), new GUILayoutOption[] { GUILayout.Width(40.0f) });
            if (enableProp.boolValue)
            {
                EditorGUIUtility.labelWidth = 50.0f;
                EditorGUILayout.PropertyField(serializedObject.FindProperty(minName), new GUIContent("Min"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(maxName), new GUIContent("Max"));
            }

            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = labelWidth;
        }

        public override void OnInspectorGUI()
        {
            ForceUpdateButton();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_randomSeed"), true);
            _showPosition = EditorGUILayout.Foldout(_showPosition, "Position");
            if (_showPosition)
            {
                EditorGUI.indentLevel++;
                CreateItem("X", "_randomizePositionX", "_positionMinX", "_positionMaxX");
                CreateItem("Y", "_randomizePositionY", "_positionMinY", "_positionMaxY");
                CreateItem("Z", "_randomizePositionZ", "_positionMinZ", "_positionMaxZ");
                EditorGUI.indentLevel--;
            }

            _showRotation = EditorGUILayout.Foldout(_showRotation, "Rotation");
            if (_showRotation)
            {
                EditorGUI.indentLevel++;
                CreateItem("X", "_randomizeRotationX", "_rotationMinX", "_rotationMaxX");
                CreateItem("Y", "_randomizeRotationY", "_rotationMinY", "_rotationMaxY");
                CreateItem("Z", "_randomizeRotationZ", "_rotationMinZ", "_rotationMaxZ");
                EditorGUI.indentLevel--;
            }

            ApplyModifiedProperties();
        }
    }
}