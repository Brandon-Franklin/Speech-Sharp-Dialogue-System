using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class SpeechUIManager : MonoBehaviour
{
    public static SpeechUIManager i;
    public GameObject uiMenu_Root;


    public Transform player;
    DialogueCharacter npc_diagCharacter;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterSpeech;

    public TextMeshProUGUI[] optionsText;

    public GameObject selection_UI;

    public int currentUIIndex = 0;
    public int[] answerIndexList;
    public float mouseWheel;

    IEnumerator progressAfterDelayEnum;
    Dialogue.Chat lastChatNode;

    public GameObject canTalkUI;


    void Start()
    {
        i = this;
    }

    private void Update()
    {
        if(player != null && npc_diagCharacter != null)
        {
            float distance = Vector3.Distance(player.position, npc_diagCharacter.transform.position);
            if(distance > 5f)
            {
                CloseSpeechMenu();
                npc_diagCharacter = null;
            }
        }
    }

    public void SetUpSpeechWithNPC(Transform player, Transform character)
    {
        DialogueCharacter playerDiag = player.GetComponent<DialogueCharacter>();
        DialogueCharacter npcDiag = character.GetComponent<DialogueCharacter>();

        playerDiag.speaking = true;
        npcDiag.speaking = true;

        playerDiag.speakingPartner = npcDiag;
        npcDiag.speakingPartner = playerDiag;

        npcDiag.AssignPlayerToGeneratedClass(playerDiag);
        npcDiag.RestartDialogue();
        SetUpUI_ForNextChatNode(npcDiag);
        npc_diagCharacter = npcDiag;
        OpenSpeechMenu();
    }

    public void MouseWheelUp()
    {
        //wrap around selection
        //currentUIIndex = (int)Mathf.Repeat(currentUIIndex - 1, answerIndexList.Length);

        //stop at top and bottom
        currentUIIndex = (int)Mathf.Clamp(currentUIIndex - 1, 0, answerIndexList.Length-1);

        SetUIHighlight();
    }

    public void MouseWheelDown()
    {        
        //wrap around selection
        //currentUIIndex = (int)Mathf.Repeat(currentUIIndex + 1, answerIndexList.Length);

        //stop at top and bottom
        currentUIIndex = (int)Mathf.Clamp(currentUIIndex + 1, 0, answerIndexList.Length-1);

        SetUIHighlight();
    }

    void SetUIHighlight()
    {
        if (optionsText != null)
        {
            selection_UI.transform.localPosition = optionsText[currentUIIndex].transform.localPosition;
        }
    }

    public void SetUpUI_ForNextChatNode(DialogueCharacter npc_diag)
    {
        currentUIIndex = 0;
        SetUIHighlight();
        answerIndexList = npc_diag.GetCurrentAnswerIndexes();
        SetCharacterName(npc_diag.transform.name);
        SetCharacterSpeech(npc_diag.GetCurrentSpeech());
        PopulateOptions(npc_diag.GetCurrentOptions());

        //play VO
        if (lastChatNode != npc_diag.dialogue.current)
        {
            npc_diag.PlayCurrentVoiceOver();
        }

        if (npc_diag.GetCurrentOptions().Count == 0)
        {
            if(progressAfterDelayEnum != null)
            {
                StopCoroutine(progressAfterDelayEnum);
                progressAfterDelayEnum = null;
            }
            progressAfterDelayEnum = progressAfterSpeechIsDone(npc_diag);
            StartCoroutine(progressAfterDelayEnum);
        }
        lastChatNode = npc_diag.dialogue.current;
    }

    IEnumerator progressAfterSpeechIsDone(DialogueCharacter npc_diag)
    {
        float delay = 2f;
        if (npc_diag.dialogue.current.voiceAudio != null)
        {
            delay = npc_diag.dialogue.current.voiceAudio.length;
        }
        yield return new WaitForSeconds(delay);
        npc_diag.Answer(0);
    }

    public void SetCharacterName(string name)
    {

        string[] pieces = name.Split(' ');
        if(pieces.Length > 1)
        {
            string surname = "";
            for (int i = 1; i < pieces.Length; i++)
            {
                surname += " " + pieces[i];
            }
            characterName.text = pieces[0] +"<size=20>" +surname + "</size>";
        }
        else
        {
            characterName.text = name;
        }
    }
    public void SetCharacterSpeech(string speech)
    {
        characterSpeech.text = speech;
    }
    public void PopulateOptions(List<string> options)
    {
        for (int i = 0; i < optionsText.Length; i++)
        {
            optionsText[i].text = "";
        }
        for (int i = 0; i < options.Count; i++)
        {
            optionsText[i].text = options[i];
        }
    }

    public void SelectAnwser()
    {
        if (npc_diagCharacter != null)
        {
            if (answerIndexList.Length > 0)
            {
                int selectionIndex = answerIndexList[currentUIIndex];
                npc_diagCharacter.Answer(selectionIndex);
            }
            else
            {
                npc_diagCharacter.Answer(0);
            }
            SetUpUI_ForNextChatNode(npc_diagCharacter);
        }
    }

    public void ToggleSpeechMenu()
    {
        if (uiMenu_Root.activeSelf)
        {
            CloseSpeechMenu();
        }
        else
        {
            OpenSpeechMenu();
        }
    }

    public void OpenSpeechMenu()
    {
        uiMenu_Root.SetActive(true);
    }

    public void CloseSpeechMenu()
    {
        uiMenu_Root.SetActive(false);
    }
}
