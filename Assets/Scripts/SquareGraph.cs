using System.Collections.Generic;
using UnityEngine;

public class SquareGraph : Graph
{
    public int vertices;
    public Node squareNodePrefab;

    public override void InitializeGraph()
    {
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - padding;
        desiredSize = 2f * screenWidth / vertices;
        Vertex = new List<Node>();
        for (int i = 0; i < vertices; i++)
        {
            for (int j = 0; j < vertices; j++)
            {
                Node node = Instantiate(squareNodePrefab, new Vector2(-screenWidth + desiredSize / 2f + j * desiredSize, screenWidth - desiredSize / 2f - i * desiredSize), Quaternion.identity);
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

}
