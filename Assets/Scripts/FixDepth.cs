using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixDepth : MonoBehaviour {

    // Variable para actualizar la profundidad en cada fotograma
    public bool fixEveryFrame;
    public bool customLayout;
    public bool canEnter;
    public SpriteRenderer feedback_sprite;
    SpriteRenderer spr;

    private SpriteRenderer player_sprite;
    private SpriteRenderer parent_sprite;
    private Transform player_transform;
    private int ordenCapa = 10;

    private void Awake() {
        spr = GetComponent<SpriteRenderer>();
        //ordenCapa = spr.sortingOrder;
        if (customLayout) {
            spr.enabled = false;
        }
    }

    void Start() {
        player_sprite = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        player_transform = GameObject.Find("Player").transform;

        spr.sortingLayerName = "Player";
        if (customLayout) {
            parent_sprite = transform.parent.GetComponent<SpriteRenderer>();
            parent_sprite.sortingLayerName = spr.sortingLayerName;
        }
        spr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
	}

	void Update () {
        if (customLayout)
            CheckLayout();
        if (fixEveryFrame) {
            spr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
	}

    private void CheckLayout() {
        //spr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        if(player_transform.position.y < transform.position.y) {
            parent_sprite.sortingOrder = spr.sortingOrder - ordenCapa;
            if (canEnter)
                feedback_sprite.sortingOrder = spr.sortingOrder - ordenCapa + 5;   
        } else {
            parent_sprite.sortingOrder = spr.sortingOrder + ordenCapa;
        }
    }
}
