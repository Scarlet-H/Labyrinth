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
    public float moveSpeed;
    public Text test;
    private SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    private readonly float referenceScale = 0.068f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        float targetHeightUnits = 10f; 
        float worldHeight = Camera.main.orthographicSize * 2f;
        float scaleFactor = worldHeight / targetHeightUnits;
        transform.localScale = Vector3.one * (referenceScale * scaleFactor);
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
        if (cursorPos == Vector2.zero)
        {
            rb.linearVelocity = Vector2.zero;
            spriteRenderer.sprite = downSprite;
        }
        else
        {
            rb.linearVelocity = direction.normalized * moveSpeed;

            if (Mathf.Abs(rb.linearVelocity.y) > Mathf.Abs(rb.linearVelocity.x))
            {
                if (rb.linearVelocity.y > 0)
                    spriteRenderer.sprite = upSprite;
                else
                    spriteRenderer.sprite = downSprite;
            }
            else
            {
                if (rb.linearVelocity.x > 0)
                    spriteRenderer.sprite = rightSprite;
                else
                    spriteRenderer.sprite = leftSprite;
            }
        }
    }
    public void Spawn(int spawnPoint, List<Node>_Vertex)
    {
        transform.position = _Vertex[spawnPoint].transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Friend"))
        {
            //generate new labyrinth hide the friend
            collision.gameObject.SetActive(false);
            GameManager.Instance.NextLevel();
            moveSpeed += 0.05f;
        }
        if(collision.gameObject.CompareTag("TimeBonus"))
        {
            //add time and hide the bonus
            collision.gameObject.SetActive(false);
            Timer.Instance.AddTime();
        }
        if (collision.gameObject.CompareTag("SpeedBooster"))
        {
            //add moveSpeed and hide the booster
            collision.gameObject.SetActive(false);
            moveSpeed += 0.5f;
        }
    }
}
