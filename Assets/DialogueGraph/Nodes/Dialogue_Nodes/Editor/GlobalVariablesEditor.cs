using System.Collections.Generic;
using System.Linq;
using Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueEditor
{
    [CustomNodeEditor(typeof(GlobalVariables))]
    public class GlobalVariablesEditor : NodeEditor
    {
        //public override int GetHeight()
        //{
        //    GlobalVariables globe = target as GlobalVariables;

        //    return globe.height;
        //}

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("defineGlobalVariables"));

            GlobalVariables globals = target as GlobalVariables;
            globals.width = EditorGUILayout.IntField("Width", globals.width);

            serializedObject.ApplyModifiedProperties();

        }

        public override int GetWidth()
        {
            GlobalVariables globals = target as GlobalVariables;
            return globals.width;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            GlobalVariables globals = target as GlobalVariables;
            DialogueGraph dg = globals.graph as DialogueGraph;
            dg.OrganizeNodes();
        }
    }
}