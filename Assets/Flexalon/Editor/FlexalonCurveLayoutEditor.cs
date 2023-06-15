using UnityEditor;
using UnityEngine;

namespace Flexalon.Editor
{
    // A tiny custom editor for ExampleScript component
    [CustomEditor(typeof(FlexalonCurveLayout)), CanEditMultipleObjects]
    public class FlexalonCurveLayoutEditor : FlexalonComponentEditor
    {
        private bool _showPoints = true;

        [MenuItem("GameObject/Flexalon/Curve Layout")]
        public static void Create()
        {
            FlexalonComponentEditor.Create<FlexalonCurveLayout>("Curve Layout");
        }

        public override void OnInspectorGUI()
        {
            ForceUpdateButton();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_lockTangents"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_lockPositions"), true);

            var curveLayout = (target as FlexalonCurveLayout);
            _showPoints = EditorGUILayout.Foldout(_showPoints, "Points");
            if (_showPoints)
            {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+", GUILayout.Width(50)))
                {
                    var newPos = Vector3.zero;
                    if (curveLayout.Points.Count == 1)
                    {
                        newPos = curveLayout.Points[0].Position + Vector3.left;
                    }
                    else if (curveLayout.Points.Count > 1)
                    {
                        newPos = 2 * curveLayout.Points[0].Position - curveLayout.Points[1].Position;
                    }

                    Record(curveLayout);
                    curveLayout.InsertPoint(0, new FlexalonCurveLayout.CurvePoint{
                        Position = newPos,
                        TangentMode = FlexalonCurveLayout.TangentMode.Smooth,
                    });
                }
                EditorGUILayout.EndHorizontal();

                var mouse = Event.current.mousePosition;

                for (int i = 0; i < curveLayout.Points.Count; i++)
                {
                    EditorGUILayout.BeginVertical();
                    var point = curveLayout.Points[i];
                    EditorGUI.BeginChangeCheck();
                    point.Position = EditorGUILayout.Vector3Field("Position", point.Position);

                    point.TangentMode = (FlexalonCurveLayout.TangentMode) EditorGUILayout.EnumPopup("Tangent", point.TangentMode);
                    Vector3 tan = point.Tangent;
                    if (point.TangentMode == FlexalonCurveLayout.TangentMode.Manual)
                    {
                        point.Tangent = EditorGUILayout.Vector3Field(" ", point.Tangent);
                    }

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("+", GUILayout.Width(50)))
                    {
                        Record(curveLayout);
                        var newPos = point.Position + Vector3.right;
                        if (i < curveLayout.Points.Count - 1)
                        {
                            newPos = point.Position + (curveLayout.Points[i + 1].Position - point.Position) / 2;
                        }
                        else if (curveLayout.Points.Count > 1)
                        {
                            newPos = 2 * point.Position - curveLayout.Points[i - 1].Position;
                        }

                        curveLayout.InsertPoint(i + 1, new FlexalonCurveLayout.CurvePoint{
                            Position = newPos,
                            TangentMode = FlexalonCurveLayout.TangentMode.Smooth,
                        });

                        break;
                    }
                    if (GUILayout.Button("-", GUILayout.Width(50)))
                    {
                        Record(curveLayout);
                        curveLayout.RemovePoint(i);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        Record(curveLayout);
                        curveLayout.ReplacePoint(i, point);
                        break;
                    }

                    EditorGUILayout.Space();

                    EditorGUILayout.EndVertical();

                    var rect = GUILayoutUtility.GetLastRect();

                    if (mouse.x > 0 && mouse.x < rect.width && rect.yMin < mouse.y && mouse.y < rect.yMax)
                    {
                        curveLayout.EditorHovered = i;
                        EditorApplication.QueuePlayerLoopUpdate();
                    }
                }

                EditorGUI.indentLevel -= 1;
            }


            EditorGUILayout.PropertyField(serializedObject.FindProperty("_spacingType"), true);

            if ((target as FlexalonCurveLayout).SpacingType == FlexalonCurveLayout.SpacingOptions.Fixed)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_spacing"), true);
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_startAt"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rotation"), true);
            ApplyModifiedProperties();
        }

        private void DrawLine(Vector3 p1, Vector3 p2, float thickness)
        {
#if UNITY_2020_2_OR_NEWER
            Handles.DrawLine(p1, p2, thickness);
#else
            Handles.DrawLine(p1, p2);
#endif
        }

        public void OnSceneGUI()
        {
            var curveLayout = target as FlexalonCurveLayout;
            var points = curveLayout.Points;
            var transform = curveLayout.gameObject.transform;

            float thickness = 2f;

            if (points != null)
            {
                Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var point1 = points[i];
                    var point2 = points[i + 1];

                    Handles.color = new Color(1, 1, 0, 0.5f);
                    DrawLine(point1.Position, point1.Position + point1.Tangent, thickness);
                    DrawLine(point2.Position, point2.Position - point2.Tangent, thickness);
                }

                Handles.color = new Color(1, 1, 1, 0.5f);
                for (int i = 1; i < curveLayout.CurvePositions.Count; i++)
                {
                    DrawLine(curveLayout.CurvePositions[i - 1], curveLayout.CurvePositions[i], thickness);
                }

                for (int i = 0; i < points.Count; i++)
                {
                    EditorGUI.BeginChangeCheck();
                    var p = points[i];

                    if (!curveLayout.LockPositions)
                    {
                        Vector3 newPos = Handles.PositionHandle(p.Position, Quaternion.identity);
                        if (EditorGUI.EndChangeCheck())
                        {
                            curveLayout.EditorHovered = i;
                            Record(curveLayout);
                            curveLayout.ReplacePoint(i, p.ChangePosition(newPos));
                            MarkDirty(curveLayout);
                        }
                    }

                    if (!curveLayout.LockTangents && points[i].TangentMode == FlexalonCurveLayout.TangentMode.Manual)
                    {
                        if (i < points.Count - 1)
                        {
                            EditorGUI.BeginChangeCheck();
                            Vector3 newTan1 = Handles.PositionHandle(points[i].Position + points[i].Tangent, Quaternion.identity);
                            if (EditorGUI.EndChangeCheck())
                            {
                                curveLayout.EditorHovered = i;
                                Record(curveLayout);
                                curveLayout.ReplacePoint(i, p.ChangeTangent(newTan1 - points[i].Position));
                                MarkDirty(curveLayout);
                            }
                        }

                        if (i > 0)
                        {
                            curveLayout.EditorHovered = i;
                            EditorGUI.BeginChangeCheck();
                            Vector3 newTan2 = Handles.PositionHandle(points[i].Position - points[i].Tangent, Quaternion.identity);
                            if (EditorGUI.EndChangeCheck())
                            {
                                Record(curveLayout);
                                curveLayout.ReplacePoint(i, p.ChangeTangent(points[i].Position - newTan2));
                                MarkDirty(curveLayout);
                            }
                        }
                    }
                }
            }
        }
    }
}