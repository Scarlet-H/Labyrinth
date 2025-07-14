using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    public static Character Instance;
    //public Text test;
    private SpriteRenderer spriteRenderer;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    private readonly float referenceScale = 0.068f;
    [SerializeField] private InputActionReference moveActionToUse;
    public float speed;
    private Vector2 moveDirection;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        float targetHeightUnits = 10f; 
        float worldHeight = Camera.main.orthographicSize * 2f;
        float scaleFactor = worldHeight / targetHeightUnits;
        transform.localScale = Vector3.one * (referenceScale * scaleFactor);
    }
    void Update()
    {
        moveDirection = moveActionToUse.action.ReadValue<Vector2>();
        transform.Translate(speed * Time.deltaTime * moveDirection);
    }
    private void FixedUpdate()
    {
        if(Mathf.Abs(moveDirection.y) > Mathf.Abs(moveDirection.x))
        {
            if (moveDirection.y > 0)
                spriteRenderer.sprite = upSprite;
            else
                spriteRenderer.sprite = downSprite;
        }
        else
        {
            if (moveDirection.x > 0)
                spriteRenderer.sprite = rightSprite;
            else
                spriteRenderer.sprite = leftSprite;
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
            speed += 0.025f;
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
            ScoreManager.instance.AddPoint(5);
            collision.gameObject.SetActive(false);
            speed += 0.05f;
        }
    }
}
