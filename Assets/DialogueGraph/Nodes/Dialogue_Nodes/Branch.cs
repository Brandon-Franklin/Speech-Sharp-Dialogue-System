using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace Dialogue {
    [NodeTint("#A1B3FD")]
    //[NodeTint(1,1,1)]

    public class Branch : DialogueBaseNode
    {

        [Input] public Chat.Connection input;
        [TextArea(2,8)]
        public string Conditions;

        [Output]
        public Chat.Connection pass;
        [Output] public Chat.Connection fail;


        private bool success;
        public int width = 240;

        public ConditionCheck conditionCheck;

        public override void Trigger()
        {
            bool success = true;

            // Perform condition
            if (conditionCheck != null)
            {
                success = conditionCheck.Invoke();
            }

            //Trigger next nodes
            NodePort port;
            if (success) port = GetOutputPort("pass");
            else port = GetOutputPort("fail");
            if (port == null) return;
            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            }
        }

        public bool CheckIfConnected()
        {
            bool connected = false;

            NodePort passPort = GetOutputPort("pass");
            if (passPort != null)
            {
                if (passPort.ConnectionCount != 0)
                {
                    connected = true;
                }
            }
            NodePort failPort = GetOutputPort("fail");
            if (failPort != null)
            {
                if (failPort.ConnectionCount != 0)
                {
                    connected = true;
                }
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