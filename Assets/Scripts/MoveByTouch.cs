using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByTouch : MonoBehaviour {

    Touch touch;
    Vector3 touchPosition;

    // Update is called once per frame
    void Update() {
        /*if(Input.touchCount > 0) {
            //0: obtener el numero de touch
            touch = Input.GetTouch(0);
            //Estado(phase) del touch: Began, ended, moved, stationary, canceled
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;
            transform.position = touchPosition;
        }*/

        for(int i = 0; i < Input.touchCount; i++) {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
            //Dibujara los touch que detecte
            Debug.DrawLine(Vector3.zero, touchPosition, Color.red);
        }
    }
}
