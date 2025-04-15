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
    public List<Node> Vertex;
    protected float desiredSize;
    protected float screenWidth;
    protected float screenHeight;
    public readonly float padding = 0.5f;
    public virtual void InitializeGraph() { }

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
