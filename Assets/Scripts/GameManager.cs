using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Character character;
    public Friend friend;
    public TimeBonus timeBonus;
    public SpeedBooster speedBooster;
    private Graph graph;
    public TriangleGraph triangleGraph;
    public SquareGraph squareGraph;
    public HexagonGraph hexagonGraph;
    private int mode = 0;
    public Camera mainCamera;
    public Canvas gameOver;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameOver.gameObject.SetActive(false);
        mode = PlayerPrefs.GetInt("mode", 0);
        if (mode == 0)
        {
            mainCamera.backgroundColor = new Color(0.8470588235294118f, 0.7764705882352941f, 1f);
        }
        else
        {
            mainCamera.backgroundColor = Color.black;
        }
        //PlayerPrefs.DeleteAll();

        //game initialize
        squareGraph.InitializeGraph();
        //squareGraph.gameObject.SetActive(false);
        hexagonGraph.InitializeGraph();
        hexagonGraph.gameObject.SetActive(false);
        triangleGraph.InitializeGraph();
        triangleGraph.gameObject.SetActive(false);
        graph = squareGraph;
        graph.RDFS(UnityEngine.Random.Range(0, graph.Vertex.Count - 1));
        Spawn(UnityEngine.Random.Range(0, 2));
    }
    public void SwitchGraphType(int type)
    {
        graph = type switch
        {
            0 => squareGraph,
            1 => triangleGraph,
            2 => hexagonGraph,
            _ => squareGraph,
        };
        switch (type)
        {
            case 0:
                triangleGraph.gameObject.SetActive(false);
                squareGraph.gameObject.SetActive(true);
                hexagonGraph.gameObject.SetActive(false);
                break;
            case 1:
                triangleGraph.gameObject.SetActive(true);
                squareGraph.gameObject.SetActive(false);
                hexagonGraph.gameObject.SetActive(false);
                break;
            case 2:
                hexagonGraph.gameObject.SetActive(true);
                triangleGraph.gameObject.SetActive(false);
                squareGraph.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void ChooseAlgorythm(int type)
    {
        switch (type)
        {
            case 0:
                graph.RDFS(UnityEngine.Random.Range(0, graph.Vertex.Count - 1));
                break;
            case 1:
                graph.BinaryTree();
                break;
            case 2:
                graph.Eller();
                break;
            default:
                graph.RDFS(UnityEngine.Random.Range(0, graph.Vertex.Count - 1));
                break;
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void NextLevel()
    {
        SwitchGraphType(UnityEngine.Random.Range(0, 3));
        graph.Renew();  //renew the graph
        ChooseAlgorythm(UnityEngine.Random.Range(0, 3));
        ScoreManager.instance.AddPoint(1);   //add point to the score
        Spawn(UnityEngine.Random.Range(0, 2)); //spawn character and coin and timebonus (if needed)
    }
    void Spawn(int spawnPoint)//spawn character and coin and timebonus (if needed)
    {
        switch (spawnPoint)
        {
            case 0:
                character.Spawn(0, graph.Vertex);
                friend.Spawn(graph.Vertex.Count - 1, graph.Vertex);
                break;
            case 1:
                character.Spawn(graph.Vertex.Count - 1, graph.Vertex);
                friend.Spawn(0, graph.Vertex);
                break;
            default:
                break;
        }
        if (UnityEngine.Random.Range(0, 2) == 0)
            speedBooster.Spawn(UnityEngine.Random.Range(1, graph.Vertex.Count - 2), graph.Vertex);
        if (PlayerPrefs.GetInt("timedPlay") == 1) //if timedmode spawn timeBonuses
            if (UnityEngine.Random.Range(0, 3) != 0)
                timeBonus.Spawn(UnityEngine.Random.Range(1, graph.Vertex.Count - 2), graph.Vertex);
    }
    public void GameOver()
    {
        gameOver.gameObject.SetActive(true);
        Character.Instance.speed = 0;
    }
}
