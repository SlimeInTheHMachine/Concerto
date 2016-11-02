using UnityEngine;
using UnityEditor;
using System.Collections;

public class LevelEditor : EditorWindow {
    GameObject[][] levelBlocks;
    bool groupEnabled;

    //Add Menu item called Level Editor to Window Menu
    [MenuItem ("Window/Level Editor")]
    public static void ShowWindow()
    {
        //Show existing window or make one
        EditorWindow.GetWindow(typeof(LevelEditor));
    }

	void OnGUI()
    {
        //Window Code
        GUILayout.Label("Editor Parameters", EditorStyles.boldLabel);
        //myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Level Grid", groupEnabled);
        //myBool = EditorGUILayout.Toggle("Toggle", myBool);
        //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}
