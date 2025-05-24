using UnityEngine;

public class HexagonNode : Node
{
    public override void EraseSide(Node second)
    {
        if (transform.position.y < second.transform.position.y) // ������ ����
        {
            if (transform.position.x < second.transform.position.x) //������ ������-������
            {
                transform.Find("TopRight").gameObject.SetActive(false);
                second.transform.Find("BottomLeft").gameObject.SetActive(false);
            }
            else if (transform.position.x > second.transform.position.x) // ������ ������-�����
            {
                transform.Find("TopLeft").gameObject.SetActive(false);
                second.transform.Find("BottomRight").gameObject.SetActive(false);
            }
            else //������ ������
            {
                transform.Find("Top").gameObject.SetActive(false);
                second.transform.Find("Bottom").gameObject.SetActive(false);
            }
        }
        else if (transform.position.y > second.transform.position.y)
        {
            if (transform.position.x < second.transform.position.x) // ������ �����-������
            {
                transform.Find("BottomRight").gameObject.SetActive(false);
                second.transform.Find("TopLeft").gameObject.SetActive(false);
            }
            else if (transform.position.x > second.transform.position.x) // ������ �����-�����
            {
                transform.Find("BottomLeft").gameObject.SetActive(false);
                second.transform.Find("TopRight").gameObject.SetActive(false);
            }
            else //������ �����
            {
                transform.Find("Bottom").gameObject.SetActive(false);
                second.transform.Find("Top").gameObject.SetActive(false);
            }
        }
    }
}
