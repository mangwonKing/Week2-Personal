using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override int GetSelectedCoinCount() //���� �ϴ� 3������ 
    {
        //Debug.Log("������ �����ϼ���");
        useMoveCoin = 0;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useMoveCoin = 1;
            //Debug.Log("1�� ����");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            useMoveCoin = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            useMoveCoin = 3;
        }

        return useMoveCoin;
    }
    public override int SelectAction()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            return 1;
            //SafePass(isEmpty);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            //InvestPass(isEmpty);
            return 2;
        }
        return 0; //�Է� ����
        
    }
    public override int SelectInvestMode()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            return 1;// 2ĭ ����
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            return 2;
        }
        else return 0;
    }

}
