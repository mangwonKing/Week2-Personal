using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public override int GetSelectedCoinCount() //���� �ϴ� 3������ 
    {
        Debug.Log("�� �̵� ����!");
        useMoveCoin = Random.Range(1, 4); // 1~3 ���� �����ϰ� ����
        return useMoveCoin;
    }
    public override int SelectAction()
    {

        int enemyAction = -1;
        if (enemyAction == -1)// 1ȸ��
            enemyAction = Random.Range(0, 101);// 0 ~49�� ���� ,50~ 100 �� ����

        if (enemyAction < 50)
            return 1;
        else
            return 2;

    }
}

