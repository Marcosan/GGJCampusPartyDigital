using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomZone : MonoBehaviour {
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        //StartCoroutine(ZoomCamera());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Zoom") {
            SingletonVars.Instance.ActiveZoom("in", "near", true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Zoom") {
            SingletonVars.Instance.ActiveZoom("out", "near", true);
        }
    }

    /* Efecto zoom lampara con camara
     IEnumerator ZoomCamera() {
        print("trigger");
        targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);

        
        while (Camera.main.orthographicSize > 1)
        {
            yield return new WaitForSeconds(1f);
            Camera.main.orthographicSize -= 0.1f;
        }
        

        // Reproducimos la animación de destrucción y esperamos
        yield return new WaitForSeconds(10);
    }
    */
}
