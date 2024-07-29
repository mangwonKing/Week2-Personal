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

    //�ൿ ��� 

    //�� �ѱ��

    //������ ����
    public Button button1;
    public Button button2;
    public Button button3;

    //�ൿ ���� ��ư
    public Button button4; // ����
    public Button button5; // ���� 

    int selectCoin = 0;
    int actionSelectCoin = 0;

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(() => OnButtonClick(1));
        button2.onClick.AddListener(() => OnButtonClick(2));
        button3.onClick.AddListener(() => OnButtonClick(3));

        button4.onClick.AddListener(() => OnActionButtonClick(1)); //1 �� ����
        button5.onClick.AddListener(() => OnActionButtonClick(2)); //2 �� ����
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
        selectCoin = coinCount;
        Debug.Log(coinCount + " ��ư ����!");
    }
    public void OnActionButtonClick(int coinCount)
    {
        actionSelectCoin = coinCount;
        Debug.Log(coinCount + " ��ư ����!");
    }

    public void ActionSelectButtonOn()
    {
        Debug.Log("�ൿ��ư����");
        selectActionButtonCanvas.gameObject.SetActive(true);
    }
    public void ActionSelectButtonOff()
    {
        actionSelectCoin = 0;
        selectActionButtonCanvas.gameObject.SetActive(false);
        Debug.Log("�ൿ��ư����");
    }
   
    public int GetSelectedCoinCount()
    {
        
        return selectCoin;
    }
    public int GetActionSelectedCoinCount()
    {
        return actionSelectCoin;
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
