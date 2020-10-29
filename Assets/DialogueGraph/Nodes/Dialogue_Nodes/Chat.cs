using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialogue {
    [NodeTint("#9BFFC8")]
    public class Chat : DialogueBaseNode
    {
        [Input] public Connection input;
        [Output] public Connection output;
        [TextArea(3, 100)] public string characterDialogue;

        public AudioClip voiceAudio;
        public List<Answer> answers = new List<Answer>();

        public int width = 400;

        [System.Serializable]
        public class Answer
        {
            [TextArea(2,20)]
            public string text;
            [TextArea(1, 4)]
            public string conditions;
            public string portName;
            public ConditionCheck conditionCheck;
            public bool conditionTestResults;

            public void CheckCondition()
            {
                conditionTestResults = conditionCheck.Invoke();
            }
        }

        public void AnswerQuestion(int index)
        {
            NodePort port = null;
            if (answers.Count == 0)
            {
                port = GetOutputPort("output");
            }
            else
            {
                if (answers.Count <= index) return;
                port = GetOutputPort(answers[index].portName);
            }

            if (port == null)
            {
                return;
            }

            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        public List<NodePort> TryGetPortsForAnswer(int index)
        {
            NodePort port = null;
            if (answers.Count == 0)
            {
                port = GetOutputPort("output");
                return port.GetConnections();
            }
            else
            {
                if (answers.Count <= index)
                {
                    return null;
                }
                port = GetOutputPort(answers[index].portName);
                if (port == null)
                {
                    return null;
                }
                return port.GetConnections();
            }     
        }


        public override void Trigger()
        {
            for (int i = 0; i < answers.Count; i++)
            {
                answers[i].CheckCondition();
            }

            (graph as DialogueGraph).current = this;
        }

        public bool CheckIfConnected()
        {
            bool connected = false;

            if(answers.Count == 0)
            {
                NodePort outport = GetOutputPort("output");
                if (outport != null)
                {
                    if (outport.ConnectionCount != 0)
                    {
                        connected = true;
                    }
                }
            }
            else
            {
                connected = true;
            }
            NodePort inport = GetInputPort("input");
            if (inport != null)
            {
                if (inport.ConnectionCount != 0)
                {
                    connected = true;
                }
            }

            return connected;
        }
    }
}