using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public override int GetSelectedCoinCount() //동전 일단 3개까지 
    {
        //Debug.Log("동전을 선택하세요");
        useMoveCoin = 0;
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    useMoveCoin = 1;
        //    //Debug.Log("1번 눌림");
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    useMoveCoin = 2;
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    useMoveCoin = 3;
        //}
        useMoveCoin = uiManager.GetSelectedCoinCount(); // 버튼 입력
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
            return 1;// 2칸 점령
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            return 2;
        }
        else return 0;
    }

}
