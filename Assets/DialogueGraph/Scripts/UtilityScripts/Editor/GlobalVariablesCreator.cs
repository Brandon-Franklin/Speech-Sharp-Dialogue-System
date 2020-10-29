using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GlobalVariablesCreator
{
#if UNITY_EDITOR

    public static string ClassName = "Globals";
    public static bool writeVarsAsReadOnly = true;
    public static void GenerateAGlobalVariablesScript()
    {
        string copyPath = "Assets/DialogueGraph/Scripts/" + ClassName + ".cs";
        if (!System.IO.File.Exists(Application.dataPath + "DialogueGraph/Scripts/" + ClassName + ".cs"))
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
            outfile.WriteLine(" ");
            outfile.WriteLine("[DisallowMultipleComponent]");
            outfile.WriteLine("public class " + ClassName + " : MonoBehaviour {");
            outfile.WriteLine(" ");
            outfile.WriteLine("public static Globals g;");
            outfile.WriteLine("void Start(){");
            outfile.WriteLine("g = this;");
            outfile.WriteLine("}");
            outfile.WriteLine(GrabAllGlobalVariablesFromAssets());
            outfile.WriteLine(" ");
            outfile.WriteLine("}");
        }
        //File written
        AssetDatabase.Refresh();
    }

    static string GrabAllGlobalVariablesFromAssets()
    {
        string vars = "";

        List<Dialogue.DialogueGraph> list = FindAssetsByType<Dialogue.DialogueGraph>();

        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].globalVars != null)
            {
                if (list[i].globalVars.defineGlobalVariables != null && list[i].globalVars.defineGlobalVariables != "")
                {
                    vars += MakeVariablesPublic(list[i].name, list[i].globalVars.defineGlobalVariables) + "\n";
                }
            }
        }

        return vars;
    }

    static string MakeVariablesPublic(string diag_name, string rawVars)
    {
        string output = "";
        string[] vars = rawVars.Split(';');

        output += "//" + diag_name + "\n" + "[Header("+ "\"" + diag_name.ToString() + "\"" + ")]\n";
        for (int i = 0; i < vars.Length; i++)
        {
            vars[i] = vars[i].Trim('\n');
            if (vars[i].Length > 1)
            {
                output += writeVarsAsReadOnly == true ? 
                    "[ReadOnly]\n"+
                    "public " + vars[i] + ";\n"
                    : "public " + vars[i] + ";\n";
            }
        }

        return output;
    }

    public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();
        string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
        //Debug.Log("assets count: " + guids.Length);

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            //Debug.Log("found asset: " + assetPath);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
            {
                assets.Add(asset);
            }
        }
        return assets;
    }
#endif
}
