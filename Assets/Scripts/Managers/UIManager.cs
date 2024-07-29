using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public Canvas pauseMenu;

    public CoinTossUI moveCoinTossUI;
    public CoinTossUI actionCoinTossUI;
    public CoinCountUI playerCoinCountUI;
    public CoinCountUI EnemyCoinCountUI;
    public GroundUI playerGroundUI;
    public GroundUI enemyGroundUI;

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
    public Button selectOnebutton;
    public Button selectTwobutton2;
    public Button selectThreebutton3;

    //�ൿ ���� ��ư
    public Button safeButton; // ����
    public Button investButton; // ���� 

    //���� ���� ��ư 
    public Button investTwoButton;// 2��
    public Button investThreeButton;// 3�� 


    // �Ͻ����� ��ư
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;

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

    public TextMeshProUGUI enoughCoin;
    // Start is called before the first frame update
    void Start()
    {
        selectOnebutton.onClick.AddListener(() => OnButtonClick(1));
        selectTwobutton2.onClick.AddListener(() => OnButtonClick(2));
        selectThreebutton3.onClick.AddListener(() => OnButtonClick(3));

        safeButton.onClick.AddListener(() => OnActionButtonClick(1)); //1 �� ����
        investButton.onClick.AddListener(() => OnActionButtonClick(2)); //2 �� ����

        investTwoButton.onClick.AddListener(() => OnInvestButtonClick(1)); //1 �� 2
        investThreeButton.onClick.AddListener(() => OnInvestButtonClick(2)); //2 �� 3�� ����

        button8.onClick.AddListener(() => OnTurnOverButtonClick(1)); // �� �ѱ��

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }
    public void OnResumeButtonClick()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(1); // �����
        Time.timeScale = 1f;
    }
    public void OnExitButtonClick()
    {
        Debug.Log("��������");
        Application.Quit(); // ���� ����
    }
    public void ShowNotEnoughCoin()
    {
        enoughCoin.gameObject.SetActive(true);
        StartCoroutine(ShowNotEnoughCoinCor());
    }
    IEnumerator ShowNotEnoughCoinCor()
    {
        yield return new WaitForSeconds(1);
        enoughCoin.gameObject.SetActive(false);
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
    public void ShowGroundCount(int ground, int idx)
    {
        if (idx == 0)
        {
            playerGroundUI.ShowGroundCount(ground);
        }
        else
        {
            enemyGroundUI.ShowGroundCount(ground);
        }
    }
}
