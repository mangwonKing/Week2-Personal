using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinTossUI : MonoBehaviour
{
    public Sprite successSprite; // 성공 시 표시할 스프라이트
    public Sprite failureSprite; // 실패 시 표시할 스프라이트

    public Image[] coinImages; // 동전 결과 표시 

    public void InitCoinView()
    {
        for (int i = 0; i < coinImages.Length; i++)
        {
            coinImages[i].sprite = successSprite;// 없는거
            coinImages[i].color = Color.black;
        }
    }
    public void ShowCoinResult(List<bool> results)
    {
        for (int i = results.Count; i < coinImages.Length; i++)
        {
            coinImages[i].sprite = successSprite;// 없는거
            coinImages[i].color = Color.black;
        }
        Debug.Log("결과물 길이 : " + results.Count);
        int idx = 0;
        foreach(bool result in results)
        {
           
            if (result) //  성공
            {
                coinImages[idx].sprite = successSprite; //성공 원
                coinImages[idx].color = Color.green; // 성공 초록색
                Debug.Log("성공 원 활성");
            }
            else
            {
                coinImages[idx].sprite = failureSprite; // 실패
                coinImages[idx].color = Color.red; // 실패 빨간색
                Debug.Log("실패 원 활성");
            }
            idx++;
        }
        
    }
    
}
