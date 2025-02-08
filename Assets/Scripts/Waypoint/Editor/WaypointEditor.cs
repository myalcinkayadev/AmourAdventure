using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    private Waypoint WaypointTarget => target as Waypoint;

    private void OnSceneGUI()
    {
        if (WaypointTarget.Points.Length == 0) return;
        
        Handles.color = Color.red;
        for (int i = 0; i < WaypointTarget.Points.Length; i++)
        {
            EditorGUI.BeginChangeCheck();

            Vector3 currentPoint = WaypointTarget.EntityPosition + WaypointTarget.Points[i];
            Vector3 newPosition = Handles.FreeMoveHandle(currentPoint, 
                0.4f, Vector3.one * 0.4f, Handles.SphereHandleCap);

            // Label for waypoint index
            GUIStyle textStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 15,
                normal = new GUIStyleState { textColor = Color.black },
                alignment = TextAnchor.MiddleCenter
            };
            Handles.Label(currentPoint + Vector3.up * 0.3f, $"{i + 1}", textStyle);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Move Waypoint");
                WaypointTarget.Points[i] = newPosition - WaypointTarget.EntityPosition;
                EditorUtility.SetDirty(target); // Mark object as changed
            }

            // Draw a line connecting waypoints
            if (i > 0)
            {
                Vector3 previousPoint = WaypointTarget.EntityPosition + WaypointTarget.Points[i - 1];
                Handles.color = Color.red;
                Handles.DrawLine(previousPoint, currentPoint);
            }
        }
    }
}