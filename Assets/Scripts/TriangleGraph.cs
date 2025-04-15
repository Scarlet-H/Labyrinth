using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleGraph : Graph
{
    public int rows, cols; // ���������� ����� � ��������
    public TriangleNode triangleNodePrefab;
    float triangleHeight;
    public override void InitializeGraph()
    {
        rows = cols-cols/3;
        print(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x);
        screenWidth = 2f*(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x-padding);
        screenHeight = 2f *Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        desiredSize = screenWidth / cols;
        triangleHeight = Mathf.Sqrt(3) * desiredSize/2f ; // ������ ��������������� ������������

        Vertex = new List<Node>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                bool isUp = (i + j) % 2 == 0; // ������� ��� ������ �����������

                Vector2 position = new(
                    -screenWidth / 2f+desiredSize/2f + j*desiredSize,
                    1.75f -i * 2f*triangleHeight + (isUp?0: triangleHeight)
                );
                TriangleNode node = Instantiate(triangleNodePrefab, position, isUp ? Quaternion.identity : Quaternion.Euler(0, 0, 180));
                node.transform.localScale = new Vector3(desiredSize, desiredSize, 1);
                node.id = Vertex.Count;
                node.isUp = isUp;
                Vertex.Add(node);
            }
        }

        // ���������� �������
        for (int i = 0; i < Vertex.Count; i++)
        {
            int row = i / cols;
            int col = i % cols;
            bool isUp = (row + col) % 2 == 0; // ������� ����������� 

            if (isUp) // ������� 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); // ����� (������ )
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); // ������ (������ )
                if (row < rows - 1) Vertex[i].Neighbors.Add(Vertex[i + cols]); // ������ (������ )
            }
            else // ������ 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); // ����� (������� )
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); // ������ (������� )
                if (row > 0) Vertex[i].Neighbors.Add(Vertex[i - cols]); // ������� (������� )
            }
        }
        print("initialized triangle graph");
    }
}
