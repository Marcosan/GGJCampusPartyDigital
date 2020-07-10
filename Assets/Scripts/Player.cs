using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour{
    private Vector2 mov;
    private Vector2 lastMov;
    private Vector2 diferencia;
    private Rigidbody2D rb2d;
    private Animator anim;
    private bool CanMove = true;
    private int numChildren = 0;
    private Touch touch;
    private Vector3 pos;
    private RaycastHit2D hit;
    //private static GlobalDataGame GmData;

    public float speed = 4f;
    public Transform cruz;
    public bool modoTouch;
    public Joystick joystick;

    private SpriteRenderer cruz_sprite;
    private float originalSpeed;

    CircleCollider2D actionCollider;
    CircleCollider2D interCollider;

    bool interacting = false;
    private bool isActionButton = false;

    float offsetCeilingX = 0;
    float offsetCeilingY = 0;
    
    private bool isMovingAlone = false;
    private Vector3 destinyAlone;

    private void Awake(){

        /* Para evitar mas carga desde el inicio puse esta instancia en Awake ya que de preferencia es
         * mejor dejar cargando los archivos mas pesados de a poco en vez de golpe como seria en start.
         */
        // Carga los datos guardados la ultima vez
        //PlData = SaveSystem.LoadPlayer();
    }

    // Start is called before the first frame update
    void Start(){
        foreach (Transform t in transform){
            numChildren++;
        }

        originalSpeed = speed;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        cruz_sprite = cruz.GetComponent<SpriteRenderer>();

        actionCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        interCollider = transform.GetChild(1).GetComponent<CircleCollider2D>();

        actionCollider.offset = interCollider.offset;
        actionCollider.radius = interCollider.radius;

        actionCollider.enabled = false;
        
    }

    // Update is called once per frame
    void Update(){

        if (CanMove){
            if (isActionButton){
                interacting = true;
            }
            else{
                interacting = false;
                actionCollider.enabled = false;
            }
            Movements();

            if (modoTouch) {
                MovementByTouch();
            } else {
                MoveMentsJoyStick();
            }

            Animations(mov);

            Interact();

        }
        
    }

    private void MovementByTouch() {
        if (Input.touchCount == 1 && !SingletonVars.Instance.isMenuOpen) {
            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            hit = Physics2D.Raycast(pos, Vector2.zero);
            touch = Input.GetTouch(0);
            //if (touch.phase == TouchPhase.Began && hit.collider != null && hit.collider.tag == "Piso") {
            if (touch.phase == TouchPhase.Began) {
                if (hit.collider != null) {
                    if (hit.collider.tag.Equals("Piso")) {
                        isMovingAlone = true;
                        destinyAlone = Camera.main.ScreenToWorldPoint(touch.position);
                        destinyAlone.x = (float)Math.Round(destinyAlone.x, 1);
                        destinyAlone.y = (float)Math.Round(destinyAlone.y, 1);
                        destinyAlone.z = 0;
                        cruz.position = destinyAlone;
                        cruz_sprite.enabled = true;
                    }
                }
                
            }
        }
        if (isMovingAlone) {
            if (transform.position.x <= destinyAlone.x + 0.1 &&
                transform.position.x >= destinyAlone.x - 0.1 &&
                transform.position.y <= destinyAlone.y + 0.1 &&
                transform.position.y >= destinyAlone.y - 0.1) {
                cruz_sprite.enabled = false;
                isMovingAlone = false;
            }
            else {
                diferencia = (destinyAlone - transform.position).normalized;
                mov = new Vector2(diferencia.x, diferencia.y);
                lastMov = new Vector2(diferencia.x, diferencia.y);
            }
            //transform.position = Vector3.MoveTowards(transform.position, destinyAlone, speedTmp * Time.deltaTime);
        }
    }

    private void FixedUpdate(){
        MoveCharacter();
    }

    void MoveMentsJoyStick() {
        if ((joystick.Horizontal > .2f || joystick.Horizontal < -.2f) ||
            (joystick.Vertical > .2f || joystick.Vertical < -.2f))
        {
            //mov = new Vector2(joystick.Horizontal, joystick.Vertical);
            // Almaneca ultimo movimiento
            lastMov = new Vector2(joystick.Horizontal, joystick.Vertical);
            int angulo = (int)Angle(joystick.Direction);
            if (angulo >= 345 || angulo <= 15)
                angulo = 0;
            else if (angulo > 15 && angulo < 75)
                angulo = 60;
            else if (angulo >= 75 && angulo <= 105)
                angulo = 90;
            else if (angulo > 105 && angulo < 165)
                angulo = 120;
            else if (angulo >= 165 && angulo <= 195)
                angulo = 180;
            else if (angulo > 195 && angulo < 255)
                angulo = 240;
            else if (angulo >= 255 && angulo <= 285)
                angulo = 270;
            else if (angulo > 285 && angulo < 345)
                angulo = 300;

            float dirMagnitud = joystick.Direction.magnitude;
            double radians = Math.Round(angulo * (Math.PI / 180), 2, MidpointRounding.ToEven);
            float dirHorizontal = Convert.ToSingle(Math.Round(dirMagnitud * Math.Sin(radians), 2, MidpointRounding.ToEven));
            float dirVertical = Convert.ToSingle(Math.Round(dirMagnitud * Math.Cos(radians), 2, MidpointRounding.ToEven));
            mov = new Vector2(dirHorizontal, dirVertical);
        } else
            mov = Vector2.zero;
    }

    //Angulo con respecto a la vertical y en sentido horario
    public static float Angle(Vector2 p_vector2) {
        if (p_vector2.x < 0) {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        } else {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }
    void Movements(){
        //Devuelve 1 o -1 segun la direccion de las flechas, 0 si no se mueve uno de los ejes
        mov = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void TouchUIElement() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            // Check if finger is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                Debug.Log("Clicked on the UI");
            }
        }
    }

    void Animations(Vector2 direction){
        //Para dejar el la direccion a la que camine
        if (direction != Vector2.zero){ //Si el vector de movimiento es distinto de cero
            anim.SetFloat("moveX", direction.x);
            anim.SetFloat("moveY", direction.y);
            anim.SetBool("walking", true);
        } else{
            anim.SetBool("walking", false);
        }
    }

    void Interact(){
        //mov da 1 o -1, por lo que hay que dividir para 2 para el offset
        if (mov != Vector2.zero){
            offsetCeilingX = 0;
            offsetCeilingY = 0;
            float deltaOffset = .2f;
            if (mov.x < -deltaOffset) offsetCeilingX = ((float)Math.Floor(mov.x)) / 2;
            if (mov.y < -deltaOffset) offsetCeilingY = ((float)Math.Floor(mov.y)) / 2;

            if (mov.x > deltaOffset) offsetCeilingX = ((float)Math.Ceiling(mov.x)) / 2;
            if (mov.y > deltaOffset) offsetCeilingY = ((float)Math.Ceiling(mov.y)) / 2;
            
            actionCollider.offset = new Vector2(offsetCeilingX, offsetCeilingY);
            interCollider.offset = new Vector2(offsetCeilingX, offsetCeilingY);
        }

        if (interacting){
            actionCollider.enabled = true;
        }
    }
            
    // Mover al jugador
    void MoveCharacter(){
        rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);
    }

    // Asignar al player la posicion indicada
    public void setPosition(float posX, float posY, float posZ) {
        Vector3 positionPlayer;
        positionPlayer.x = posX;
        positionPlayer.y = posY;
        positionPlayer.z = posZ;
        this.transform.position = positionPlayer;
    }

    // Asignar al player una orientacion (Hacia donde esta mirando)
    public void setPlayerDirection(Vector2 dir) {
        if (dir != Vector2.zero){
            anim.SetFloat("moveX", dir.x);
            anim.SetFloat("moveY", dir.y);
        }
    }
    
    public void MovePlayer(bool move) {
        mov = new Vector2(0f,0f);
        this.CanMove = move;
    }

    public Vector2 getLastMov(){
        return this.lastMov;
    }

    // Cambiar el valor booleano al boton
    // Ir a Assets/Scripts/ActionBtn/Changer.cs para ver implementacion.
    public void setIsActionButton(bool action) {
        this.isActionButton = action;
    }
}
