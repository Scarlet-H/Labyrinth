using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> Neighbors = new();
    public int id;
    public bool visited = false;

    public virtual void EraseSide(Node second)
    {
        if(transform.position.x < second.transform.position.x)
        {
            transform.Find("RightWall").gameObject.SetActive(false);
            second.transform.Find("LeftWall").gameObject.SetActive(false);
        } 
        else if(transform.position.x > second.transform.position.x)
        {
            transform.Find("LeftWall").gameObject.SetActive(false);
            second.transform.Find("RightWall").gameObject.SetActive(false);
        } 
        else if (transform.position.y < second.transform.position.y)
        {
            transform.Find("UpWall").gameObject.SetActive(false);
            second.transform.Find("DownWall").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("DownWall").gameObject.SetActive(false);
            second.transform.Find("UpWall").gameObject.SetActive(false);
        }
    }
}
