using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterController : MonoBehaviour
{
    private Animator animacion;

    public float moveSpeed;
    private Rigidbody2D miCuerpoRigido;
    private bool moving;



    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    private Vector3 moveDirection;



    public float tiempoParaRecargar;
    private bool recargando;
    private GameObject elJugador;



    // Start is called before the first frame update
    void Start()
    {

        animacion = GetComponent<Animator>();
        miCuerpoRigido = GetComponent<Rigidbody2D>();

        //    timeBetweenMoveCounter = timeBetweenMove;
        //    timeToMoveCounter = timeToMove;

        timeBetweenMoveCounter = Random.Range(timeBetweenMove*0.75f,timeBetweenMove*1.5f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.5f);


    }

    // Update is called once per frame
    void Update()

    {
        animacion.SetFloat("isMoving", 0);


        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            miCuerpoRigido.velocity = moveDirection;

            animacion.SetFloat("isMoving", 1);


            if (timeToMoveCounter < 0f) {
                moving = false;
                //timeBetweenMoveCounter = timeBetweenMove;
                timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.5f);

            }



        }
        else {
            timeBetweenMoveCounter -= Time.deltaTime;
            miCuerpoRigido.velocity = Vector2.zero;
            if(timeBetweenMoveCounter < 0f) {
                moving = true;
                // timeToMoveCounter = timeToMove;

                timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.5f);


                float ejeX = Random.Range(-1f, 1f);
                float ejeY = Random.Range(-1f, 1f);

                moveDirection = new Vector3(ejeX * moveSpeed, ejeY * moveSpeed, 0f);


                animacion.SetFloat("posicionX", ejeX);
                animacion.SetFloat("posicionY", ejeY);
                animacion.SetFloat("isMoving", 1);
            }
        }
    }

}
