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
        rows = cols * 3-1;
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        screenWidth = 2f * (screenBounds.x - padding);
        screenHeight = 2f * screenBounds.y;
        desiredSize = screenWidth / (3 * cols - 1);
        hexagonHeight = desiredSize * Mathf.Sqrt(3);
        Vertex = new List<Node>();
        for (int i = 0; i < rows; i++)
        {
            float xOffset = (i % 2 == 0) ? desiredSize : 2.5f * desiredSize;
            int colsInRow = (i % 2 == 0) ? cols : cols - 1;
            for(int j = 0; j < colsInRow; j++)
            {
                Vector2 position = new(
                    -screenWidth / 2f + xOffset + j * desiredSize * 3f,
                    screenHeight * 0.2f - i * (hexagonHeight / 2f));
                HexagonNode node = Instantiate(hexagonNodePrefab, position, Quaternion.identity, parent);
                node.transform.localScale = new Vector3(desiredSize / 2f, desiredSize / 2f, 1);
                node.id = Vertex.Count;
                Vertex.Add(node);
            }
        }
        int currentIndex = 0; //индекс текущего узла в списке Vertex
        for(int row = 0; currentIndex < Vertex.Count; row++)
        {
            bool isEvenRow = row % 2 == 0; //четный или нечетный ряд
            int colsInRow = isEvenRow ? cols : cols - 1; //в четном ряду cols шестиугольников, в нечетном - cols-1
            for(int col = 0; col < colsInRow; col++)
            {
                Node current = Vertex[currentIndex];
                if (col > 0) //левый сосед
                    current.Neighbors.Add(Vertex[currentIndex - 1]);
                if (col < colsInRow - 1) //правый сосед
                    current.Neighbors.Add(Vertex[currentIndex + 1]);
                if (row > 0)
                {
                    int upperCols = isEvenRow ? cols - 1 : cols; //сколько шестиугольников в верхней строке
                    int upperRowStart = currentIndex - colsInRow - upperCols; //индекс первого шестиугольника в верхней строке
                    if (col < upperCols)
                        current.Neighbors.Add(Vertex[upperRowStart + col]);//верхний правый сосед
                    if (col > 0)
                        current.Neighbors.Add(Vertex[upperRowStart + col - 1]);//верхний левый сосед
                }
                if (currentIndex + colsInRow < Vertex.Count)
                {
                    int lowerCols = isEvenRow ? cols - 1 : cols; //сколько шестиугольников в нижней строке
                    int lowerRowStart = currentIndex + colsInRow; //индекс первого шестиугольника в нижней строке
                    if (col < lowerCols)
                        current.Neighbors.Add(Vertex[lowerRowStart + col]);//нижний правый сосед
                    if(col>0)
                        current.Neighbors.Add(Vertex[lowerRowStart+col-1]);//нижний левый сосед
                }
                currentIndex++;
            }
            
        }
    }
}
