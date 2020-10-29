using XNode;

namespace Dialogue {
	public abstract class DialogueBaseNode : Node {
        [ReadOnly]
        public string nodeName;
        abstract public void Trigger();
        public delegate bool ConditionCheck();

        [System.Serializable] public class Connection { }
    }
}