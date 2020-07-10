using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    string ActiveScene;

    public Player player;

    // Para guardar la partida
    public void SavePlayer() {
        // Asigna a una variable la escena antes de guardar.
        ActiveScene = SceneManager.GetActiveScene().name;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void GoToMainMenu() {
        //SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    // Obtener la ultima escena en la que estuvo el player
    public string GetLastScene(){
        return this.ActiveScene;
    }
}
