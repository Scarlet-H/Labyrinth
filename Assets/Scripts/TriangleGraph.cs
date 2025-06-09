using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleGraph : Graph
{
    public int rows, cols; // ���������� ����� � ��������
    public TriangleNode triangleNodePrefab;
    float triangleHeight;
    public Transform parent;
    public override void InitializeGraph()
    {
        rows = (cols + 3) / 2;
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        screenWidth = 2f * (screenBounds.x - padding);
        screenHeight = 2f * screenBounds.y;
        desiredSize = screenWidth / ((cols + 1) / 2);
        triangleHeight = Mathf.Sqrt(3) * desiredSize / 2f; // ������ ��������������� ������������

        Vertex = new List<Node>();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                bool isUp = (i + j) % 2 == 0; // ������� ��� ������ �����������

                Vector2 position = new(
                    -screenWidth / 2f + desiredSize / 2f + j * desiredSize / 2f,
                    screenHeight * 0.2f - i * triangleHeight + (isUp ? 0 : triangleHeight / 3f)
                );
                TriangleNode node = Instantiate(triangleNodePrefab, position, isUp ? Quaternion.identity : Quaternion.Euler(0, 0, 180), parent);
                node.transform.localScale = new Vector3(desiredSize / 2f, desiredSize / 2f, 1);
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

            if (isUp) //������� 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); //����� (������)
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); //������ (������)
                if (row < rows - 1) Vertex[i].Neighbors.Add(Vertex[i + cols]); //������ (������)
            }
            else //������ 
            {
                if (col > 0) Vertex[i].Neighbors.Add(Vertex[i - 1]); //����� (�������)
                if (col < cols - 1) Vertex[i].Neighbors.Add(Vertex[i + 1]); //������ (�������)
                if (row > 0) Vertex[i].Neighbors.Add(Vertex[i - cols]); //������� (�������)
            }
        }
    }
    public void Eller()
    {
        for (int row = 0; row < rows; row++) //�������� �� ������ ������ �����
        {
            List<Node> currentRow = new List<Node>();
            //�������� ������� ������
            for (int col = 0; col < cols; col++) //�������� �� ��������
            {
                Node currentNode = Vertex[col + row * cols];
                //���� ���� �� ����������� ��������� - ������� �����
                if (currentNode.set == null)
                {
                    currentNode.set = new HashSet<Node> { currentNode };
                }
                currentRow.Add(currentNode);
            }
            //�������������� ����������
            for (int i = 0; i < cols - 1; i++)
            {
                if (currentRow[i].set != currentRow[i + 1].set)
                {
                    if (!((TriangleNode)currentRow[i]).isUp)
                    {
                        currentRow[i].EraseSide(currentRow[i + 1]);
                        currentRow[i].set.UnionWith(currentRow[i + 1].set);
                        foreach (var node in currentRow[i + 1].set)
                        {
                            node.set = currentRow[i].set;
                        }
                    }
                    else
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            currentRow[i].EraseSide(currentRow[i + 1]);
                            currentRow[i].set.UnionWith(currentRow[i + 1].set);
                            foreach (var node in currentRow[i + 1].set)
                            {
                                node.set = currentRow[i].set;
                            }
                        }
                    }
                }
            }
            //�������� �������������� ���������� ����� ������� ������������ ���������� �������
            if (!((TriangleNode)currentRow[cols - 1]).isUp)
            {
                if(currentRow[cols - 1].set!= currentRow[cols - 2].set)
                {
                    currentRow[cols - 1].EraseSide(currentRow[cols - 2]);
                    currentRow[cols - 1].set.UnionWith(currentRow[cols - 2].set);
                    foreach (var node in currentRow[cols - 2].set)
                    {
                        node.set = currentRow[cols - 1].set;
                    }
                }
            }
            //������������ ����������, ���� ������ �� ���������
            if (row < rows - 1)
            {
                //���������� ��������� �� ������� ������
                var setsInRow = currentRow
                    .Select(node => node.set)
                    .Distinct()
                    .ToList();
                foreach (var nodeSet in setsInRow)
                {
                    var nodes = nodeSet //�� ��������� �������� ������� ������ �� �������� ����
                        .Where(node => currentRow.Contains(node))
                        .Cast<TriangleNode>()
                        .Where(node => node.isUp) //������ ������� ������������
                        .ToList();
                    nodes = nodes
                        .OrderBy(_ => UnityEngine.Random.value).ToList(); //������������
                    if (nodes.Count == 0)
                        continue;
                    //�������� ��������� ���������� ���������� ����
                    int connectionsCount = UnityEngine.Random.Range(1, nodes.Count + 1);
                    for (int i = 0; i < connectionsCount; i++)
                    {
                        Node node = nodes[i];
                        Node bottomNode = Vertex[node.id + cols];
                        if (node.set != bottomNode.set)
                        {
                            node.EraseSide(bottomNode);
                            node.set.Add(bottomNode);
                            bottomNode.set = node.set;
                        }
                    }
                }
            }
            //��������� ������
            else
            {
                for (int i = 0; i < cols - 1; i++)
                {
                    Node currentNode = Vertex[i + cols * row];
                    Node rightNeighbor = Vertex[i + 1 + cols * row];
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
    }
}
