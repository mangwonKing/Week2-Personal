using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    bool isSelect = false;
    public override int GetSelectedCoinCount() //���� �ϴ� 3������ 
    {
        
        if(!isSelect)
        {
            isSelect = true;
            StartCoroutine(WaitSelectTime()); // �� �̵� ������ �����ֱ� ���� ��ġ
        }
       
        return useMoveCoin;
    }
    IEnumerator WaitSelectTime()
    {
        
        yield return new WaitForSeconds(2f);
        //Debug.Log("�� ������!");
        useMoveCoin = Random.Range(1, 4); // 1~3 ���� �����ϰ� ����
        isSelect = false;

    }
    
    public override int SelectAction()
    {

        int enemyAction = -1;
        if (enemyAction == -1)// 1ȸ��
            enemyAction = Random.Range(0, 101);// 0 ~49�� ���� ,50~ 100 �� ����

        if (enemyAction < 35) // �� ��ġ �׽�Ʈ
            return 1;
        else
            return 2;

    }
    public override int SelectInvestMode()
    {
        int enemyAction = -1;
        if (enemyAction == -1)// 1ȸ��
            enemyAction = Random.Range(0, 101);

        if (enemyAction <30) // 4 : 6 ���� 2, 3 ���� ����
        {
            return 1;// 2ĭ ����
        }
        else 
        {
            return 2;
        }
    }
}

