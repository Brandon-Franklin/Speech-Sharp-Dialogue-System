using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
 
[DisallowMultipleComponent]
public class Globals : MonoBehaviour {
 
public static Globals g;
void Start(){
g = this;
}
//Character1_Dialogue
[Header("Character1_Dialogue")]
[ReadOnly]
public bool spokeToCharacter3 = false;


 
}
