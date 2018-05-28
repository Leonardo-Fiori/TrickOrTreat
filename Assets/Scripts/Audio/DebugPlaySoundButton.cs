using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DebugPlaySound))]
public class DebugPlaySoundButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DebugPlaySound script = (DebugPlaySound) target;

        if (GUILayout.Button("Play Sound"))
        {
            script.Play();
        }
    }
}