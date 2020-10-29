using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Dialogue {
    [CreateAssetMenu(menuName = "DialogueGraph", order = 0)]
    public class DialogueGraph : NodeGraph
    {
        //[HideInInspector]
        public Chat current;

        public GlobalVariables globalVars;
        public ConversationVariables localVars;

        public List<Chat> ChatNodes;
        public List<Branch> BranchNodes;
        public List<Event> EventNodes;

        public void Restart()
        {
            //Find the first DialogueNode without any inputs. This is the starting node.
            current = GetStartPoint();

            OrganizeNodes();

            localVars = nodes.Find(x => x is ConversationVariables) as ConversationVariables;
            globalVars = nodes.Find(x => x is GlobalVariables) as GlobalVariables;

            current.Trigger();
        }

        public Chat GetStartPoint()
        {
            return nodes.Find(x => x is Chat && x.Inputs.All(y => !y.IsConnected)) as Chat;
        }

        //[ContextMenu("Organize Nodes")]
        public void OrganizeNodes()
        {
            ChatNodes = new List<Chat>();
            BranchNodes = new List<Branch>();
            EventNodes = new List<Event>();

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] is Chat)
                {
                    ChatNodes.Add(nodes[i] as Chat);
                }
                else if (nodes[i] is Branch)
                {
                    BranchNodes.Add(nodes[i] as Branch);
                }
                else if (nodes[i] is Event)
                {
                    EventNodes.Add(nodes[i] as Event);
                }
            }
            localVars = nodes.Find(x => x is ConversationVariables) as ConversationVariables;
            globalVars = nodes.Find(x => x is GlobalVariables) as GlobalVariables;
            NameNodes();
        }

        //[ContextMenu("Name Nodes")]
        public void NameNodes()
        {
            //Debug.Log("chat count : " + ChatNodes.Count + " branch count: " + BranchNodes.Count + " event count: " + EventNodes.Count);
            for (int i = 0; i < BranchNodes.Count; i++)
            {
                BranchNodes[i].nodeName = "Branch_" + i;
            }
            for (int i = 0; i < EventNodes.Count; i++)
            {
                EventNodes[i].nodeName = "Event_" + i;
            }
            for (int i = 0; i < ChatNodes.Count; i++)
            {
                ChatNodes[i].nodeName = "Chat_" + i;
            }
        }
        [ContextMenu("Assign Temp Audio")]
        public void AssignAudio()
        {
            for (int i = 0; i < ChatNodes.Count; i++)
            {
                string name = ChatNodes[i].nodeName + "_VO_" + ChatNodes[i].graph.name;
                AudioClip a;
                string path = "VoiceAudio/" + ChatNodes[i].graph.name + "/" + name;
                if (Resources.Load<AudioClip>(path) != null)
                {
                    a = Resources.Load<AudioClip>(path);
                    
                    #if UNITY_EDITOR
                    Undo.RegisterCompleteObjectUndo(ChatNodes[i], "assignAudio " + ChatNodes[i].name);
                    #endif

                    ChatNodes[i].voiceAudio = a;
                    
                    #if UNITY_EDITOR
                    EditorUtility.SetDirty(ChatNodes[i]);
                    #endif
                }
                else
                {
                    //Debug.Log(name + " DID NOT EXIST");
                    ChatNodes[i].voiceAudio = null;
                }
            }
        }

        [ContextMenu("Clear Unused Nodes")]
        public void ClearNodes()
        {
            for (int i = 0; i < ChatNodes.Count; i++)
            {
                if (!ChatNodes[i].CheckIfConnected())
                {
                    Debug.LogWarning("Removed " + ChatNodes[i].nodeName + " from graph.");
                    RemoveNode(ChatNodes[i]);
                    //ChatNodes.RemoveAt(i);
                }
            }
            for (int i = 0; i < BranchNodes.Count; i++)
            {
                if (!BranchNodes[i].CheckIfConnected())
                {
                    Debug.LogWarning("Removed " + BranchNodes[i].nodeName + " from graph.");
                    RemoveNode(BranchNodes[i]);
                    //BranchNodes.RemoveAt(i);
                }
            }
            for (int i = 0; i < EventNodes.Count; i++)
            {
                if (!EventNodes[i].CheckIfConnected())
                {
                    Debug.LogWarning("Removed " + EventNodes[i].nodeName + " from graph.");
                    RemoveNode(EventNodes[i]);
                    //EventNodes.RemoveAt(i);
                }
            }
        }

        //[ContextMenu("Generate Globals")]
        //public void GenerateGlobalVariables()
        //{

        //    GlobalVariablesCreator.GenerateAGlobalVariablesScript();
        //}
    }
}