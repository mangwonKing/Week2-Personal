using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{ 
    public override int GetSelectedCoinCount() 
    {
        useMoveCoin = 0;
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
        int invest = 0;
        invest = uiManager.GetInvestType();
        Debug.Log("���ڼ���" + invest);
        return invest;
    }

}
