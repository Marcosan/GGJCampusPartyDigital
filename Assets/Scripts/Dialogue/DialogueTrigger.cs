using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour {
    //Clase para llamar a DialogueManager, que ya tiene vinculado el respectivo texto del nombre y oraciones del dialogo
    public bool hasOptions;
    public Dialogue dialogue;
    Vector2 npcPosition;
    SpriteRenderer interCollider;

    public bool hasGloboTexto = false;

    //Para text globo
    public GameObject globoGo;
    //SpriteRenderer interCollider;
    public SpriteRenderer spriteGlobo;

    public MeshRenderer dialogoMeshNombre;
    public MeshRenderer dialogoMeshDesc;
    public TextMeshPro dialogoTextNombre;
    public TextMeshPro dialogoTextDesc;

    private void Start() {
        interCollider = transform.GetChild(0).GetComponent<SpriteRenderer>();
        interCollider.enabled = false;
        //Si es text globo
        if (hasGloboTexto) {
            dialogoMeshNombre.sortingLayerName = "Alert";
            dialogoMeshNombre.sortingOrder = 10;
            dialogoMeshDesc.sortingLayerName = "Alert";
            dialogoMeshDesc.sortingOrder = 10;
            globoGo.SetActive(false);
        } else {
            if (!hasOptions) {
                FindObjectOfType<DialogueManager>().SetActiveOptions(false);
            } else {
                FindObjectOfType<DialogueManager>().SetActiveOptions(true);
            }
        }
    }

    void Update() {
        npcPosition = transform.position;
    }

    // Detectamos la colisión con una corrutina
    void OnTriggerEnter2D(Collider2D col) {
        // Si es un ataque
        if (col.tag == "Action") {
            //print("Boton de accion a " + col.gameObject.name);
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, npcPosition);
        }

        if (col.tag == "Interact") {
            if (hasGloboTexto) {
                globoGo.SetActive(true);
            } else {
                interCollider.enabled = true;
            }
        }
        if (!hasGloboTexto)
            changer.StartDialog();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Interact") {
            if (hasGloboTexto)
                globoGo.SetActive(false);
            else
                interCollider.enabled = false;
            changer.DisableButton();
        }
    }
    public void AddSentence(string nombre, string dialogo) {
        //dialogue.AddSentence(dialogo);
        //dialogue.AddName(nombre);
        //nombreIGN.text = nombre + "";
        dialogoTextNombre.text = nombre + "";
        dialogoTextDesc.text = dialogo + "";
    }
}
