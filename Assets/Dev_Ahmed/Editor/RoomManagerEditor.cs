using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomManager))]
public class RoomManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This script is responsible for create and joining rooms ", MessageType.Info);

        RoomManager roomManager = (RoomManager)target;

        if (GUILayout.Button("Join School Room"))
        {
            roomManager.OnEnteredRoom_School();
        }
    }
}
