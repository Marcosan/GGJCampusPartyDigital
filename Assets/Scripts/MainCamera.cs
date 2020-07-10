using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour{

    public Transform target;
    float tLX, tLY, bRX, bRY; //TopLeftX TopLeftY BottomRightX BottomRightY
    float posX, posY;
    Vector2 velocity;
    private bool isCameraControlling = true;

    // Para controlar si empieza o no la transición
    //bool start = true;
    // Opacidad inicial del cuadrado de transición
    private float alpha = 1;
    // Transición de 1 segundo
    private float fadeTime = 1f;
    private GameObject area;

    /*
     * Variables para mirador del mapa
     */
    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedTouch = 0.1f;
    private static readonly float ZoomSpeedMouse = 1.5f;

    private static readonly float[] BoundsX = new float[] { -10f, 20f };
    private static readonly float[] BoundsY = new float[] { -50f, 20f };
    private static readonly float[] ZoomBounds = new float[] { 5f, 20f };

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only
    private bool isMenuOpen = false;
    private bool modoMirador = false;

    void Awake()
    {
        cam = GetComponent<Camera>();
        area = GameObject.FindGameObjectWithTag("Area");
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    private void Start()
    {
        //Screen.SetResolution(800, 800, true);
        StartCoroutine(StartFade());
    }

    void Update()
    {
        if (isMenuOpen)
        {
            return;
        }
        if (modoMirador)
        {
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }
        }
    }

    void LateUpdate()
    {
        if (!modoMirador)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }

    }


    public void FastMove()
    {
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

    }

    IEnumerator StartFade()
    {
        target.GetComponent<Animator>().enabled = false;
        target.GetComponent<Player>().enabled = false;

        yield return new WaitForSeconds(fadeTime);

        target.GetComponent<Animator>().enabled = true;
        target.GetComponent<Player>().enabled = true;

        if (area != null)
            StartCoroutine(area.GetComponent<Area>().ShowArea("Temporal", 1f));
    }

    // Dibujaremos un cuadrado con opacidad encima de la pantalla simulando una transición
    void OnGUI()
    {
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Creamos una textura temporal para rellenar la pantalla
        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        // Dibujamos la textura sobre toda la pantalla
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        // Le restamos opacidad
        alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
        // Si la opacidad llega a 0 desactivamos la transición
        //if (alpha < 0) start = false;

    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, offset.y * PanSpeed, 0);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.y = Mathf.Clamp(transform.position.y, BoundsY[0], BoundsY[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    public bool getisCameraControlling()
    {
        return this.isCameraControlling;
    }

    public void setIsCameraControlling(bool control)
    {
        this.isCameraControlling = control;
    }

    public void SetModoMirador(bool active)
    {
        modoMirador = active;
    }

    public void SetSize(float size)
    {
        cam.orthographicSize = size;
    }
}
