using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public CoinTossUI moveCoinTossUI;
    public CoinTossUI actionCoinTossUI;
    public CoinCountUI playerCoinCountUI;
    public CoinCountUI EnemyCoinCountUI;

    //�̵� �ֻ��� ����
    public Canvas selectMoveCoinCanvas;
    public Canvas enemySelectMoveCanvas;
    //�̵� �ֻ��� ���
    public TextMeshProUGUI selectMoveResultCanvas;

    //������
    public List<TextMeshProUGUI> groundInfo;
    //�ൿ ����
    public Canvas selectActionButtonCanvas;

    public Canvas investSelectButtonCanvas;

    //������ ����
    public Button button1;
    public Button button2;
    public Button button3;

    //�ൿ ���� ��ư
    public Button button4; // ����
    public Button button5; // ���� 

    //���� ���� ��ư 
    public Button button6;// 2��
    public Button button7;// 3�� 

    //���� ���
    public List<TextMeshProUGUI> investResultText;


    //�� ���� ��ư
    public Button button8;

    public Canvas turnOverCanavas;
    public Canvas enemyTurnOverCanvas;

    public List<TextMeshProUGUI> resultText;

    int selectMoveCoin = 0;
    int selectActionCoin = 0;
    int selectInvestType = 0;
    int selectTurnOver = 0;

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(() => OnButtonClick(1));
        button2.onClick.AddListener(() => OnButtonClick(2));
        button3.onClick.AddListener(() => OnButtonClick(3));

        button4.onClick.AddListener(() => OnActionButtonClick(1)); //1 �� ����
        button5.onClick.AddListener(() => OnActionButtonClick(2)); //2 �� ����

        button6.onClick.AddListener(() => OnInvestButtonClick(1)); //1 �� 2
        button7.onClick.AddListener(() => OnInvestButtonClick(2)); //2 �� 3�� ����

        button8.onClick.AddListener(() => OnTurnOverButtonClick(1)); // �� �ѱ��
    }
    public void ShowInvestResult(bool result)
    {
        if(result)
            investResultText[0].gameObject.SetActive(true);
        else
            investResultText[1].gameObject.SetActive(true);
    }
    public void OffInvestResult(bool result)
    {
        if (result)
            investResultText[0].gameObject.SetActive(false);
        else
            investResultText[1].gameObject.SetActive(false);
    }
    public void ShowGameResult(int result)
    {
        resultText[result].gameObject.SetActive(true); // 0 �� �¸� . 1 �� ���º� .2 �� �й�
    }
    public void OnTurnOverButtonClick(int select)
    {
        selectTurnOver = 1;
    }
    public int GetTurnOver()
    {
        return selectTurnOver;
    }
    public void EnemyTurnOverOn()
    {
        enemyTurnOverCanvas.gameObject.SetActive(true);
    }
    public void EnemyTurnOverOff()
    {
        enemyTurnOverCanvas.gameObject.SetActive(false);
    }
    public void TurnOverOn()
    {
       
        turnOverCanavas.gameObject.SetActive(true);
    }
    public void TurnOverOff()
    {
        selectTurnOver = 0;
        turnOverCanavas.gameObject.SetActive(false);
    }

    //�̵� ui �Ѱ� ����
    public void SelectMoveCoinViewOn(int idx)
    {
        if(idx == 0)
        {
            selectMoveCoinCanvas.gameObject.SetActive(true);
        }
        else
        {
            enemySelectMoveCanvas.gameObject.SetActive(true);
        }
    }
    public void SelectMoveCoinViewOff(int idx)
    {
        if (idx == 0)
        {
            selectMoveCoinCanvas.gameObject.SetActive(false);
        }
        else
        {
            enemySelectMoveCanvas.gameObject.SetActive(false);
        }
    }
    public void OnButtonClick(int coinCount)
    {
        selectMoveCoin = coinCount;
        //Debug.Log(coinCount + " ��ư ����!");
    }
    public void OnActionButtonClick(int coinCount)
    {
        selectActionCoin = coinCount;
        //Debug.Log(coinCount + " ��ư ����!");
    }
    public void OnInvestButtonClick(int investType)
    {
        selectInvestType = investType;
        Debug.Log("����Ÿ�� ��ư ����");
    }
    public void InvestSelectButtonOn()
    {
        investSelectButtonCanvas.gameObject.SetActive(true);
    }
    public void InvestSelectButtonOff()
    {
        selectInvestType = 0; //�ʱ�ȭ 
        investSelectButtonCanvas.gameObject.SetActive(false);
    }
    public void ActionSelectButtonOn()
    {
        //Debug.Log("�ൿ��ư����");
        selectActionButtonCanvas.gameObject.SetActive(true);
    }
    public void ActionSelectButtonOff()
    {
        selectActionCoin = 0;
        selectActionButtonCanvas.gameObject.SetActive(false);
        //Debug.Log("�ൿ��ư����");
    }
    public int GetInvestType()
    {
        return selectInvestType;
    }
    public int GetSelectedCoinCount()
    {
        return selectMoveCoin;
    }
    public int GetActionSelectedCoinCount()
    {
        return selectActionCoin;
    }
    public void SetSelectedCoinCount()
    {
        selectMoveCoin = 0;
    }
    public void InitCoinView()
    {
        moveCoinTossUI.InitCoinView();
        actionCoinTossUI.InitCoinView();
    }
    public void ShowMoveCoinTossUI(List<bool> results) // ���� ���� ���
    {
        moveCoinTossUI.ShowCoinResult(results);
        MoveResultOn(results); // ��� �ؽ�Ʈ�� �˷��ֱ�
    }
    void MoveResultOn(List<bool> results)
    {
        selectMoveResultCanvas.gameObject.SetActive(true);
        string res = "";
        foreach (bool result in results)
        {
            if (result)
                res += "���� ";
            else
                res += "���� ";
        }

        selectMoveResultCanvas.text = "�̵� ���������� ��� : " + results.Count + "�� ����, ����� " + res + "�Դϴ�.";
    }
    public void MoveResultOff()
    {
        selectMoveResultCanvas.gameObject.SetActive(false);

    }

    public void GroundInfoOn(int groundType)
    {
        groundInfo[groundType].gameObject.SetActive(true);
    }
    public void GroundInfoOff(int groundType)
    {
        groundInfo[groundType].gameObject.SetActive(false);
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
