using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour {

    GameObject player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() {

    }

    IEnumerator ActiveInteraction() {
        player.GetComponent<Player>().setIsActionButton(true);
        yield return new WaitForSeconds(.01f);
        player.GetComponent<Player>().setIsActionButton(false);
    }

    public void PushAction() {
        StartCoroutine(ActiveInteraction());
    }
}
