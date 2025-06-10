using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonGraph : Graph
{
    public int cols;
    private int rows;
    private int allCols;
    public HexagonNode hexagonNodePrefab;
    public Transform parent;
    float hexagonHeight;
    public override void InitializeGraph()
    {
        rows = cols * 2 - 2;
        allCols = cols + (cols - 1);
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        screenWidth = 2f * (screenBounds.x - padding);
        screenHeight = 2f * screenBounds.y;
        desiredSize = screenWidth / (3 * cols - 1);
        hexagonHeight = desiredSize * Mathf.Sqrt(3);
        Vertex = new List<Node>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector2 topPosition = new(
                    -screenWidth / 2f + desiredSize + j * desiredSize * 3f,
                    screenHeight * 0.2f - i * hexagonHeight);
                HexagonNode top = Instantiate(hexagonNodePrefab, topPosition, Quaternion.identity, parent);
                top.transform.localScale = new Vector3(desiredSize / 2f, desiredSize / 2f, 1);
                top.id = Vertex.Count;
                Vertex.Add(top);
                if (j < cols - 1)
                {
                    Vector2 bottomPosition = new(
                        -screenWidth / 2f + j * desiredSize * 3f + 2.5f * desiredSize,
                        screenHeight * 0.2f - i * hexagonHeight - hexagonHeight / 2f);
                    HexagonNode bottom = Instantiate(hexagonNodePrefab, bottomPosition, Quaternion.identity, parent);
                    bottom.transform.localScale = new Vector3(desiredSize / 2f, desiredSize / 2f, 1);
                    bottom.id = Vertex.Count;
                    Vertex.Add(bottom);
                }
            }
        }
        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < allCols; j++)
            {
                Node current = Vertex[index];
                if (j < allCols - 1) //����� ������ ������ (�������-�������, ������-��������)
                {
                    current.Neighbors.Add(Vertex[index + 1]);
                }
                if (j > 0)//����� ������ ����� (�������-�������, ������-��������)
                {
                    current.Neighbors.Add(Vertex[index - 1]);
                    if (i > 0)
                        if ((index % allCols) % 2 == 0) //�������
                            current.Neighbors.Add(Vertex[index - allCols - 1]); //������� ����� ��� �������
                }
                if (i < rows - 1) //����� ������ �����
                {
                    current.Neighbors.Add(Vertex[index + allCols]);
                    if ((index % allCols) % 2 != 0) //������
                        current.Neighbors.Add(Vertex[index + allCols + 1]); //������ ������ ��� ������
                    if (j > 0)
                        if ((index % allCols) % 2 != 0) //������
                            current.Neighbors.Add(Vertex[index + allCols - 1]); //������ ����� ��� ������
                }
                if (i > 0) //����� ������ ������
                {
                    current.Neighbors.Add(Vertex[index - allCols]);
                    if (j < allCols - 1)
                        if ((index % allCols) % 2 == 0) //�������
                            current.Neighbors.Add(Vertex[index - allCols + 1]); //������� ������ ��� �������
                }
                index++;
            }
        }
    }
    public override void BinaryTree()
    {
        for (int i = 0; i < Vertex.Count; i++)
        {
            Node current = Vertex[i]; //������� �������
            Node south_east;
            Node south;
            Node north_east = null;
            if ((i % allCols) % 2 == 0) //�������
            {
                south_east = (i % allCols != 0) ? Vertex[i - 1] : null; //���� ��� ���-���������� ������, �� null
                south = (i < Vertex.Count - allCols) ? Vertex[i + allCols] : null; //���� ��� ������ ������, �� null
            }
            else //������
            {
                south_east = (i < Vertex.Count - allCols) ? Vertex[i + allCols - 1] : null; //���� ��� ���-���������� ������, �� null
                south = (i < Vertex.Count - allCols) ? Vertex[i + allCols] : null; //���� ��� ������ ������, �� null
                north_east = (i > Vertex.Count - allCols) ? Vertex[i - 1] : null;
            }
            if (south_east != null && south != null)
            {
                Node chosen = UnityEngine.Random.value < 0.5f ? south_east : south; //�������� ���� ���-������ ���� ��
                current.EraseSide(chosen);
            }
            else if (south_east != null) //���� ���� ������ ���-������� - �������� ���
            {
                current.EraseSide(south_east);
            }
            else if (south != null) //���� ���� ������ �� - �������� ���
            {
                current.EraseSide(south);
            }
            else if (north_east != null)
            {
                current.EraseSide(north_east);
            }
        }
        print("Binary Tree complete");
    }
    public override void Eller()
    {
        for (int row = 0; row < rows; row++)
        {
            List<Node> currentRow = new();
            //�������� ������� ������
            for (int col = 0; col < allCols; col++) //�������� �� ��������
            {
                Node currentNode = Vertex[col + row * allCols];
                //���� ���� �� ����������� ��������� - ������� �����
                if (currentNode.set == null)
                {
                    currentNode.set = new HashSet<Node> { currentNode };
                }
                currentRow.Add(currentNode);
            }
            //�������������� ����������
            for (int i = 0; i < allCols - 1; i++)
            {
                if (currentRow[i].set != currentRow[i + 1].set) //���� ��������� i-� � i+1-� ������ �� ���������
                {
                    if (UnityEngine.Random.Range(0, 1) == 0)
                    {
                        currentRow[i].EraseSide(currentRow[i + 1]);
                        currentRow[i].set.UnionWith(currentRow[i + 1].set); //���������� ������ ��������� �� ������
                        foreach (var node in currentRow[i + 1].set) //���������� ������ ��� ������� ���������
                        {
                            node.set = currentRow[i].set;
                        }
                    }
                }
            }
            //������������ ����������, ���� ������ �� ���������
            if (row < rows - 1)
            {
                var setsInRow = currentRow //����������� ������ �� ���������� � ����� ������
                    .Select(node => node.set)
                    .Distinct()
                    .ToList();
                foreach (var nodeSet in setsInRow) //��� ������� ��������� � ����
                {
                    var nodes = nodeSet //�� ��������� �������� ������� ������ �� �������� ����
                        .Where(node => currentRow.Contains(node))
                        .ToList();
                    nodes = nodes //������������ �������, ����� ������� ����� ��������� �������
                        .OrderBy(_ => UnityEngine.Random.value).ToList();
                    //�������� ���������� ������� ���� ����� �������
                    //������� ��� ������� 1 �����
                    int connectionsCount = UnityEngine.Random.Range(1, nodes.Count + 1);
                    for (int i = 0; i < connectionsCount; i++)
                    {
                        Node node = nodes[i];
                        Node bottomNode = Vertex[node.id + allCols];
                        if (node.set != bottomNode.set)
                        {
                            node.EraseSide(bottomNode);
                            node.set.Add(bottomNode);
                            bottomNode.set = node.set;
                        }
                    }
                }
            }
            else //��������� ������ - ����� ��������� ��� ������� ������� �� ������ ��������
            {
                
                for (int i = 0; i < allCols - 1; i++)
                {
                    Node currentNode = Vertex[i + allCols * row];
                    Node rightNeighbor = Vertex[i + 1 + allCols * row];
                    if (currentNode.set != rightNeighbor.set)
                    {
                        currentNode.EraseSide(rightNeighbor);
                        currentNode.set.UnionWith(rightNeighbor.set);
                        foreach (var node in rightNeighbor.set)
                        {
                            node.set = currentNode.set;
                        }
                    }
                }
            }
        }
        print("Eller complete");
    }
}
