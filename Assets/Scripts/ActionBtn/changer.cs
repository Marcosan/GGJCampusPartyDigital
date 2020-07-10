using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class changer : MonoBehaviour
{
    public Button ActionButton;

    // Las imagenes se ingresan desde la interfaz de unity
    public Sprite btnInteract;
    public Sprite btnAttack;
    public Sprite btnPassive;
    
    private static bool btnChange = false;
    private static bool busybtn = false;
    private static string tag = null;

    private void Start()
    {
        ActionButton = GetComponent<Button>();
    }

    private void Update()
    {
        // Cambia la imagen del boton de acuerdo al booleano btnChange y
        // el tag que se ingresa con el metodo setTag
        ChangeButton();
    }

    public void ChangeButton() {
        if (btnChange)
        {
            if (tag.Equals("Dialog"))
                ActionButton.image.overrideSprite = btnInteract;
            else if (tag.Equals("Destroy"))
                ActionButton.image.overrideSprite = btnAttack;
        } else {
            ActionButton.image.overrideSprite = btnPassive;
        }
    }
    
    public static bool GetBusyBtn() {
        return busybtn;
    }
    
    public static void StartDialog() {
        btnChange = true;
        busybtn = true;
        tag = "Dialog";
    }

    public static void StartAction() {
        btnChange = true; // Hace que el boton se habilite
        tag = "Destroy"; // Hace que aparezca la imagen de acuerdo al tipo de interaccion
    }

    public static void DisableButton() {
        btnChange = false; // Deshabilita el boton
        if (busybtn) {
            busybtn = false;
        }
    }

}
