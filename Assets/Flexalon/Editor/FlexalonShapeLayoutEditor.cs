using UnityEditor;

namespace Flexalon.Editor
{
    [CustomEditor(typeof(FlexalonShapeLayout)), CanEditMultipleObjects]
    public class FlexalonShapeLayoutEditor : FlexalonComponentEditor
    {
        [MenuItem("GameObject/Flexalon/Shape Layout")]
        public static void Create()
        {
            FlexalonComponentEditor.Create<FlexalonShapeLayout>("Shape Layout");
        }

        public override void OnInspectorGUI()
        {
            ForceUpdateButton();
            SerializedObject so = serializedObject;
            EditorGUILayout.PropertyField(so.FindProperty("_sides"), true);
            EditorGUILayout.PropertyField(so.FindProperty("_shapeRotationDegrees"), true);
            EditorGUILayout.PropertyField(so.FindProperty("_spacing"), true);
            EditorGUILayout.PropertyField(so.FindProperty("_plane"), true);
            EditorGUILayout.PropertyField(so.FindProperty("_planeAlign"), true);
            ApplyModifiedProperties();
        }
    }
}