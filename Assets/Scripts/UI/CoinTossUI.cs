using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTossUI : MonoBehaviour
{
    public Sprite successSprite; // ���� �� ǥ���� ��������Ʈ
    public Sprite failureSprite; // ���� �� ǥ���� ��������Ʈ

    public Image[] coinImages; // ���� ��� ǥ�� 

    public void InitCoinView()
    {
        for (int i = 0; i < coinImages.Length; i++)
        {
            coinImages[i].sprite = successSprite;// ���°�
            coinImages[i].color = Color.black;
        }
    }
    public void ShowCoinResult(List<bool> results)
    {
        for (int i = results.Count; i < coinImages.Length; i++)
        {
            coinImages[i].sprite = successSprite;// ���°�
            coinImages[i].color = Color.black;
        }
        Debug.Log("����� ���� : " + results.Count);
        int idx = 0;
        foreach(bool result in results)
        {
           
            if (result) //  ����
            {
                coinImages[idx].sprite = successSprite; //���� ��
                coinImages[idx].color = Color.green; // ���� �ʷϻ�
                Debug.Log("���� �� Ȱ��");
            }
            else
            {
                coinImages[idx].sprite = failureSprite; // ����
                coinImages[idx].color = Color.red; // ���� ������
                Debug.Log("���� �� Ȱ��");
            }
            idx++;
        }
        
    }
    
}
