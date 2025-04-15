using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Character character;
    public Coin coin;
    public TimeBonus timeBonus;
    private Graph graph;
    public TriangleGraph triangleGraph;
    public SquareGraph squareGraph;
    public int startingPoint;
    public int spawnPoint;
    public int mode = 0;
    public Camera mainCamera;
    public Canvas gameOver;
    public bool isGameOver = false;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameOver.gameObject.SetActive(false);
        isGameOver = false;
        mode = PlayerPrefs.GetInt("mode", 0);
        if (mode == 0)
        {
            mainCamera.backgroundColor = new Color(1f, 0.7607843f, 0.8196079f);
        }
        else
        {
            mainCamera.backgroundColor = Color.black;
        }
        //PlayerPrefs.DeleteAll();
        //game initialize
        graph = triangleGraph;
        graph.InitializeGraph();
        //startingPoint = UnityEngine.Random.Range(0, graph.Vertex.Count - 1);
        //print("starting point is " + startingPoint);
        //graph.RDFS(startingPoint);
        //spawnPoint = UnityEngine.Random.Range(0, 4);
        //Spawn();
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void NextLevel()
    {
        graph.Renew();  //renew the graph
        ScoreManager.instance.AddPoint();   //add point to the score
        startingPoint = UnityEngine.Random.Range(0, graph.Vertex.Count - 1); //choose new random start point for generating
        graph.RDFS(startingPoint); //use RDFS to generate labyrinth 
        spawnPoint = UnityEngine.Random.Range(0, 4); //choose a random spawn point for character
        Spawn(); //spawn character and coin and timebonus (if needed)

    }
    void Spawn()//spawn character and coin and timebonus (if needed)
    {
        switch (spawnPoint)
        {
            case 0:
                character.Spawn(0, graph.Vertex);
                coin.Spawn(graph.Vertex.Count - 1, graph.Vertex);
                break;
            case 1:
                character.Spawn(graph.Vertex.Count - 1, graph.Vertex);
                coin.Spawn(graph.Vertex.Count - graph.Vertex.Count, graph.Vertex);
                break;
            case 2:
                character.Spawn(graph.Vertex.Count - graph.Vertex.Count, graph.Vertex);
                coin.Spawn(graph.Vertex.Count - 1, graph.Vertex);
                break;
            case 3:
                character.Spawn(graph.Vertex.Count - 1, graph.Vertex);
                coin.Spawn(0, graph.Vertex);
                break;
            default:
                break;
        }
        if(PlayerPrefs.GetInt("timedPlay") == 1) //if timedmode spawn timeBonuses
            if(UnityEngine.Random.Range(0,2)==0)
                timeBonus.Spawn(UnityEngine.Random.Range(1, graph.Vertex.Count - 2), graph.Vertex);
    }
    public void GameOver()
    {
        gameOver.gameObject.SetActive(true);
        isGameOver = true;
    }
}
