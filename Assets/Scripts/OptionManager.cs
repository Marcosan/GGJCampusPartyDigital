using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour {

    private Animator anim_perla;

    private void Start() {
        //anim_perla = GameObject.Find("Area/AnimacionPerla").GetComponent<Animator>();
    }
    
    public void OptionNo() {
        FindObjectOfType<DialogueManager>().EndDialogue();
        print("NO!");
    }
    
    public void OptionYes() {
        FindObjectOfType<DialogueManager>().EndDialogue();
        print("SI!");
    }
}
