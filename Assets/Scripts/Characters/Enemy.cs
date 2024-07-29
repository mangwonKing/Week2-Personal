using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    bool isSelect = false;
    public override int GetSelectedCoinCount() //동전 일단 3개까지 
    {
        
        if(!isSelect)
        {
            isSelect = true;
            StartCoroutine(WaitSelectTime()); // 적 이동 선택을 보여주기 위한 장치
        }
       
        return useMoveCoin;
    }
    IEnumerator WaitSelectTime()
    {
        
        yield return new WaitForSeconds(2f);
        //Debug.Log("적 선택중!");
        useMoveCoin = Random.Range(1, 4); // 1~3 개중 랜덤하게 선택
        isSelect = false;

    }
    
    public override int SelectAction()
    {

        int enemyAction = -1;
        if (enemyAction == -1)// 1회만
            enemyAction = Random.Range(0, 101);// 0 ~49는 안전 ,50~ 100 은 투자

        if (enemyAction < 35) // 적 수치 테스트
            return 1;
        else
            return 2;

    }
    public override int SelectInvestMode()
    {
        int enemyAction = -1;
        if (enemyAction == -1)// 1회만
            enemyAction = Random.Range(0, 101);

        if (enemyAction <30) // 4 : 6 으로 2, 3 투자 선택
        {
            return 1;// 2칸 점령
        }
        else 
        {
            return 2;
        }
    }
}

