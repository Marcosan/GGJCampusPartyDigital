using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SingletonVars : Singleton<SingletonVars>{
    protected SingletonVars(){
    }

    public string nameCurrentMap;

    public bool isCounting = false;
    public bool isZooming = false;
    public bool isMenuOpen = false;

    public static int contadorVisitas = 0;
    public static int numMaxVisitas = 10;

    private float targetOrtho;
    private float smoothSpeed = 2.0f;
    private float minOrtho = 3.0f;
    private float maxOrtho = 5.0f;
    private string zoomInOut;

    private string currentStand = "";
    private string currentProduct = "";

    private string jsonprueba01;
    private string jsonprueba02;

    // Start is called before the first frame update
    void Awake() {
        targetOrtho = Camera.main.orthographicSize;
        
    }

    public string SetNameCurrentMap(string nameMap) {
        string[] split = Regex.Split(nameMap, @"(?<!^)(?=[A-Z])");
        string newName = "";
        foreach (string word in split){
            newName += " " + word;
        }
        return newName;
    }

    public void SetIsCounting(bool isCounting) {
        this.isCounting = isCounting;
    }
    public bool GetIsCounting() {
        return this.isCounting;
    }

    public bool IsZooming(){
        return this.isZooming;
    }

    public void SetIsZooming(bool zoom){
        this.isZooming = zoom;
    }

    private IEnumerator ZoomCamera() {
        targetOrtho = Camera.main.orthographicSize;
        //while (maxOrtho >= targetOrtho && minOrtho <= targetOrtho){
        while (true){
            if (zoomInOut.Equals("in"))
                targetOrtho -= 0.1f * smoothSpeed;
            else
                targetOrtho += 0.1f * smoothSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);

            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
            Debug.Log(targetOrtho);
            if (Camera.main.orthographicSize == minOrtho || Camera.main.orthographicSize == maxOrtho)
                break;
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log("salio del loop");
    }
    
    public void ActiveZoom(string zoom, string distance, bool active) {
        this.zoomInOut = zoom;
        if (active){
            StopCoroutine(ZoomCamera());
            StartCoroutine(ZoomCamera());
        }else { 
            StopCoroutine(ZoomCamera());
        }
    }

}
