using UnityEngine;
using XNode;
namespace Dialogue
{
    [NodeTint("#FFFFFF")]
    public class GlobalVariables : Node
    {
        [TextArea(8, 17)]
        public string defineGlobalVariables;

        public int width = 300;

        //// Return the correct value of an output port when requested
        //public override object GetValue(NodePort port)
        //{
        //    return null; // Replace this
        //}
    }
}
