using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    public class ClassGenerator
    {
        public static void GenerateAScript(DialogueCharacter dc)
        {
            if (dc.dialogue != null)
            {
                dc.folderName = "DialogueGraph/DialogueScripts";
                dc.generatedClassName = GenerateClassNameByGameObject(dc);
                string copyPath = "Assets/" + dc.folderName + "/" + dc.generatedClassName + ".cs";
                dc.generatedClassPath = copyPath;

                if (!Directory.Exists(Application.dataPath + "/" + dc.folderName + "/"))
                {
                    Directory.CreateDirectory(Application.dataPath + "/" + dc.folderName + "/");
                }

                if (!File.Exists(Application.dataPath + "/" + dc.folderName + "/" + dc.generatedClassName + ".cs"))
                {
                    Debug.Log("Creating Classfile: " + copyPath);
                }
                else
                {
                    Debug.Log("Updating Classfile: " + copyPath);
                }

                StreamWriter outfile = new StreamWriter(copyPath);
                using (outfile)
                {
                    outfile.WriteLine("using UnityEngine;");
                    outfile.WriteLine("using System.Collections;");
                    outfile.WriteLine("using System.Collections.Generic;");
                    outfile.WriteLine("using Dialogue;");
                    outfile.WriteLine("using static Globals;");
                    outfile.WriteLine(" ");
                    outfile.WriteLine("[DisallowMultipleComponent]");
                    outfile.WriteLine("public class " + dc.generatedClassName + " : MonoBehaviour {");
                    outfile.WriteLine(" ");
                    outfile.WriteLine(" public DialogueCharacter character, player;");
                    outfile.WriteLine(dc.dialogue.localVars.defineConversationVariables);
                    outfile.WriteLine(" ");
                    outfile.WriteLine(" void Awake () {");
                    outfile.WriteLine(" character = GetComponent<DialogueCharacter>();");
                    outfile.WriteLine(" }");
                    outfile.WriteLine(" ");
                    //generate all conditional functions
                    outfile.WriteLine(GenerateConditionalFunctions(dc.dialogue));

                    outfile.WriteLine("}");
                }
                //File written

                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Dialogue Character Missing DialogueGraph!");
            }
            GlobalVariablesCreator.GenerateAGlobalVariablesScript();
        }
        public static string GenerateConditionalFunctions(DialogueGraph dialogue)
        {
            string returnString = "";

            returnString += "//all Chat Nodes\n";
            for (int i = 0; i < dialogue.ChatNodes.Count; i++)
            {
                Chat thisChat = dialogue.ChatNodes[i];
                returnString += "//chat options for " + thisChat.nodeName + "\n";
                for (int j = 0; j < thisChat.answers.Count; j++)
                {
                    string name = thisChat.nodeName + "_Option_" + j;
                    returnString += MakeConditionalFunction(name, thisChat.answers[j].conditions);
                }
            }

            returnString += "//all Event Nodes\n";
            for (int i = 0; i < dialogue.EventNodes.Count; i++)
            {
                Dialogue.Event thisEvent = dialogue.EventNodes[i];
                string name = "CallEvent" + thisEvent.nodeName;
                string conditionalBody = thisEvent.cSharpCode;
                returnString += MakeEvent(name, conditionalBody);
            }

            returnString += "//all Branch Nodes\n";
            for (int i = 0; i < dialogue.BranchNodes.Count; i++)
            {
                Branch thisBranch = dialogue.BranchNodes[i];
                string name = "ResolveBranch" + thisBranch.nodeName;
                string conditionalBody = thisBranch.Conditions;
                returnString += MakeConditionalFunction(name, conditionalBody);
            }

            return returnString;
        }

        static string MakeConditionalFunction(string functionName, string conditional)
        {
            string baseText = "\n   public bool " + functionName + "(){" + "\n";
            if (conditional != null && conditional != "")
            {
                baseText +=
                "if (" + conditional + ")" + "\n" +
                "{" + "\n" +
                "   return true;" + "\n" +
                "}" + "\n" +
                "else" + "\n" +
                "{" + "\n" +
                "   return false;" + "\n" +
                "}" + "\n";
            }
            else
            {
                baseText += "return true;" + "\n";
            }
            baseText += "}\n";
            return baseText;
        }
        static string MakeEvent(string functionName, string codeText)
        {
            string baseText = "\n   public void " + functionName + "(){" + "\n";
            baseText += codeText + "\n";
            baseText += "}\n";
            return baseText;
        }


        static string GenerateClassNameByGameObject(DialogueCharacter dc)
        {
            string name = dc.gameObject.name.Replace(" ", "_");
            name = name.Replace("-", "_");
            name += "_Dialogue";
            return name;
        }
    }
}
