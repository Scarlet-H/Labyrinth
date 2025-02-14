using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        float screenHeight = 2f * Camera.main.orthographicSize;
        float screenWidth = screenHeight * Screen.width / Screen.height;
        float scaleX = screenWidth / 1080f * 185f;
        float scaleY = screenHeight / 1920f * 185f;
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
    public void Spawn(int spawnPoint, List<Node> _Vertex)
    {
        gameObject.SetActive(true);
        transform.position = _Vertex[spawnPoint].transform.position;
    }
}
