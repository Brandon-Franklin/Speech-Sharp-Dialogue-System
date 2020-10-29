using System;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Dialogue {
    [CustomNodeEditor(typeof(Chat))]
    public class ChatEditor : NodeEditor {

        bool recording = false;
        AudioClip myAudioClip;
        int finalPos;

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            EditorStyles.textField.wordWrap = true;

            Chat dialogue = target as Chat;
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("nodeName"));
            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(target.GetInputPort("input"), GUILayout.Width(50));
            EditorGUILayout.Space();
            if (dialogue.answers.Count == 0) NodeEditorGUILayout.PortField(target.GetOutputPort("output"), GUILayout.Width(50));
            GUILayout.EndHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("characterDialogue"));

            GUILayout.BeginHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("voiceAudio"));
            if (GUILayout.Button(recording ? "Stop" : "Rec o", GUILayout.Width(50)))
            {
                recording = !recording;

                if(recording == true)
                {
                    myAudioClip = Microphone.Start(null, false, 3*60, 44100);
                }
                else
                {
                    finalPos = Microphone.GetPosition(null);
                    var soundData = new float[myAudioClip.samples * myAudioClip.channels];
                    myAudioClip.GetData(soundData, 0);

                    //Create shortened array for the data that was used for recording
                    var newData = new float[finalPos * myAudioClip.channels];

                    //Copy the used samples to a new array
                    for (int i = 0; i < newData.Length; i++)
                    {
                        newData[i] = soundData[i];
                    }

                    //One does not simply shorten an AudioClip,
                    //    so we make a new one with the appropriate length
                    var newClip = AudioClip.Create(myAudioClip.name, finalPos,
                        myAudioClip.channels, myAudioClip.frequency, false);

                    newClip.SetData(newData, 0);        //Give it the data from the old clip

                    //Replace the old clip
                    myAudioClip = newClip;

                    SavWav.Save("VoiceAudio/" + dialogue.graph.name + "/" + dialogue.nodeName + "_VO_" + dialogue.graph.name, myAudioClip);
                    AssetDatabase.Refresh();
                    //doesn't work
                    //AudioClip a = Resources.Load<AudioClip>(dialogue.nodeName + "_VO");
                    //dialogue.voiceOver = a;
                }
            }

            GUILayout.EndHorizontal();

            //condition gui
            GUIStyle gui = new GUIStyle();
            Texture2D tex = new Texture2D(2, 2);
            Color colr = new Color(.7f, .5f, .5f, 1f);
            tex.SetPixels(new Color[] { colr, colr, colr, colr });
            gui.normal.background = tex;
            gui.normal.textColor = Color.white;
            gui.wordWrap = true;
            //gui.stretchHeight = true;

            if (dialogue.answers.Count > 0)
            {
                string sectionText = "Player Options";
                EditorGUILayout.LabelField(sectionText);
            }

            for (int i = 0; i < dialogue.answers.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    target.RemoveInstancePort(dialogue.answers[i].portName);
                    dialogue.answers.RemoveAt(i);
                    i--;
                }
                dialogue.answers[i].text = EditorGUILayout.TextArea(dialogue.answers[i].text);

                NodeEditorGUILayout.PortField(new GUIContent(), target.GetOutputPort(dialogue.answers[i].portName), GUILayout.Width(-4));
                GUILayout.EndHorizontal();

                dialogue.answers[i].conditions = EditorGUILayout.TextArea(dialogue.answers[i].conditions, gui);
                GUILayout.Space(2);
            }
            GUILayout.Space(1);

            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                NodePort newport = target.AddInstanceOutput(typeof(Chat.Connection));
                dialogue.answers.Add(new Chat.Answer() { text = "", portName = newport.fieldName });
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            dialogue.width = EditorGUILayout.IntField("Width", dialogue.width);

            serializedObject.ApplyModifiedProperties();
        }

        public override int GetWidth() {
            Chat dialogue = target as Chat;
            return dialogue.width;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Chat dialogue = target as Chat;
            DialogueGraph dg = dialogue.graph as DialogueGraph;
            dg.OrganizeNodes();
        }
    }
}