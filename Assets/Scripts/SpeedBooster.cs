using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    public void Spawn(int spawnPoint, List<Node> _Vertex)
    {
        gameObject.SetActive(true);
        transform.position = _Vertex[spawnPoint].transform.position;
    }
}
