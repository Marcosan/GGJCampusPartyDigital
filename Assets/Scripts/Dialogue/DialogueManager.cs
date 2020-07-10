using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour{

    private Queue<string> sentences;

    public Text nameText;
    public Text dialogueText;

    public Animator animator;
    public Animator animatorJoystick;

    public GameObject options;

    Vector2 npcPosition;

    private bool hasOptions = false;

    void Start() {
        sentences = new Queue<string>();
        options.SetActive(false);
    }

    private void Update() {
        //if (Input.GetMouseButtonDown(0)) Debug.Log((int)Camera.main.WorldToScreenPoint(mousePos).y);
        if (Input.GetMouseButtonDown(0)) {
            //Debug.Log(mousePos);
        }
    }

    public void StartDialogue(Dialogue dialogue, Vector2 npcPosition) {
        this.npcPosition = npcPosition;
        animatorJoystick.SetBool("IsClosed", true);
        if (Camera.main.WorldToScreenPoint(npcPosition).y > (Screen.height / 2)) {
            animator.SetBool("IsOpen", true);
            animator.SetBool("IsUp", false);
        } else{
            animator.SetBool("IsOpen", true);
            animator.SetBool("IsUp", true);
        }
        //Debug.Log("Posicion en la camara del npc: " + (int) Camera.main.WorldToScreenPoint(npcPosition).y + " < " + (Screen.height / 2));
        //animator.SetBool("IsUp", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (hasOptions) {
            if (sentences.Count == 1) {
                options.SetActive(true);
            }
        }
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue() {
        options.SetActive(false);
        animatorJoystick.SetBool("IsClosed", false);
        animator.SetBool("IsOpen", false);
        animator.SetBool("IsUp", false);
    }

    public void SetActiveOptions(bool active) {
        //options.SetActive(active);
        hasOptions = active;
    }
}
