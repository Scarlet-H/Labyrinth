using System.Collections.Generic;
using UnityEngine;

public class Friend : MonoBehaviour
{
    private float referenceScale = 0.08f;
    private void Start()
    {
        float targetHeightUnits = 10f;
        float worldHeight = Camera.main.orthographicSize * 2f;
        float scaleFactor = worldHeight / targetHeightUnits;
        transform.localScale = Vector3.one * (referenceScale * scaleFactor);
    }
    public void Spawn(int spawnPoint, List<Node> _Vertex)
    {
        gameObject.SetActive(true);
        transform.position = _Vertex[spawnPoint].transform.position;
    }
}
