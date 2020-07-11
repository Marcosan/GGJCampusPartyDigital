using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Tiempo : MonoBehaviour
{
    private float tiempo;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "20.00";
        tiempo = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempo >0)
        {
            tiempo -= Time.deltaTime;
            text.text = "" + tiempo;

        }
        else
        {
            text.text = "F";
        }
    }
}
