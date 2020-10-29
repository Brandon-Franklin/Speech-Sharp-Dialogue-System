using UnityEngine;
using XNode;
namespace Dialogue
{
    [NodeTint("#CDCDCD")]
    public class ConversationVariables : Node
    {
        [TextArea(8, 17)]
        public string defineConversationVariables;

        public int width = 300;

        //// Return the correct value of an output port when requested
        //public override object GetValue(NodePort port)
        //{
        //    return null; // Replace this
        //}
    }
}