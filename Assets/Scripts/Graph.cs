using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Graph : MonoBehaviour
{
    public int vertices;
    public List<Node> Vertex;
    float desiredSize;
    public Node nodePrefab;
    float screenSize;
    readonly float padding = 0.1f;
    public void InitializeGraph()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - padding;
        desiredSize = 2f * screenSize / vertices;
        Vertex = new List<Node>();
        for (int i = 0; i < vertices; i++)
        {
            for (int j = 0; j < vertices; j++)
            {
                Node node = Instantiate(nodePrefab, new Vector2(-screenSize + desiredSize / 2f + j * desiredSize, screenSize - desiredSize / 2f - i * desiredSize), Quaternion.identity);
                node.transform.localScale = new Vector3(desiredSize / 1.9f, desiredSize / 1.9f, 1);
                node.id = Vertex.Count;
                Vertex.Add(node);
            }
        }

        for (int i = 0; i < Vertex.Count; i++)
        {
            //краевые узлы, находящиеся на углах лабиринта
            if (i == 0)
            {
                Vertex[i].Neighbors.Add(Vertex[i + 1]);
                Vertex[i].Neighbors.Add(Vertex[i + vertices]);
            }
            else if (i == vertices - 1)
            {
                Vertex[i].Neighbors.Add(Vertex[i - 1]);
                Vertex[i].Neighbors.Add(Vertex[i + vertices]);
            }
            else if (i == (vertices - 1) * vertices)
            {
                Vertex[i].Neighbors.Add(Vertex[i - vertices]);
                Vertex[i].Neighbors.Add(Vertex[i + 1]);
            }
            else if (i == (vertices - 1) * vertices + vertices - 1)
            {
                Vertex[i].Neighbors.Add(Vertex[i - 1]);
                Vertex[i].Neighbors.Add(Vertex[i - vertices]);
            }

            //узлы, образующие внешние стены лабиринта, кроме краевых
            else if (i > 0 && i < vertices - 1)
            {
                Vertex[i].Neighbors.Add(Vertex[i - 1]);
                Vertex[i].Neighbors.Add(Vertex[i + 1]);
                Vertex[i].Neighbors.Add(Vertex[i + vertices]);
            }
            else if (i < vertices * vertices - 1 &&
                    i > vertices * vertices - vertices)
            {
                Vertex[i].Neighbors.Add(Vertex[i + 1]);
                Vertex[i].Neighbors.Add(Vertex[i - 1]);
                Vertex[i].Neighbors.Add(Vertex[i - vertices]);
            }
            else if (i % vertices == 0)
            {
                Vertex[i].Neighbors.Add(Vertex[i - vertices]);
                Vertex[i].Neighbors.Add(Vertex[i + 1]);
                Vertex[i].Neighbors.Add(Vertex[i + vertices]);
            }
            else if ((i - vertices + 1) % vertices == 0)
            {
                Vertex[i].Neighbors.Add(Vertex[i - vertices]);
                Vertex[i].Neighbors.Add(Vertex[i - 1]);
                Vertex[i].Neighbors.Add(Vertex[i + vertices]);
            }

            //внутренние узлы, не образующие внешние стены лабиринта
            else
            {
                Vertex[i].Neighbors.Add(Vertex[i - vertices]);
                Vertex[i].Neighbors.Add(Vertex[i - 1]);
                Vertex[i].Neighbors.Add(Vertex[i + 1]);
                Vertex[i].Neighbors.Add(Vertex[i + vertices]);
            }
        }
    }

    public void Renew() //renew the graph
    {
        foreach(Node element in Vertex) //for each vertex
        {
            element.visited = false; //make them unvisited 
            foreach(Transform child in element.transform) //for every wall of the vertex
            {
                child.gameObject.SetActive(true); //make the wall visible again
            }
        }
    }
    public void RDFS(int start)
    {
        Stack<Node> stack = new();
        Vertex[start].visited = true;           //Choose a starting point and mark it visited

        //print("Starting point for RDFS is " + start);

        stack.Push(Vertex[start]);              //Add it to the stack
        while (stack.Count > 0)                 //While stack isn't empty
        {
            Node current = stack.Pop();         //Delete a node from the stack and make it current
            if(current.Neighbors.Any(p => p.visited == false)) //if current has unvisited neighbors
            {
                stack.Push(current);            //Add current to the stack
                int chosenIndex = UnityEngine.Random.Range(0, current.Neighbors.Count);//Randomly choose unvisited neighbor
                while (current.Neighbors[chosenIndex].visited)
                {
                    chosenIndex = UnityEngine.Random.Range(0, current.Neighbors.Count);
                }
                Node chosen = current.Neighbors[chosenIndex];
                current.EraseSide(chosen);  //Delete a wall between current and chosen
                chosen.visited = true;      //Mark chosen as visited
                stack.Push(chosen);         //Add chosen to the stack
            }
        }
        print("RDFS complete");
    }
}
