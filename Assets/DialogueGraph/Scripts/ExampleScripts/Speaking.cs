using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaking : MonoBehaviour
{
    public Transform playerCam;

    public KeyCode startSpeechKey = KeyCode.Mouse0;
    public KeyCode selectAnswerKey = KeyCode.Mouse1;

    public SpeechUIManager speechMan;
    public DialogueCharacter player;

    void Update()
    {
        if (Input.GetKeyDown(selectAnswerKey))
        {
            if (speechMan.uiMenu_Root.activeSelf)
            {
                speechMan.SelectAnwser();
            }
        }

        if (speechMan.answerIndexList.Length > 0)
        {
            speechMan.mouseWheel = Input.mouseScrollDelta.y;
            if (speechMan.mouseWheel > 0f)
            {
                speechMan.MouseWheelUp();
            }
            else if (speechMan.mouseWheel < 0f)
            {
                speechMan.MouseWheelDown();
            }
        }

        RaycastHit dialogueCharacterHit;
        if (player.speaking == false)
        {
            if (Physics.Raycast(playerCam.position, playerCam.forward, out dialogueCharacterHit, 4f))
            {
                if (dialogueCharacterHit.transform.GetComponent<DialogueCharacter>())
                {
                    speechMan.canTalkUI.gameObject.SetActive(true);
                }
                else
                {
                    speechMan.canTalkUI.gameObject.SetActive(false);
                }
            }
            else
            {
                speechMan.canTalkUI.gameObject.SetActive(false);
            }
        }
        else
        {
            speechMan.canTalkUI.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(startSpeechKey))
        {
            RaycastHit characterHit;
            if (Physics.Raycast(playerCam.position, playerCam.forward, out characterHit, 4f))
            {
                if(characterHit.transform.GetComponent<DialogueCharacter>())
                {
                    speechMan.SetUpSpeechWithNPC(transform, characterHit.transform);
                }
            }
        }
    }
}
