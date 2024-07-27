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
        WaitMove,
        CheckOwner,
        SelectAction,
        SafeAction,
        InvestAction,
        ConfirmEndTurn,
        EndTurn,
        EndGame
    }
    
    public Character[] characters;
    public GroundManager groundManager;
    public UIManager uiManager;

    private int characterIdx = 0;
    private int coinCount = 0;
    private int moveCount = 0;
    private int ownerGround = 0; // 0 은 빈땅 , 1은 내땅 2는 상대
    int playerAction = 0; //1 이 안전 2 가 투자
    
    //게임 상태관리
    private GameState state;

    void Start()
    {
        characterIdx = 0;
        state = GameState.StartTurn;
    }

    string ownerName = null;
    public IEnumerator GameLoop()
    {
        //시작시 코인 알려주기
        uiManager.ShowCoinCount(characters[0].coinCounts, 0);
        uiManager.ShowCoinCount(characters[1].coinCounts, 1);
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
                case GameState.WaitMove:
                    WaitMove();
                    break;
                case GameState.CheckOwner:
                    CheckOwner();
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
                case GameState.EndGame:
                    EndGame();
                    break;
            }
            yield return null;
        }
    }

    void StartTurn()
    {
        if (characters[0].coinCounts <= 0 && characters[1].coinCounts <= 0)
        {
            Debug.Log("게임종료");
            state = GameState.EndGame;
        }
        else
        {
            uiManager.InitCoinView();
            // 턴 시작 로직
            //if (!isSelectMoveCoin)
            //{
            //    //Debug.Log("턴시작!");
            //    //state = GameState.SelectMoveCoin;
            //}
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
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts,characterIdx); // idx 로 캐릭터 구분
            state = GameState.MoveCoinFlip;
        }
    }

    void MoveCoinFlip()
    {
        moveCount = characters[characterIdx].MoveCoinFlip(coinCount);
        if (moveCount > 0)
        {
            Debug.Log("이동 동전 굴림!");
            uiManager.ShowMoveCoinTossUI(characters[characterIdx].coinFlipResult);
            //moveCoinTossUI.ShowCoinResult(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();// 다음을 위해 비우기
            state = GameState.Move;
        }
    }

    void Move()
    {
        characters[characterIdx].Move(moveCount);
        Debug.Log("이동!");
        //이동 대기 상태 만들것
        state = GameState.WaitMove;
    }
    void WaitMove()
    {
        if (characters[characterIdx].isMoveEnd) //이동을 기다리도록 
        {
            state = GameState.CheckOwner;
        }
    }
    void CheckOwner()
    {
        ownerName = groundManager.OwnerName(characters[characterIdx].GetPosx(), characters[characterIdx].GetPosy()); //땅 존재 받고
        if (ownerName == "None") ownerGround = 0;
        else if (ownerName == characters[characterIdx].GetCharacterType())
        {
            Debug.Log("내땅을 밟았네요 코인 보너스!");
            characters[characterIdx].coinCounts += 2; // 내땅은 바로 코인 주고 끝내기
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
            state = GameState.ConfirmEndTurn;
        }
        else ownerGround = 2;
        Debug.Log(ownerName + "땅 입니다.");
        state = GameState.SelectAction;
    }
    //지금 액션 선택이랑 실행이 같이 있음 -> 액션을 선택하면 실행쪽 턴으로 넘기게 해보자.
    void SelectAction()
    {
        playerAction = characters[characterIdx].SelectAction(); // 플레이어가 투자를 할지 안전을 할지 선택
        if(playerAction > 0)
        {
            Debug.Log("행동 : " + playerAction); 
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
    //구조 개선: 샐랙트 액션에서 땅 존재를 파악하고 변수로 갖고 있고 액션을 선택하는게 올바른 방향 
    void SafeAction() // 안전 액션 
    {
        Debug.Log("안전 액션!");

        if (ownerGround == 0) //빈땅
        {
            characters[characterIdx].coinCounts--; //코인 1개 소모

            groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // 캐릭터의 이름을 넘겨주고 점령
            state = GameState.ConfirmEndTurn;
        }
        else if (ownerGround == 2) // 적 땅
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
        uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
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
            }// 일단 각 50퍼 
            
        }
        uiManager.ShowActionCoinTossUI(characters[characterIdx].coinFlipResult);
        //actionCoinTossUI.ShowCoinResult(characters[characterIdx].coinFlipResult);
        characters[characterIdx].coinFlipResult.Clear();
        if (isSuccess) // 성공
        {
            groundManager.OccupyThreeGround(characters[characterIdx].GetCharacterType());
        }
        state = GameState.ConfirmEndTurn;
        uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
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
        
        characterIdx = (characterIdx + 1) % characters.Length; // 플레이어 교체
        if (characters[characterIdx].coinCounts <= 0)
        {
            //다음 캐릭터가 코인이 없으니 턴은 오지 않음
            characterIdx = (characterIdx + 1) % characters.Length;
        }
         //다음 플레이어를 위해 턴에서 사용했던것들 초기화
        playerAction = 0;
        coinCount = 0;
        moveCount = 0;
        ownerGround = 0;
        ownerName = null;

        state = GameState.StartTurn;
        
        
    }
    void EndGame()
    {
        Debug.Log("게임종료");
    }
    
}
