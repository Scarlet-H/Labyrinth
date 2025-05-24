using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleGraph : Graph
{
    public int rows, cols; // Количество строк и столбцов
    public TriangleNode triangleNodePrefab;
    float triangleHeight;
    public Transform parent;
    public override void InitializeGraph()
    {
        rows = (cols+3)/2;
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        screenWidth = 2f * (screenBounds.x - padding);
        screenHeight = 2f * screenBounds.y;
        desiredSize = screenWidth / ((cols+1)/2);
        triangleHeight = Mathf.Sqrt(3) * desiredSize/2f ; // Высота равностороннего треугольника

        Vertex = new List<Node>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                bool isUp = (i + j) % 2 == 0; // Верхний или нижний треугольник

                Vector2 position = new(
                    -screenWidth/2f + desiredSize/2f  + j * desiredSize/2f,
                    screenHeight *0.2f - i*triangleHeight + (isUp ? 0 : triangleHeight/3f)
                );
                TriangleNode node = Instantiate(triangleNodePrefab, position, isUp ? Quaternion.identity : Quaternion.Euler(0, 0, 180), parent);
                node.transform.localScale = new Vector3(desiredSize/2f, desiredSize/2f, 1);
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

            if (isUp) //верхний 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); //левый (нижний)
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); //правый (нижний)
                if (row < rows - 1) Vertex[i].Neighbors.Add(Vertex[i + cols]); //нижний (нижний)
            }
            else //нижний 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); //левый (верхний)
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); //правый (верхний)
                if (row > 0) Vertex[i].Neighbors.Add(Vertex[i - cols]); //верхний (верхний)
            }
        }
    }
}
