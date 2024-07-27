using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum GameState
    {
        StartTurn,
        SelectMoveCoin,
        MoveCoinFlip,
        Move,
        SelectAction,
        SafeAction,
        InvestAction,
        ConfirmEndTurn,
        EndTurn,
    }

    public Character[] characters;
    public CoinTossUI coinTossUI;
    public CoinTossUI actionCoinTossUI;
    public GroundManager groundManager;

    private int characterIdx = 0;
    private int coinCount = 0;
    private int moveCount = 0;
    private bool playerTurn = true;
    int playerAction = 0; //1 이 안전 2 가 투자


    private bool isSelectMoveCoin = false;
    private bool isMoveCoinFlip = false;
    private bool isMove = false;
    private bool isSelectAction = false;
    private bool isEndTurn = false;

    private GameState state;


    int test = 0;
    void Start()
    {
        state = GameState.StartTurn;
    }

    string ownerName = null;
    public IEnumerator GameLoop()
    {
        while (true)
        {
            switch (state)
            {
                case GameState.StartTurn:
                    StartTurn();
                    break;
                case GameState.SelectMoveCoin:
                    SelectMoveCoin();
                    break;
                case GameState.MoveCoinFlip:
                    MoveCoinFlip();
                    break;
                case GameState.Move:
                    Move();
                    break;
                case GameState.SelectAction:
                    SelectAction();
                    break;
                case GameState.SafeAction:
                    SafeAction();
                    break;
                case GameState.InvestAction:
                    InvestAction();
                    break;
                case GameState.ConfirmEndTurn:
                    yield return StartCoroutine(ConfirmEndTurn());
                    break;
                case GameState.EndTurn:
                    EndTurn();
                    break;
            }
            yield return null;
        }
    }

    void StartTurn()
    {
        coinTossUI.InitCoinView();
        actionCoinTossUI.InitCoinView();
        // 턴 시작 로직
        if (!isSelectMoveCoin)
        {
            Debug.Log("턴시작!");
            state = GameState.SelectMoveCoin;
        }
    }

    void SelectMoveCoin()
    {
        coinCount = characters[characterIdx].GetSelectedCoinCount();
        
        if (coinCount > 0)
        {
            Debug.Log("이동 동전 선택!");

            characters[characterIdx].coinCounts -= coinCount; // 사용 코인만큼 차감 , 1회만
            isSelectMoveCoin = true;
            state = GameState.MoveCoinFlip;
        }
    }

    void MoveCoinFlip()
    {
        moveCount = characters[characterIdx].MoveCoinFlip(coinCount);
        if (moveCount > 0)
        {
            Debug.Log("이동 동전 굴림!");

            isMoveCoinFlip = true;
            coinTossUI.ShowCoinResult(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();// 다음을 위해 비우기
            state = GameState.Move;
        }
    }

    void Move()
    {
        characters[characterIdx].Move(moveCount);
        isMove = true;
        Debug.Log("이동!");

        state = GameState.SelectAction;
    }
    //지금 액션 선택이랑 실행이 같이 있음 -> 액션을 선택하면 실행쪽 턴으로 넘기게 해보자.
    void SelectAction()
    {
        ownerName = groundManager.OwnerName(characters[characterIdx].GetPosx(), characters[characterIdx].GetPosy()); //땅 존재 받고
        
        playerAction = characters[characterIdx].SelectAction(); // 플레이어가 투자를 할지 안전을 할지 선택
        if(playerAction > 0)
        {
            Debug.Log("행동 : " + playerAction); 
            isSelectAction = true;
            if (playerAction == 1) // 안전
            {
                Debug.Log(" 안전 상태를 선택했습니다.");
                state = GameState.SafeAction;
            }
            else if (playerAction == 2) //투자
            {
                Debug.Log(" 투자 상태를 선택했습니다.");

                state = GameState.InvestAction;
            }
        }
    }

    void SafeAction() // 안전 액션 
    {
        Debug.Log("안전 액션!");

        if (ownerName == "None") //빈땅
        {
            characters[characterIdx].coinCounts--; //코인 1개 소모
            groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // 캐릭터의 이름을 넘겨주고 점령
            state = GameState.ConfirmEndTurn;
        }
        else if (ownerName != characters[characterIdx].GetCharacterType()) // 적 땅
        {
            characters[characterIdx].coinCounts--;
            state = GameState.ConfirmEndTurn;
        }
        else // 코인을 더 주던가 턴을 더주던가 선택
        {
            state = GameState.ConfirmEndTurn;
            Debug.Log(" 내땅 밟음");
            return;
        }
    }
    void InvestAction() // 투자 액션 
    {
        Debug.Log("투자액션!");
        bool isSuccess = true;
        // 땅 주인에 따라 다르게 하기
        //일단 2개 고정 투자해서 돌리는걸로
        characters[characterIdx].coinCounts -= 2; // 2개 소모해서 투자
        //동전 플립
        for(int i = 0; i<  2; i++)
        {
            if (!characters[characterIdx].CoinFlip(50))
            {
                isSuccess = false;
               //실패!
            }// 일단 50퍼 
            
        }
        actionCoinTossUI.ShowCoinResult(characters[characterIdx].coinFlipResult);
        characters[characterIdx].coinFlipResult.Clear();
        if (isSuccess) // 성공
        {
            groundManager.OccupyThreeGround(characters[characterIdx].GetCharacterType());
        }
        state = GameState.ConfirmEndTurn;
    }
    IEnumerator ConfirmEndTurn()
    {
        if (characterIdx == 0)
        {
            Debug.Log("턴을 넘기겠습니까? (F 키를 눌러 확인)");

            while (!Input.GetKeyDown(KeyCode.F))
            {
                yield return null; // F 키 입력을 기다림
            }
        }
        else
        {
            Debug.Log("컴퓨터의 턴이 끝났습니다.");
            yield return new WaitForSeconds(3f); // 약간의 딜레이 추가 (선택 사항)
        }

        state = GameState.EndTurn;
    }
    void EndTurn()
    {
        Debug.Log("턴 종료 남은코인 : " + characters[characterIdx].coinCounts);
        // 턴 종료 로직
        playerTurn = !playerTurn; // 턴의 주인 교체
        characterIdx = (characterIdx + 1) % characters.Length; // 플레이어 교체

        // 상태 초기화
        isSelectMoveCoin = false;
        isMoveCoinFlip = false;
        isMove = false;
        isSelectAction = false;
        isEndTurn = false;

        playerAction = 0;
        coinCount = 0;
        moveCount = 0;
        ownerName = null;
        state = GameState.StartTurn;
        
    }
    
}
