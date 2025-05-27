using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class TriangleNode : Node
{
    public bool isUp; // Верхний или нижний треугольник
    public override void EraseSide(Node second)
    {
        if (isUp)
        {
            if (transform.position.x < second.transform.position.x)
            {
                transform.Find("RightWall").
                    gameObject.SetActive(false);
                second.transform.Find("RightWall").
                    gameObject.SetActive(false);
            }
            else if (transform.position.x > second.transform.position.x)
            {
                transform.Find("LeftWall").
                    gameObject.SetActive(false);
                second.transform.Find("LeftWall").
                    gameObject.SetActive(false);
            }
            else
            {
                transform.Find("DownWall").
                    gameObject.SetActive(false);
                second.transform.Find("DownWall").
                    gameObject.SetActive(false);
            }
        }
        else
        {
            if (transform.position.x < second.transform.position.x)
            {
                transform.Find("LeftWall").
                    gameObject.SetActive(false);
                second.transform.Find("LeftWall").
                    gameObject.SetActive(false);
            }
            else if (transform.position.x > second.transform.position.x)
            {
                transform.Find("RightWall").
                    gameObject.SetActive(false);
                second.transform.Find("RightWall").
                    gameObject.SetActive(false);
            }
            else
            {
                transform.Find("DownWall").
                    gameObject.SetActive(false);
                second.transform.Find("DownWall").
                    gameObject.SetActive(false);
            }
        }
    }
}
