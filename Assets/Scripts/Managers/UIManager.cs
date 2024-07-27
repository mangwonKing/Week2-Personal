using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public CoinTossUI moveCoinTossUI;
    public CoinTossUI actionCoinTossUI;
    public CoinCountUI playerCoinCountUI;
    public CoinCountUI EnemyCoinCountUI;

    public Canvas canvas;
    //������ ����
    public Button button1;
    public Button button2;
    public Button button3;

    int selectCoin = 0;

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(() => OnButtonClick(1));
        button2.onClick.AddListener(() => OnButtonClick(2));
        button3.onClick.AddListener(() => OnButtonClick(3));
    }
    //public void MoveCanvasActive()
    //{
    //    canvas.gameObject.SetActive(true);
    //}
    //public void MoveCanvasActiveF()
    //{
    //    canvas.gameObject.SetActive(false);
    //}
    public void OnButtonClick(int coinCount)
    {
        selectCoin = coinCount;
        Debug.Log(coinCount + " ��ư ����!");
    }

    public int GetSelectedCoinCount()
    {
        
        return selectCoin;
    }
    public void SetSelectedCoinCount()
    {
        selectCoin = 0;
    }
    public void InitCoinView()
    {
        moveCoinTossUI.InitCoinView();
        actionCoinTossUI.InitCoinView();
    }
    public void ShowMoveCoinTossUI(List<bool> results) // ���� ���� ���
    {
        moveCoinTossUI.ShowCoinResult(results);
    }
    public void ShowActionCoinTossUI(List<bool> results) // �׼� ���� ���
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
