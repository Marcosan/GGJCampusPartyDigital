using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject cam;
    public static bool jump;
    public static Vector3 v3;
    public static bool isRight = true;
    public static bool alive;
    public Text DT;
    public Text RT;
    public static Vector3 dif;

    void Start()
    {
        dif = cam.transform.position - transform.position;
        alive = true;
        jump = false;
        v3 = transform.position;
        if (CheckPointManager.respawn)
        {
            transform.position = CheckPointManager.checkpointPosition;
            CheckPointManager.respawn = false;
        }
    }
    void Update()
    {
        v3 = transform.position;
        if (alive)
        {
            if (!jump)
            {
                gameObject.GetComponent<Animator>().SetBool("jump", true);
            }
            if (Input.GetKey("left") && jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-18000f * Time.deltaTime, 0));
                isRight = false;
                gameObject.GetComponent<Animator>().SetBool("move", true);
            }
            if (Input.GetKey("right") && jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(18000f * Time.deltaTime, 0));
                isRight = true;
                gameObject.GetComponent<Animator>().SetBool("move", true);
            }
            if (Input.GetKey("left") && !jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-18000f * Time.deltaTime, 0));
                isRight = false;
                gameObject.GetComponent<Animator>().SetBool("move", false);
                gameObject.GetComponent<Animator>().SetBool("jump", true);
            }
            if (Input.GetKey("right") && !jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(18000f * Time.deltaTime, 0));
                isRight = true;
                gameObject.GetComponent<Animator>().SetBool("move", false);
                gameObject.GetComponent<Animator>().SetBool("jump", true);
            }
            if (Input.GetKeyDown("space") && jump)
            {
                gameObject.GetComponent<Animator>().SetBool("move", false);
                gameObject.GetComponent<Animator>().SetBool("jump", true);
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 6000f));
                gameObject.GetComponent<AudioSource>().Play();
                jump = false;
            }
            if (jump)
            {
                gameObject.GetComponent<Animator>().SetBool("jump", false);
            }
        }
        if (!alive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameObject.GetComponent<Animator>().SetBool("move", false);
                alive = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                DT.text = "";
                RT.text = "";
                CheckPointManager.respawn = true;
            }
        }
        if (!Input.GetKey("left") && !Input.GetKey("right"))
        {
            gameObject.GetComponent<Animator>().SetBool("move", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Platform" || collision.transform.tag == "crate")
        {
            if (v3.y > collision.transform.position.y + 28f)
            {
                jump = true;
            }
        }
        if (collision.transform.tag == "spike")
        {
            alive = false;
            DT.text = "YOU DIED";
            RT.text = "Press R to restart";
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                DT.transform.position = v3 + new Vector3(220, 100, 0);
                RT.transform.position = v3 + new Vector3(220, 95, 0);
            }
            if (SceneManager.GetActiveScene().name == "Stage1_1")
            {
                DT.transform.position = v3 + new Vector3(0, 100, 0);
                RT.transform.position = v3 + new Vector3(0, 95, 0);
            }
        }
        if (collision.transform.tag == "moving crate")
        {
            if (v3.y > collision.transform.position.y + 28f)
            {
                jump = true;
                gameObject.transform.parent = collision.transform;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "moving crate" || collision.transform.tag == "crate")
        {
            gameObject.transform.parent = null;
            jump = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "moving crate" || collision.transform.tag == "crate" || collision.transform.tag == "platform")
        {
            jump = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Platform" || collision.transform.tag == "crate")
        {
            jump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "acid" || collision.transform.tag == "arrow" || collision.CompareTag("enemy") || collision.CompareTag("boss"))
        {
            alive = false;
            DT.text = "YOU DIED";
            RT.text = "Press R to restart";
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                DT.transform.position = v3 + new Vector3(220, 100, 0);
                RT.transform.position = v3 + new Vector3(220, 95, 0);
            }
            if (SceneManager.GetActiveScene().name == "Stage1_1")
            {
                DT.transform.position = v3 + new Vector3(0, 100, 0);
                RT.transform.position = v3 + new Vector3(0, 95, 0);
            }
        }
    }
}
