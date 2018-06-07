#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveGameManager))]
public class CustomSaveInspector : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SaveGameManager script = (SaveGameManager)target;

        if (GUILayout.Button("Save file from inspector"))
        {
            script.SaveFileFromInspector();
        }

        if (GUILayout.Button("Load file from inspector"))
        {
            script.LoadFileFromInspector();
        }
    }
}
#endif