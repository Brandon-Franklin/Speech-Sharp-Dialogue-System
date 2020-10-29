using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using UnityEngine.Events;

namespace Dialogue {
	[NodeTint("#FDA491")]
	public class Event : DialogueBaseNode {
        [Input] public Connection input;

        [TextArea]
        public string cSharpCode;

        public int width = 240;

        public delegate void EventCall();
        public EventCall triggerEvent;

        public override void Trigger() {
            triggerEvent.Invoke();
        }

        public bool CheckIfConnected()
        {
            bool connected = false;

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