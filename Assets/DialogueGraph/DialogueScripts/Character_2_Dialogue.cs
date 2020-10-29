using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using static Globals;
 
[DisallowMultipleComponent]
public class Character_2_Dialogue : MonoBehaviour {
 
 public DialogueCharacter character, player;
int timesSpoken = 0;
bool spoken3Times = false;
 
 void Awake () {
 character = GetComponent<DialogueCharacter>();
 }
 
//all Chat Nodes
//chat options for Chat_0

   public bool Chat_0_Option_0(){
return true;
}

   public bool Chat_0_Option_1(){
if (spoken3Times == true)
{
   return true;
}
else
{
   return false;
}
}

   public bool Chat_0_Option_2(){
return true;
}
//chat options for Chat_1

   public bool Chat_1_Option_0(){
return true;
}
//chat options for Chat_2

   public bool Chat_2_Option_0(){
return true;
}
//all Event Nodes

   public void CallEventEvent_0(){
player.EndConversation();
}

   public void CallEventEvent_1(){
timesSpoken++;
}

   public void CallEventEvent_2(){
player.EndConversation();
}

   public void CallEventEvent_3(){
if(timesSpoken > 3){
    spoken3Times = true;
}
}
//all Branch Nodes

}
