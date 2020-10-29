using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using static Globals;
 
[DisallowMultipleComponent]
public class Character_3_Dialogue : MonoBehaviour {
 
 public DialogueCharacter character, player;

 
 void Awake () {
 character = GetComponent<DialogueCharacter>();
 }
 
//all Chat Nodes
//chat options for Chat_0

   public bool Chat_0_Option_0(){
if (g.spokeToCharacter3 == false)
{
   return true;
}
else
{
   return false;
}
}

   public bool Chat_0_Option_1(){
return true;
}
//chat options for Chat_1
//all Event Nodes

   public void CallEventEvent_0(){
g.spokeToCharacter3 = true;
}

   public void CallEventEvent_1(){
player.EndConversation();
}

   public void CallEventEvent_2(){
character.EndConversation();
character.StartCombat(player);
}
//all Branch Nodes

}
