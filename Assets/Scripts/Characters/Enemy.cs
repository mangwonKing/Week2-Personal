using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public override int GetSelectedCoinCount() //동전 일단 3개까지 
    {
        Debug.Log("적 이동 선택!");
        useMoveCoin = Random.Range(1, 4); // 1~3 개중 랜덤하게 선택
        return useMoveCoin;
    }
    public override int SelectAction()
    {

        int enemyAction = -1;
        if (enemyAction == -1)// 1회만
            enemyAction = Random.Range(0, 101);// 0 ~49는 안전 ,50~ 100 은 투자

        if (enemyAction < 50)
            return 1;
        else
            return 2;

    }
}

