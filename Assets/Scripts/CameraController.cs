using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerController.v3 + PlayerController.dif + new Vector3(5,1.5f,0);
    }
}
