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

    //이동 주사위 선택
    public Canvas selectMoveCoinCanvas;
    public Canvas enemySelectMoveCanvas;
    //이동 주사위 결과
    public TextMeshProUGUI selectMoveResultCanvas;

    //땅정보
    public List<TextMeshProUGUI> groundInfo;
    //행동 선택
    public Canvas selectActionButtonCanvas;

    public Canvas investSelectButtonCanvas;

    //움직임 동전
    public Button selectOnebutton;
    public Button selectTwobutton2;
    public Button selectThreebutton3;

    //행동 선택 버튼
    public Button safeButton; // 안전
    public Button investButton; // 투자 

    //투자 선택 버튼 
    public Button investTwoButton;// 2개
    public Button investThreeButton;// 3개 


    // 일시정지 버튼
    public Button resumeButton;
    public Button restartButton;
    public Button exitButton;

    //투자 결과
    public List<TextMeshProUGUI> investResultText;


    //턴 종료 버튼
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

        safeButton.onClick.AddListener(() => OnActionButtonClick(1)); //1 은 안전
        investButton.onClick.AddListener(() => OnActionButtonClick(2)); //2 는 투자

        investTwoButton.onClick.AddListener(() => OnInvestButtonClick(1)); //1 은 2
        investThreeButton.onClick.AddListener(() => OnInvestButtonClick(2)); //2 는 3개 투자

        button8.onClick.AddListener(() => OnTurnOverButtonClick(1)); // 턴 넘기기

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
        SceneManager.LoadScene(1); // 재시작
        Time.timeScale = 1f;
    }
    public void OnExitButtonClick()
    {
        Debug.Log("게임종료");
        Application.Quit(); // 게임 종료
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
        resultText[result].gameObject.SetActive(true); // 0 은 승리 . 1 은 무승부 .2 는 패배
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

    //이동 ui 켜고 끄기
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
        //Debug.Log(coinCount + " 버튼 눌림!");
    }
    public void OnActionButtonClick(int coinCount)
    {
        selectActionCoin = coinCount;
        //Debug.Log(coinCount + " 버튼 눌림!");
    }
    public void OnInvestButtonClick(int investType)
    {
        selectInvestType = investType;
        Debug.Log("투자타입 버튼 눌림");
    }
    public void InvestSelectButtonOn()
    {
        investSelectButtonCanvas.gameObject.SetActive(true);
    }
    public void InvestSelectButtonOff()
    {
        selectInvestType = 0; //초기화 
        investSelectButtonCanvas.gameObject.SetActive(false);
    }
    public void ActionSelectButtonOn()
    {
        //Debug.Log("행동버튼켜짐");
        selectActionButtonCanvas.gameObject.SetActive(true);
    }
    public void ActionSelectButtonOff()
    {
        selectActionCoin = 0;
        selectActionButtonCanvas.gameObject.SetActive(false);
        //Debug.Log("행동버튼꺼짐");
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
    public void ShowMoveCoinTossUI(List<bool> results) // 무브 코인 결과
    {
        moveCoinTossUI.ShowCoinResult(results);
        MoveResultOn(results); // 결과 텍스트로 알려주기
    }
    void MoveResultOn(List<bool> results)
    {
        selectMoveResultCanvas.gameObject.SetActive(true);
        string res = "";
        foreach (bool result in results)
        {
            if (result)
                res += "성공 ";
            else
                res += "실패 ";
        }

        selectMoveResultCanvas.text = "이동 동전던지기 결과 : " + results.Count + "개 투자, 결과는 " + res + "입니다.";
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
