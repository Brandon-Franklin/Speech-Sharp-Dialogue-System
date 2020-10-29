using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using static Dialogue.Chat;
using static Dialogue.DialogueBaseNode;
using static Dialogue.Event;

public class DialogueCharacter : MonoBehaviour
{
    public DialogueGraph dialogue;
    public Character character;

    public AudioSource source;

    [HideInInspector]
    public string folderName = "";

    [ReadOnly]
    public string generatedClassName = "", generatedClassPath = "";

    public bool speaking = false;
    public DialogueCharacter speakingPartner;

    //core functionality
    void Start()
    {
        if (dialogue != null)
        {
            AssignConditionalFunctions(dialogue);
            source = GetComponent<AudioSource>();
        }
    }

    public void PlayCurrentVoiceOver()
    {
        source.loop = false;
        source.clip = GetCurrentVO();
        source.Play();
    }

    public void RestartDialogue()
    {
        dialogue.Restart();
    }

    public void AssignPlayerToGeneratedClass(DialogueCharacter player)
    {
        if (dialogue != null)
        {
            Component c = GetComponent(generatedClassName);
            FieldInfo myFieldInfo = c.GetType().GetField("player");
            myFieldInfo.SetValue(c, player);
        }
        else
        {
            Debug.LogWarning(transform.name, transform);
            Debug.LogWarning("Missing diag graph on " + transform.name);
        }
    }

    public List<string> GetCurrentOptions()
    {
        List<string> options = new List<string>();
        for (int i = 0; i < dialogue.current.answers.Count; i++)
        {
            if (dialogue.current.answers[i].conditionTestResults)
            {
                options.Add(dialogue.current.answers[i].text);
            }
        }
        return options;
    }

    public int[] GetCurrentAnswerIndexes()
    {
        List<int> indexes = new List<int>();
        for (int i = 0; i < dialogue.current.answers.Count; i++)
        {
            if (dialogue.current.answers[i].conditionTestResults)
            {
                indexes.Add(i);
            }
        }
        return indexes.ToArray();
    }

    public string GetCurrentSpeech()
    {
        return dialogue.current.characterDialogue;
    }

    public AudioClip GetCurrentVO()
    {
        return dialogue.current.voiceAudio;
    }

    public void Answer(int i)
    {
        if (speaking)
        {
            dialogue.current.AnswerQuestion(i);
        }
    }

    //data queries
    public bool RelationshipExists(DialogueCharacter listener)
    {
        return true;
    }

    private float GetRelationshipValue(DialogueCharacter speaker)
    {
        return 1f;
    }

    public bool CheckRelationship(DialogueCharacter speaker, string comparison, float value)
    {
        if (comparison == "<")
        {
            if (GetRelationshipValue(speaker) < value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (comparison == "==")
        {
            if (GetRelationshipValue(speaker) == value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (comparison == ">")
        {
            if (GetRelationshipValue(speaker) > value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (comparison == "<=" || comparison == "=<")
        {
            if (GetRelationshipValue(speaker) <= value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (comparison == ">=" || comparison == "=>")
        {
            if (GetRelationshipValue(speaker) >= value)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else if (comparison == "!=" || comparison == "=!")
        {
            if (GetRelationshipValue(speaker) != value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.LogError("Comparison Operator: " + comparison + ". Please use \"<\", \"==\", \">\", \"<=\", \">=\", or \"!=\"" + " returning \"false\"");
            return false;
        }
    }
    
    //actions
    public void LoadRecentEvents()
    {

    }

    public void LoadRelationships()
    {

    }

    public void LoadRumors()
    {

    }

    public void EndConversation()
    {
        speaking = false;
        speakingPartner.speaking = false;
        SpeechUIManager.i.CloseSpeechMenu();
        speaking = false;
    }

    public void StartCombat(DialogueCharacter target)
    {

    }

    public void InitiateKiss(DialogueCharacter target)
    {

    }

    public void CreateRelationship(DialogueCharacter target)
    {
        //make a new relationship
    }
    
    public void AddToRelationship(DialogueCharacter target, float amount)
    {

    }

    //assignment
    public void AssignConditionalFunctions(DialogueGraph dialogue)
    {
        Type genClass = Type.GetType(generatedClassName);

        Component customClass = GetComponent(generatedClassName);
        for (int i = 0; i < dialogue.ChatNodes.Count; i++)
        {
            Chat thisChat = dialogue.ChatNodes[i];
            for (int j = 0; j < thisChat.answers.Count; j++)
            {
                string name = thisChat.nodeName + "_Option_" + j;
                MethodInfo method = genClass.GetMethod(name);
                thisChat.answers[j].conditionCheck = (ConditionCheck)Delegate.CreateDelegate(typeof(ConditionCheck), customClass, method);
            }
        }

        for (int i = 0; i < dialogue.EventNodes.Count; i++)
        {
            Dialogue.Event thisEvent = dialogue.EventNodes[i];
            string name = "CallEvent" + thisEvent.nodeName;
            MethodInfo method = genClass.GetMethod(name);
            thisEvent.triggerEvent += (EventCall)Delegate.CreateDelegate(typeof(EventCall), customClass, method);
        }

        for (int i = 0; i < dialogue.BranchNodes.Count; i++)
        {
            Branch thisBranch = dialogue.BranchNodes[i];
            string name = "ResolveBranch" + thisBranch.nodeName;
            MethodInfo method = genClass.GetMethod(name);
            thisBranch.conditionCheck += (ConditionCheck)Delegate.CreateDelegate(typeof(ConditionCheck), customClass, method);
        }
    }

#if UNITY_EDITOR
    public void AddGenScript()
    {
        gameObject.AddComponent(Type.GetType(generatedClassName));
    }
#endif
}
