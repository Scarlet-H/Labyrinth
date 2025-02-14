using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleGraph : MonoBehaviour
{
    public int rows, cols; // Количество строк и столбцов
    public List<Node> Vertex;
    public TriangleNode triangleNodePrefab;
    float screenSize;
    readonly float padding = 0.1f;
    float triangleHeight, desiredSize;
    public void InitializeGraph()
    {
        cols = rows * 2-1;
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - padding;
        desiredSize = 2f*screenSize /rows;
        triangleHeight = Mathf.Sqrt(3) * desiredSize / 2f ; // Высота равностороннего треугольника

        Vertex = new List<Node>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                bool isUp = (i + j) % 2 == 0; // Верхний или нижний треугольник
                Vector2 position = new(-screenSize+desiredSize/2f + j * desiredSize/2f, screenSize - 1.2f*desiredSize - i * triangleHeight + (isUp?0: triangleHeight));
                TriangleNode node = Instantiate(triangleNodePrefab, position, isUp ? Quaternion.identity : Quaternion.Euler(0, 0, 180));
                node.transform.localScale = new Vector3(desiredSize / 1.9f, desiredSize / 1.9f, 1);
                node.id = Vertex.Count;
                node.isUp = isUp;
                Vertex.Add(node);
            }
        }

        // Определяем соседей
        for (int i = 0; i < Vertex.Count; i++)
        {
            int row = i / cols;
            int col = i % cols;
            bool isUp = (row + col) % 2 == 0; // Верхний треугольник 

            if (isUp) // Верхний 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); // Левый (нижний )
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); // Правый (нижний )
                if (row < rows - 1) Vertex[i].Neighbors.Add(Vertex[i + cols]); // Нижний (нижний )
            }
            else // Нижний 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); // Левый (верхний )
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); // Правый (верхний )
                if (row > 0) Vertex[i].Neighbors.Add(Vertex[i - cols]); // Верхний (верхний )
            }
        }
    }
    public void Renew() //renew the graph
    {
        foreach (Node element in Vertex) //for each vertex
        {
            element.visited = false; //make them unvisited 
            foreach (Transform child in element.transform) //for every wall of the vertex
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
            if (current.Neighbors.Any(p => p.visited == false)) //if current has unvisited neighbors
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
