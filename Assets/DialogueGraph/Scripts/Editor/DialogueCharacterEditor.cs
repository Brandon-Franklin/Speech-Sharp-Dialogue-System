using Dialogue;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueCharacter))]
public class DialogueCharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogueCharacter cha = (DialogueCharacter)target;

        if(cha.generatedClassName != "" && cha.generatedClassPath != "" && System.IO.File.Exists(Application.dataPath + "/" + cha.folderName + "/" + cha.generatedClassName + ".cs"))
        {
            if (GUILayout.Button("UPDATE C# Dialogue Script"))
            {
                ClassGenerator.GenerateAScript(cha);
            }
        }
        else
        {
            if (GUILayout.Button("CREATE C# Dialogue Script"))
            {
                ClassGenerator.GenerateAScript(cha);
            }
        }

        if (cha.generatedClassName != "" && cha.generatedClassPath != "" && System.IO.File.Exists(Application.dataPath + "/" + cha.folderName + "/" + cha.generatedClassName + ".cs"))
        {
            if (GUILayout.Button("ADD C# Dialogue Script"))
            {
                cha.AddGenScript();
            }
        }
        else
        {
            serializedObject.Update();
            cha.generatedClassName = "";
            cha.generatedClassPath = "";
            serializedObject.ApplyModifiedProperties();
        }

        EditorUtility.SetDirty(target);
    }




}
