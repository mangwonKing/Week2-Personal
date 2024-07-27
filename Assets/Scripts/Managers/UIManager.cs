using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public CoinTossUI moveCoinTossUI;
    public CoinTossUI actionCoinTossUI;
    public CoinCountUI playerCoinCountUI;
    public CoinCountUI EnemyCoinCountUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitCoinView()
    {
        moveCoinTossUI.InitCoinView();
        actionCoinTossUI.InitCoinView();
    }
    public void ShowMoveCoinTossUI(List<bool> results) // 무브 코인 결과
    {
        moveCoinTossUI.ShowCoinResult(results);
    }
    public void ShowActionCoinTossUI(List<bool> results) // 액션 코인 결과
    {
        actionCoinTossUI.ShowCoinResult(results);
    }
    public void ShowCoinCount(int coin , int idx)
    {
        if(idx == 0)
        {
            playerCoinCountUI.ShowCoinCount(coin);
        }
        else
        {
            EnemyCoinCountUI.ShowCoinCount(coin);
        }
        
    }
}
