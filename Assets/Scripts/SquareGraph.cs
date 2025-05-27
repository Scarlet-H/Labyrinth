using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SquareGraph : Graph
{
    public int vertices;
    public Node squareNodePrefab;
    public Transform parent;
    public override void InitializeGraph()
    {
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - padding;
        desiredSize = 2f * screenWidth / vertices;
        Vertex = new List<Node>();
        for (int i = 0; i < vertices; i++)
        {
            for (int j = 0; j < vertices; j++)
            {
                Node node = Instantiate(squareNodePrefab, new Vector2(
                    -screenWidth + desiredSize / 2f + j * desiredSize,
                    screenWidth - desiredSize / 2f - i * desiredSize),
                    Quaternion.identity, parent);
                node.transform.localScale = new Vector3(desiredSize / 2f, desiredSize / 2f, 1);
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
    public void BinaryTree()
    {
        for (int i = 0; i < Vertex.Count; i++)
        {
            Node current = Vertex[i]; //текущая вершина
            Node east = (i % vertices != vertices - 1) ? Vertex[i + 1] : null; //если нет восточного соседа, то null
            Node south = (i < Vertex.Count - vertices) ? Vertex[i + vertices] : null; //если нет южного соседа, то null
            if (east != null && south != null)
            {
                Node chosen = UnityEngine.Random.value < 0.5f ? east : south; //выбираем либо восток либо юг
                current.EraseSide(chosen);
            }
            else if (east != null) //если есть только восток - выбираем его
            {
                current.EraseSide(east);
            }
            else if (south != null) //если есть только юг - выбираем его
            {
                current.EraseSide(south);
            }
        }
    }
    public void Eller()
    {
        for (int j = 0; j < vertices; j++) //инициализация  строки j-row, i-col
        {
            List<Node> currentRow = new List<Node>();
            for (int i = 0; i < vertices; i++)
            {
                Node currentNode = Vertex[i + j * vertices];
                if(currentNode.set == null) //если у текущей вершины нет множества
                {
                    currentNode.set = new HashSet<Node> { currentNode }; //инициализируем уникальное множество
                }
                currentRow.Add(currentNode);
            }
            for (int i = 0; i < vertices - 1; i++)
            {
                if (currentRow[i].set != currentRow[i + 1].set) //если множества i-й и i+1-й вершин не совпадают
                {
                    if (UnityEngine.Random.Range(0, 2) == 0)
                    {
                        currentRow[i].EraseSide(currentRow[i + 1]);
                        currentRow[i].set.UnionWith(currentRow[i+1].set); //объединяем первое множество со вторым
                        foreach(var node in currentRow[i + 1].set) //обновление ссылок для второго множества
                        {
                            node.set = currentRow[i].set;
                        }
                    }
                }
            }
            if (j < vertices - 1) //генерируем вертикальные проходы если строка не последняя
            {
                var setsInRow = currentRow //группировка вершин по множествам в одной строке
                .Select(node => node.set)
                .Distinct()
                .ToList();
                foreach (var nodeSet in setsInRow) //для каждого множества в ряду
                {
                    var nodes = nodeSet //из множества выбираем вершины только из текущего ряда
                        .Where(node => currentRow.Contains(node))
                        .ToList();
                    nodes = nodes //перемешиваем вершины, чтобы удалять стены случайным образом
                        .OrderBy(_ => UnityEngine.Random.value).ToList();
                    //случайно определяем сколько стен снизу удалить
                    //удаляем как минимум 1 стену
                    int connectionsCount = UnityEngine.Random.Range(1, nodes.Count + 1);
                    for (int i = 0; i < connectionsCount; i++)
                    {
                        Node node = nodes[i];
                        Node bottomNode = Vertex[node.id + vertices];
                        if (node.set != bottomNode.set)
                        {
                            node.EraseSide(bottomNode);
                            node.set.Add(bottomNode);
                            bottomNode.set = node.set;
                        }
                    }
                }
            }
            else //последняя строка - нужно соединить все смежные вершины из разных множеств
            {
                for(int i = 0; i < vertices-1; i++)
                {
                    Node currentNode = Vertex[i + vertices * (vertices - 1)];
                    Node rightNeighbor = Vertex[i +1+ vertices * (vertices - 1)];
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
