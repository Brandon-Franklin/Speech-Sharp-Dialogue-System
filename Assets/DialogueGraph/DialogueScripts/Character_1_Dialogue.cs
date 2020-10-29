using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using static Globals;
 
[DisallowMultipleComponent]
public class Character_1_Dialogue : MonoBehaviour {
 
 public DialogueCharacter character, player;
bool spokeBefore = false;
 
 void Awake () {
 character = GetComponent<DialogueCharacter>();
 }
 
//all Chat Nodes
//chat options for Chat_0

   public bool Chat_0_Option_0(){
return true;
}

   public bool Chat_0_Option_1(){
return true;
}

   public bool Chat_0_Option_2(){
if (g.spokeToCharacter3 == true)
{
   return true;
}
else
{
   return false;
}
}

   public bool Chat_0_Option_3(){
if (spokeBefore == true)
{
   return true;
}
else
{
   return false;
}
}
//chat options for Chat_1
//chat options for Chat_2
//chat options for Chat_3

   public bool Chat_3_Option_0(){
return true;
}

   public bool Chat_3_Option_1(){
if (player.character.Acuity < 3)
{
   return true;
}
else
{
   return false;
}
}
//all Event Nodes

   public void CallEventEvent_0(){
character.EndConversation();
}

   public void CallEventEvent_1(){
player.character.Vitality = 10;
character.EndConversation();
}

   public void CallEventEvent_2(){
player.EndConversation();
}

   public void CallEventEvent_3(){
spokeBefore = true;
}
//all Branch Nodes

   public bool ResolveBranchBranch_0(){
if (character.CheckRelationship(player, ">", 50))
{
   return true;
}
else
{
   return false;
}
}

}
