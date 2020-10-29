using System.Collections.Generic;
using System.Linq;
using Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DialogueEditor
{
    [CustomNodeEditor(typeof(ConversationVariables))]
    public class ConversationVariablesEditor : NodeEditor
    {
        //public override int GetHeight()
        //{
        //    ConversationVariables convVars = target as ConversationVariables;

        //    return convVars.height;
        //}

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("defineConversationVariables"));

            ConversationVariables convVars = target as ConversationVariables;
            convVars.width = EditorGUILayout.IntField("Width", convVars.width);

            serializedObject.ApplyModifiedProperties();
        }

        public override int GetWidth()
        {
            ConversationVariables convVars = target as ConversationVariables;
            return convVars.width;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            ConversationVariables convVars = target as ConversationVariables;
            DialogueGraph dg = convVars.graph as DialogueGraph;
            dg.OrganizeNodes();
        }
    }
}