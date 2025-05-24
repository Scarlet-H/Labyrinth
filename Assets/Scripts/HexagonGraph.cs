using System.Collections.Generic;
using UnityEngine;

public class HexagonGraph : Graph
{
    public int cols;
    private int rows;
    public HexagonNode hexagonNodePrefab;
    public Transform parent;
    float hexagonHeight;
    public override void InitializeGraph()
    {
        int colsUneven = cols;
        int colsEven = cols - 1;
        rows = cols * 3-1;
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        screenWidth = 2f * (screenBounds.x - padding);
        screenHeight = 2f * screenBounds.y;
        desiredSize = screenWidth / (3 * cols - 1);
        hexagonHeight = desiredSize * Mathf.Sqrt(3);
        Vertex = new List<Node>();
        for (int i = 0; i < rows; i++)
        {
            if (i % 2 == 0)
            {
                cols = colsUneven; //столбцов нечетное количество, но это четные столбцы
                for (int j = 0; j < cols; j++)
                {
                    Vector2 position = new(
                        -screenWidth / 2f + desiredSize + j * desiredSize * 3f,
                        screenHeight * 0.2f - i * (hexagonHeight / 2f)
                    );
                    HexagonNode node = Instantiate(hexagonNodePrefab, position, Quaternion.identity, parent);
                    node.transform.localScale = new Vector3(desiredSize / 2f, desiredSize / 2f, 1);
                    node.id = Vertex.Count;
                    Vertex.Add(node);
                }
            }
            else
            {
                cols = colsEven;
                for (int j = 0; j < cols; j++)
                {
                    Vector2 position = new(
                        -screenWidth / 2f + 2.5f * desiredSize + j * desiredSize * 3f,
                        screenHeight * 0.2f - i * (hexagonHeight / 2f)
                    );
                    HexagonNode node = Instantiate(hexagonNodePrefab, position, Quaternion.identity, parent);
                    node.transform.localScale = new Vector3(desiredSize / 2f, desiredSize / 2f, 1);
                    node.id = Vertex.Count;
                    Vertex.Add(node);
                }
            }
        }
        for (int i = 0; i < Vertex.Count; i++)
        {
            //углы лабиринта (2 соседа)
            if (i == 0)
            {
                Vertex[i].Neighbors.Add(Vertex[i + cols]);
                Vertex[i].Neighbors.Add(Vertex[i + cols + cols - 1]);
            }
            else if (i == cols - 1)
            {
                Vertex[i].Neighbors.Add(Vertex[i + cols - 1]);
                Vertex[i].Neighbors.Add(Vertex[i + cols + cols - 1]);
            }
            else if (i == Vertex.Count - 1)
            {
                Vertex[i].Neighbors.Add(Vertex[i - cols]);
                Vertex[i].Neighbors.Add(Vertex[i - cols - cols + 1]);
            }
            else if (i == Vertex.Count - cols)
            {
                Vertex[i].Neighbors.Add(Vertex[i - cols + 1]);
                Vertex[i].Neighbors.Add(Vertex[i - cols + 1 - cols]);
            }

            //верхний и нижний ряд (3 соседа)
            else if (i > 0 && i < cols - 1)
            {
                Vertex[i].Neighbors.Add(Vertex[i + cols - 1]);
                Vertex[i].Neighbors.Add(Vertex[i + cols]);
                Vertex[i].Neighbors.Add(Vertex[i + 2 * cols - 1]);
            }
            else if (i < Vertex.Count - 1 && i > Vertex.Count - cols)
            {
                Vertex[i].Neighbors.Add(Vertex[i - cols]);
                Vertex[i].Neighbors.Add(Vertex[i - cols + 1]);
                Vertex[i].Neighbors.Add(Vertex[i - 2 * cols + 1]);
            }

            //левый и правый ряд (4 соседа)
            else if (i % (colsEven + colsUneven) == 0)
            {
                Vertex[i].Neighbors.Add(Vertex[i + cols]);
                Vertex[i].Neighbors.Add(Vertex[i + colsEven + colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsEven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsEven - colsUneven]);
            }
            else if (i % (colsEven + colsUneven) == colsEven)
            {
                Vertex[i].Neighbors.Add(Vertex[i + colsEven]);
                Vertex[i].Neighbors.Add(Vertex[i + colsEven + colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsEven - colsUneven]);
            }

            //верхний второй и нижний предпоследний ряды (5 соседей)
            else if (i >= cols && i < cols + colsEven)
            {
                Vertex[i].Neighbors.Add(Vertex[i-cols]);
                Vertex[i].Neighbors.Add(Vertex[i-cols+1]);
                Vertex[i].Neighbors.Add(Vertex[i+colsEven]);
                Vertex[i].Neighbors.Add(Vertex[i+colsEven+1]);
                Vertex[i].Neighbors.Add(Vertex[i+colsEven+colsUneven]);
            }
            else if (i < Vertex.Count - cols && i >= Vertex.Count - cols - colsEven)
            {
                Vertex[i].Neighbors.Add(Vertex[i + colsEven]);
                Vertex[i].Neighbors.Add(Vertex[i + colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsEven]);
                Vertex[i].Neighbors.Add(Vertex[i -  colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsEven - colsUneven]);
            }

            //внутренние (6 соседей)
            else
            {
                Vertex[i].Neighbors.Add(Vertex[i + colsEven]);
                Vertex[i].Neighbors.Add(Vertex[i + colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsEven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i - colsEven - colsUneven]);
                Vertex[i].Neighbors.Add(Vertex[i + colsEven + colsUneven]);
            }
        }
    }
}
