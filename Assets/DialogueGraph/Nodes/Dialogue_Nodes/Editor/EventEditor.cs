using System.Collections.Generic;
using System.Linq;
using Dialogue;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using ball = System;


namespace DialogueEditor {
    [CustomNodeEditor(typeof(Dialogue.Event))]
    public class EventEditor : NodeEditor {

        //public override void OnBodyGUI()
        //{
        //    GUIStyle gui = new GUIStyle();
        //    Texture2D tex = new Texture2D(2, 2);
        //    tex.SetPixels(new Color[] { Color.black, Color.black, Color.black, Color.black });
        //    gui.normal.background = tex;
        //    gui.normal.textColor = Color.white;

        //    GUI.skin.textArea = gui;
        //}

        public override int GetWidth() {
            Dialogue.Event evenT = target as Dialogue.Event;
            return evenT.width;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Dialogue.Event evenT = target as Dialogue.Event;
            DialogueGraph dg = target.graph as DialogueGraph;
            dg.OrganizeNodes();
        }
    }
}