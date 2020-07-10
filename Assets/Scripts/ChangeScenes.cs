using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeScenes : MonoBehaviour{

    public bool isAnArea = true;
    public bool isLoginMenu = false;

    // Para controlar si empieza o no la transición
    bool start = false;
    // Para controlar si la transición es de entrada o salida
    bool isFadeIn = false;
    // Opacidad inicial del cuadrado de transición
    float alpha = 0;
    // Transición de 1 segundo
    float fadeTime = 1f;

    public string nameScene = "";

    private GameObject area;

    //Esta es la forma correcta de mostrar variables privadas en el inspector. 
    //No se deben hacer public variables que no queremos sean accesibles desde otras clases-
    [SerializeField]
    private TextMeshProUGUI percentText;

    [SerializeField]
    private Image progressImage;

    [SerializeField]
    private Transform panelLoading;

    private void Awake(){
        panelLoading.GetChild(0).gameObject.SetActive(false);
        //Para esconder las imagenes del warp
        if (isAnArea) {
            GetComponent<SpriteRenderer>().enabled = false;
            //transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

            //Se busca el area con el texto (UI)
            //area = GameObject.FindGameObjectWithTag("Area");
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Player"){

            col.GetComponent<Animator>().enabled = false;
            col.GetComponent<Player>().enabled = false;

            FadeIn();
            yield return new WaitForSeconds(fadeTime);

            SceneManager.LoadScene(nameScene);
        }
    }

    public void CargarEscena(string nameScene) {
        StartCoroutine(CargarEscenaCO(nameScene));
    }

    private IEnumerator CargarEscenaCO(string nameScene) {

        FadeIn();
        if (isLoginMenu)
            yield return new WaitForSeconds(0f);
        else
            yield return new WaitForSeconds(fadeTime);

        panelLoading.GetChild(0).gameObject.SetActive(true);

        AsyncOperation loading;

        //Iniciamos la carga asíncrona de la escena y guardamos el proceso en 'loading'
        loading = SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Single);

        //Bloqueamos el salto automático entre escenas para asegurarnos el control durante el proceso
        loading.allowSceneActivation = false;

        //Cuando la escena llega al 90% de carga, se produce el cambio de escena
        while (loading.progress < 0.9f)
        {

            //Actualizamos el % de carga de una forma optima 
            //(concatenar con + tiene un alto coste en el rendimiento)
            percentText.text = string.Format("{0}%", loading.progress * 100);

            //Actualizamos la barra de carga
            progressImage.fillAmount = loading.progress;

            //Esperamos un frame
            yield return null;
        }

        //Mostramos la carga como finalizada
        percentText.text = "100%";
        progressImage.fillAmount = 1;

        //Activamos el salto de escena.
        loading.allowSceneActivation = true;
        //SceneManager.LoadScene(nameScene);
    }


    // Dibujaremos un cuadrado con opacidad encima de la pantalla simulando una transición
    void OnGUI(){
        // Si no empieza la transición salimos del evento directamente
        if (!start)
            return;

        // Si ha empezamos creamos un color con una opacidad inicial a 0
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Creamos una textura temporal para rellenar la pantalla
        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        // Dibujamos la textura sobre toda la pantalla
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        // Controlamos la transparencia
        if (isFadeIn){
            // Si es la de aparecer le sumamos opacidad
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }

    }

    // Método para activar la transición de entrada
    void FadeIn(){
        start = true;
        isFadeIn = true;
    }    
}
