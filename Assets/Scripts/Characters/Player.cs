using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override int GetSelectedCoinCount() //���� �ϴ� 3������ 
    {
        //Debug.Log("������ �����ϼ���");
        useMoveCoin = 0;
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    useMoveCoin = 1;
        //    //Debug.Log("1�� ����");
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    useMoveCoin = 2;
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    useMoveCoin = 3;
        //}
        useMoveCoin = uiManager.GetSelectedCoinCount(); // ��ư �Է�
        return useMoveCoin;
    }
    public override int SelectAction()
    {
        int action = 0;
        action = uiManager.GetActionSelectedCoinCount();
        return action;
        
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
