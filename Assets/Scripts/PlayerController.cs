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
    public static Vector3 dif;

    void Start()
    {
        dif = cam.transform.position - transform.position;
        alive = true;
        jump = false;
        v3 = transform.position;
      
    }
    void Update()
    {
        v3 = transform.position;
        if (alive)
        {
            if (Input.GetKey("left") && jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1800f * Time.deltaTime, 0));
                isRight = false;
            }
            if (Input.GetKey("right") && jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1800f * Time.deltaTime, 0));
                isRight = true;
            }
            if (Input.GetKey("left") && !jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1800f * Time.deltaTime, 0));
                isRight = false;
            }
            if (Input.GetKey("right") && !jump)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(1800f * Time.deltaTime, 0));
                isRight = true;
            }
            if (Input.GetKeyDown("space") && jump)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 600f));
                jump = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "floor")
        {
            if (v3.y > collision.transform.position.y + 28f)
            {
                jump = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "floor")
        {
            jump = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "floor")
        {
            jump = true;
        }
    }
}
