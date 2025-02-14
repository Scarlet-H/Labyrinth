using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    Vector2 cursorPos;
    Vector2 characterPosition;
    Vector2 direction;
    Rigidbody2D rb;
    CapsuleCollider2D cc;
    public float moveSpeed;
    public Animator animator;
    public GameObject square;
    public Text test;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        float screenHeight = 2f * Camera.main.orthographicSize;
        float screenWidth = screenHeight * Screen.width / Screen.height;
        float scaleX = screenWidth/1080f*185f;
        float scaleY = screenHeight/1920f*185f;
        transform.localScale = new Vector3(scaleX, scaleY, 1);
        square.transform.localScale = new Vector3(5.65f*scaleX,5.65f* scaleY, 1);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !GameManager.Instance.isGameOver)
        { 
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            characterPosition = transform.position;
            direction = cursorPos - characterPosition;
            test.text = direction.x.ToString()+" "+direction.y.ToString();
        }
        else
        {
            cursorPos = Vector2.zero;
        }
    }
    private void FixedUpdate()
    {
        if(cursorPos == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("goingDown", true);
        }
        else
        {
            rb.linearVelocity = direction.normalized * moveSpeed;

            animator.SetBool("goingUp", false);
            animator.SetBool("goingDown", false);
            animator.SetBool("goingLeft", false);
            animator.SetBool("goingRight", false);

            if (rb.linearVelocity.y > 0)
            {
                if (Math.Abs(rb.linearVelocity.x) > Math.Abs(rb.linearVelocity.y))
                {
                    if (rb.linearVelocity.x > 0)
                    {
                        animator.SetBool("goingRight", true);
                    }
                    else
                    {
                        animator.SetBool("goingLeft", true);
                    }
                }
                else
                {
                    animator.SetBool("goingUp", true);
                }
            }
            else
            {
                if (Math.Abs(rb.linearVelocity.x) > Math.Abs(rb.linearVelocity.y))
                {
                    if (rb.linearVelocity.x > 0)
                    {
                        animator.SetBool("goingRight", true);
                    }
                    else
                    {
                        animator.SetBool("goingLeft", true);
                    }
                }
                else
                {
                    animator.SetBool("goingDown", true);
                }
            }

        }
    }
    public void Spawn(int spawnPoint, List<Node>_Vertex)
    {
        transform.position = _Vertex[spawnPoint].transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Coin")
        {
            //generate new labyrinth hide the coin
            collision.gameObject.SetActive(false);
            GameManager.Instance.NextLevel();
            moveSpeed += 0.05f;
        }
        if(collision.gameObject.tag == "TimeBonus")
        {
            //add time and hide the bonus
            collision.gameObject.SetActive(false);
            Timer.Instance.AddTime();
        }
        if (collision.gameObject.tag == "SpeedBooster")
        {
            //add moveSpeed and hide the booster
            collision.gameObject.SetActive(false);
            moveSpeed += 0.5f;
        }
    }
}
