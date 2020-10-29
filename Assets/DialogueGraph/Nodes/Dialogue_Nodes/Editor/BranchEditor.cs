using System.Collections.Generic;
using System.Linq;
using Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueEditor {
    [CustomNodeEditor(typeof(Branch))]
    public class BranchEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            //NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("nodeName"));
            //GUILayout.BeginHorizontal();
            //NodeEditorGUILayout.PortField(target.GetInputPort("input"), GUILayout.Width(50));
            //EditorGUILayout.Space();
            //GUILayout.BeginVertical();
            //EditorGUILayout.Space();
            //GUILayout.BeginHorizontal();

            //NodeEditorGUILayout.PortField(target.GetOutputPort("pass"), GUILayout.Width(50)); GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();

            //EditorGUILayout.Space();

            //NodeEditorGUILayout.PortField(target.GetOutputPort("fail"), GUILayout.Width(50)); GUILayout.EndHorizontal();

            //GUILayout.EndVertical();

            //GUILayout.EndHorizontal();

            Branch branch = target as Branch;

            serializedObject.Update();

            //condition gui
            //GUIStyle gui = new GUIStyle();
            //Texture2D tex = new Texture2D(2, 2);
            //Color colr = new Color(.7f, .5f, .5f, 1f);
            //tex.SetPixels(new Color[] { colr, colr, colr, colr });
            //gui.normal.background = tex;
            //gui.normal.textColor = Color.white;
            //gui.wordWrap = true;


            //branch.Conditions = EditorGUILayout.TextArea(branch.Conditions, gui);

            EditorGUILayout.Space();

            //branch.width = EditorGUILayout.IntField("Width", branch.width);

            serializedObject.ApplyModifiedProperties();


        }

        public override void OnCreate()
        {
            base.OnCreate();
            Branch branch = target as Branch;
            DialogueGraph dg = branch.graph as DialogueGraph;
            dg.OrganizeNodes();
        }

        public override int GetWidth()
        {
            Branch branch = target as Branch;

            return branch.width;
        }
    }
}