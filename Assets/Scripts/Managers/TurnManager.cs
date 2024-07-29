using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.TextCore.Text;

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
        FocusSelect,
        InvestCustom,
        InvestCheck,
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


    int investPersent = 70; // 기본 70 
    int selectMode = 0;

    int otherIdx;
    //게임 상태관리
    private GameState state;
    void Start()
    {
        characterIdx = 0;
        state = GameState.StartTurn;
    }
    bool isSuccess = false;
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
                case GameState.InvestCustom:
                    InvestCustom();
                    break;
                case GameState.InvestCheck:
                    yield return StartCoroutine(InvestCheck());// 결과 확인
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
            otherIdx = characterIdx + 1 == 2 ? 0 : 1; // 상대방 인덱스 저장
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
        uiManager.SelectMoveCoinViewOn(characterIdx); // 적과 나 중 적절한 ui 출력

        coinCount = characters[characterIdx].GetSelectedCoinCount();
        //여기서 코인 개수가 부족하다면 선택을 못하게 해야할듯?
        if (characters[characterIdx].coinCounts < coinCount && coinCount != 0)
        {
            // 코인 
            if(characterIdx ==0)
            {
                uiManager.ShowNotEnoughCoin();
                Debug.Log("코인이 부족합니다.");
            }
            coinCount = 0;
        }
        if (coinCount > 0)
        {
            Debug.Log("이동 동전 선택!");
            characters[characterIdx].coinCounts -= coinCount; // 사용 코인만큼 차감 , 1회만
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts,characterIdx); // idx 로 캐릭터 구분
            uiManager.SelectMoveCoinViewOff(characterIdx); // 선택 버튼 끄기

            state = GameState.MoveCoinFlip;
        }
    }

    void MoveCoinFlip()
    {
        moveCount = characters[characterIdx].MoveCoinFlip(coinCount);
        if (moveCount > 0)
        {
            Debug.Log("이동 동전 굴림!");
            uiManager.ShowMoveCoinTossUI(characters[characterIdx].coinFlipResult);// ui 매니저가 결과를 받네 그럼 이때 받은걸 바탕으로 출력하자.
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
            uiManager.MoveResultOff();// 이동 완료했으니 끄기
            
        }
    }
    void CheckOwner()
    {
        ownerName = groundManager.OwnerName(characters[characterIdx].GetPosx(), characters[characterIdx].GetPosy()); //땅 존재 받고
        List<string> list = groundManager.GetNearOwner(); // 인접 땅의 오너
        if (ownerName == "None")
        {
            ownerGround = 0;
            //빈땅에서 투자할 경우의 확률 고지 
            CalculatePersent(list);
        }
        else if (ownerName == characters[characterIdx].GetCharacterType())
        {
            //Debug.Log("내땅을 밟았네요 코인 보너스!");
            characters[characterIdx].coinCounts += 2; // 내땅은 코인 주고 행동 선택
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
            ownerGround = 1;
            CalculatePersent(list);
        }
        else
        {
            ownerGround = 2;
        }
        //Debug.Log(ownerName + "땅 입니다.");
        
        if (characters[characterIdx].coinCounts == 0) // 코인이 0개면 턴 종료시키기
        {
            //uiManager.ShowNotEnoughCoin();
            Debug.Log("코인을 다 썼습니다.");
            state = GameState.ConfirmEndTurn;
        }
        else
        {
            if (characterIdx == 0)
                uiManager.GroundInfoOn(ownerGround); // 플레이어는 ui 키기
            state = GameState.SelectAction;
        }
    }
    void CalculatePersent(List<string> list)
    {
        
        foreach (var item in list)
        {
            Debug.Log("인접한 땅의 주인 : " + item);
            if (item != characters[characterIdx].GetCharacterType() && item != "None") // 빈땅도 아니고 이름이 다르다면 적으로 간주 
            {
                Debug.Log("적은 확률은 감소시킵니다.");
                investPersent -= 10;
            }
        }
        //Debug.Log("이번 땅에서 투자할 경우 확률은 " + investPersent + "%입니다.");
    }
    void SelectAction()
    {
        if (characterIdx == 0)
            uiManager.ActionSelectButtonOn();
        playerAction = characters[characterIdx].SelectAction(); // 플레이어가 투자를 할지 안전을 할지 선택

        if (characters[characterIdx].coinCounts < playerAction &&  playerAction != 0)
        {
            if (characterIdx == 0)
            {
                uiManager.ShowNotEnoughCoin();
                Debug.Log("코인이 부족합니다.");
            }
            playerAction = 0;
        }

        if (playerAction > 0)
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


            if (characterIdx == 0)
            {
                uiManager.GroundInfoOff(ownerGround); // 액션 선택했으니 빠지기
                uiManager.ActionSelectButtonOff();
            }

        }
    }


    void SafeAction() // 안전 액션 
    {
        Debug.Log("안전 액션!");

        if (ownerGround == 0) //빈땅
        {
            characters[characterIdx].coinCounts--; //코인 1개 소모

            groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // 캐릭터의 이름을 넘겨주고 점령
            characters[characterIdx].ownedTiles.Add(groundManager.curGround); // 캐릭터의 소유 리스트로 관리
            uiManager.ShowGroundCount(characters[characterIdx].ownedTiles.Count, characterIdx); // 땅 얻음 최신화

            state = GameState.ConfirmEndTurn;
        }
        else if (ownerGround == 2) // 적 땅
        {
            characters[characterIdx].coinCounts--;
            state = GameState.ConfirmEndTurn;
        }
        else // 내땅 그냥 넘어가기
        {
            state = GameState.ConfirmEndTurn;
        }
        uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
    }
    void InvestAction() // 투자 액션 
    {
        Debug.Log("투자액션!");
        isSuccess = true;
        // 땅 주인에 따라 다르게 하기
        //investPersent = 100;// 테스트용 꼭 수정할것 !!
        //빈땅
        if (ownerGround == 0 || ownerGround == 1) //빈땅 혹은 내땅
        {
            characters[characterIdx].coinCounts -= 2; // 2개 소모해서 투자

                                                      //동전 플립
            for (int i = 0; i < 2; i++)
            {
                if (!characters[characterIdx].CoinFlip(investPersent)) // 확률 적용
                {
                    isSuccess = false;
                    //실패!
                }

            }
            uiManager.ShowActionCoinTossUI(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();
            if (isSuccess) // 성공
            {
                groundManager.OccupyThreeGround(characters[characterIdx].GetCharacterType());
                OwnedCheck(groundManager.curGround);
                OwnedCheck(groundManager.near1);
                OwnedCheck(groundManager.near2);
                uiManager.ShowGroundCount(characters[characterIdx].ownedTiles.Count, characterIdx); // 땅 얻음 최신화
            }
            //투자-> 투자체크 상태로 넘기기
            state = GameState.InvestCheck;
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
            
            
        }

        else // 적 땅
        {
            // issuccess 가 false 면 땅 하나 빼앗기게 하기
            // 여기에 이제 2가지 투자에 대한 선택지 ui 를 띄우고 
            uiManager.InvestSelectButtonOn(); // on
            selectMode = characters[characterIdx].SelectInvestMode();
            if(selectMode+1 > characters[characterIdx].coinCounts && selectMode != 0)
            {
                if (characterIdx == 0)
                {
                    uiManager.ShowNotEnoughCoin();
                    Debug.Log("코인이 부족합니다.");
                }
                selectMode = 0;
            }
            if (selectMode > 0)
            {
                state = GameState.InvestCustom;
                uiManager.InvestSelectButtonOff(); // 끄기
            }
        }

    }
    void OwnedCheck(Ground ground)
    {
        bool flag = false;
        bool flag2 = false;
        foreach (Ground ownedGround in characters[characterIdx].ownedTiles) //이미 있는데 또 add 하려고 한다?
        {
            if (ownedGround == ground)
            {//이미 들어있다면
                flag = true;
                Debug.Log("이미 내땅이에요");
            }
            
        }
        foreach(Ground other in characters[otherIdx].ownedTiles)
        {
            if(other == ground) // 만약 적의 땅이라면
            {
                flag2 = true;
            }
        }
        if(!flag)
        {
            characters[characterIdx].ownedTiles.Add(ground); // 안겹치니까 넣기
        }
        if(flag2)
        {
            characters[otherIdx].ownedTiles.Remove(ground); // 적의 소유권 뺐기
            Debug.Log("적의 땅까지 뺐었네요");
        }
    }
    IEnumerator InvestCheck()
    {
        uiManager.ShowInvestResult(isSuccess);
        yield return new WaitForSeconds(2f);
        state = GameState.ConfirmEndTurn; //종료 확인 후 상태 넘기기
        uiManager.OffInvestResult(isSuccess);
    }
    // 2개 투자하는 턴 , 3개 투자하는 턴 구현하기
    void InvestCustom()
    {
        isSuccess = true;
        if (selectMode == 1)
        {
            Debug.Log("2개 투자합니다!");
            // 동전 2개 플립한 후 전부 성공시 땅 빼았기 
            characters[characterIdx].coinCounts -= 2; // 2개 투자
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);

            //70퍼 그대로 갖기 일단은
            //investPersent += 15; 

            for (int i = 0; i < 2; i++)
            {
                if (!characters[characterIdx].CoinFlip(investPersent)) // 확률 적용
                {
                    isSuccess = false;
                    //실패!
                }
            }
            uiManager.ShowActionCoinTossUI(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();
            if(isSuccess)
            {
                groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // 해당 땅 빼았기 성공
                characters[characterIdx].ownedTiles.Add(groundManager.curGround); // 캐릭터의 소유 리스트로 관리
                characters[otherIdx].ownedTiles.Remove(groundManager.curGround);// 상대방 리스트에서 제거
                uiManager.ShowGroundCount(characters[characterIdx].ownedTiles.Count, characterIdx); // 땅 얻음 최신화
                uiManager.ShowGroundCount(characters[otherIdx].ownedTiles.Count, otherIdx); // 땅 잃음 최신화
                Debug.Log("땅 빼았기 성공!");
            }
            else
            {
                // 캐릭터의 소유물 리스트에서 숫자를 하나 랜덤으로 구한다.
                if (characters[characterIdx].ownedTiles.Count == 0)
                {
                    Debug.Log("뺏길 땅도 없네요?");
                }
                else
                {
                    int steelGround = Random.Range(0, characters[characterIdx].ownedTiles.Count); // 2개면 0~1 중에 하나 리턴 
                    string steelName = characterIdx == 0 ? "Enemy" : "Player"; // 뺐기는 거니까 반대로
                    
                    characters[characterIdx].ownedTiles[steelGround].Occupied(steelName);
                    characters[otherIdx].ownedTiles.Add(characters[characterIdx].ownedTiles[steelGround]); // 땅 점령과 동시에 적의 리스트에 추가
                    characters[characterIdx].ownedTiles.RemoveAt(steelGround); // 나한테서 해당 인덱스 그라운드 삭제

                    uiManager.ShowGroundCount(characters[characterIdx].ownedTiles.Count, characterIdx); // 나 땅 잃음 최신화
                    uiManager.ShowGroundCount(characters[otherIdx].ownedTiles.Count, otherIdx); // 상대 땅 얻음 최신화
                    Debug.Log(" 땅을 뺐겼습니다.");
                }
                
            }
            
        }
        else
        {
            //동전 3개 플립 후 전부 성공시 해당 땅 + 적 보유땅 2개 빼았기
            Debug.Log("3개 투자합니다!");
            characters[characterIdx].coinCounts -= 3; // 3개 투자
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);

            //기본 70퍼로 하기
            //investPersent += 20; // 일단 동전당 70퍼센트로 상향 

            for (int i = 0; i < 3; i++)
            {
                if (!characters[characterIdx].CoinFlip(investPersent)) // 확률 적용
                {
                    isSuccess = false;
                    //실패!
                }
            }
            uiManager.ShowActionCoinTossUI(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();
            if(isSuccess) //성공시 땅 빼았기!
            {
                groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // 해당 땅 빼았기 성공
                characters[characterIdx].ownedTiles.Add(groundManager.curGround); // 캐릭터의 소유 리스트로 관리
                characters[otherIdx].ownedTiles.Remove(groundManager.curGround);// 상대방 리스트에서 제거

                Debug.Log("땅 빼았기 성공!");
                // 랜덤땅 2개 빼았기 -> 조건 추가하기 // 적의 땅이 1개면 한번만 , 0개면 넘어가기
                int ground = 2; // 기본 2번 빼았음

                if (characters[otherIdx].ownedTiles.Count == 1)
                {
                    ground = 1;
                    Debug.Log("빼앗을 땅이 하나밖에 없네요... 코인을 한개 돌려드릴게요");
                    characters[characterIdx].coinCounts += 1;
                }
                else if(characters[otherIdx].ownedTiles.Count == 0)
                {
                    ground = 0;
                    Debug.Log("빼앗을 땅이 없네요... 코인을 돌려드릴게요");
                    characters[characterIdx].coinCounts += 3;
                }
                uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);

                for (int i = 0; i < ground; i++)
                {
                    int steelGround = Random.Range(0, characters[otherIdx].ownedTiles.Count); // 적이가진 땅들 중 하나를 가져옴
                    string steelName = characterIdx == 0 ? "Player" : "Enemy"; // 빼았는 거니까 제대로 ;

                    characters[otherIdx].ownedTiles[steelGround].Occupied(steelName); //적의 땅 빼았기
                    characters[characterIdx].ownedTiles.Add(characters[otherIdx].ownedTiles[steelGround]); //빼았아서 내 리스트에 넣기
                    characters[otherIdx].ownedTiles.RemoveAt(steelGround); // 적의 해당 인덱스 그라운드 삭제
                    Debug.Log("땅을 빼았았습니다.");
                }
                uiManager.ShowGroundCount(characters[characterIdx].ownedTiles.Count, characterIdx); // 땅 얻음 최신화
                uiManager.ShowGroundCount(characters[otherIdx].ownedTiles.Count, otherIdx); // 땅 잃음 최신화

            }
            else
            {
                // 캐릭터의 소유물 리스트에서 숫자를 하나 랜덤으로 구한다.
                if (characters[characterIdx].ownedTiles.Count == 0)
                {
                    Debug.Log("뺏길 땅도 없네요?");
                }
                else
                {
                    int steelGround = Random.Range(0, characters[characterIdx].ownedTiles.Count); // 2개면 0~1 중에 하나 리턴 
                    string steelName = characterIdx == 0 ? "Enemy" : "Player"; // 뺐기는 거니까 반대로

                    characters[characterIdx].ownedTiles[steelGround].Occupied(steelName);
                    characters[otherIdx].ownedTiles.Add(characters[characterIdx].ownedTiles[steelGround]); // 땅 점령과 동시에 적의 리스트에 추가
                    characters[characterIdx].ownedTiles.RemoveAt(steelGround); // 나한테서 해당 인덱스 그라운드 삭제

                    uiManager.ShowGroundCount(characters[characterIdx].ownedTiles.Count, characterIdx); // 땅 잃음 최신화
                    uiManager.ShowGroundCount(characters[otherIdx].ownedTiles.Count, otherIdx); // 땅 얻음 최신화
                    Debug.Log(" 땅을 뺐겼습니다.");
                }

            }

        }
        uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
        state = GameState.InvestCheck; //결과확인
        //state = GameState.ConfirmEndTurn; // 턴 넘기기
    }
    // 투자를 선택하면 집중력을 사용하시겠습니까 상태로 이동
    // 일단 한개만 적용되도록 만들기

    IEnumerator ConfirmEndTurn()
    {
        
        if (characterIdx == 0)
        {
            //Debug.Log("턴을 넘기겠습니까? (F 키를 눌러 확인)");
            uiManager.TurnOverOn();
            while (uiManager.GetTurnOver() == 0)
            {
                yield return null; // 버튼 눌리길 기다림
            }
            //Debug.Log("턴 종료!");
            
        }
        else
        {
            uiManager.EnemyTurnOverOn();
            Debug.Log("컴퓨터의 턴이 끝났습니다.");
            yield return new WaitForSeconds(2f); // 약간의 딜레이 추가 (선택 사항)
        }
        uiManager.TurnOverOff();
        uiManager.EnemyTurnOverOff();
        state = GameState.EndTurn;
    }
    void EndTurn()
    {
        Debug.Log("턴 종료 남은코인 : " + characters[characterIdx].coinCounts);
        Debug.Log("턴 종료  플레이어땅 : " + characters[0].ownedTiles.Count + " 적 땅: " + characters[1].ownedTiles.Count);

        uiManager.ShowGroundCount(characters[0].ownedTiles.Count, 0); // 내 땅 최신화
        uiManager.ShowGroundCount(characters[1].ownedTiles.Count, 1); // 적 땅 최신화
        // 턴 종료 로직
        characters[characterIdx].useMoveCoin = 0; // 적을 위한 이동코인 초기화 
       
        characterIdx = (characterIdx + 1) % characters.Length; // 플레이어 교체
        if (characters[characterIdx].coinCounts <= 0)
        {
            //다음 캐릭터가 코인이 없으니 턴은 오지 않음
            characterIdx = (characterIdx + 1) % characters.Length;
        }
        //다음 플레이어를 위해 턴에서 사용했던것들 초기화
        isSuccess = false;
        playerAction = 0;
        coinCount = 0;
        moveCount = 0;
        ownerGround = 0;
        ownerName = null;
        uiManager.SetSelectedCoinCount();// 버튼 초기화
        investPersent = 70; // 투자확률 초기화
        selectMode = 0; // 투자 모드 초기화
        state = GameState.StartTurn;
        
        
    }
    void EndGame()
    {
        int result = -1;
        if (characters[0].ownedTiles.Count > characters[1].ownedTiles.Count)
        {
            Debug.Log("플레이어:" + characters[0].ownedTiles.Count + " 적: " + characters[1].ownedTiles.Count);
            result = 0;
        }
        else if(characters[0].ownedTiles.Count == characters[1].ownedTiles.Count)
        {
            Debug.Log("플레이어:" + characters[0].ownedTiles.Count + " 적: " + characters[1].ownedTiles.Count);
            result = 1;
        }
        else 
        {
            Debug.Log("플레이어:" + characters[0].ownedTiles.Count + " 적: " + characters[1].ownedTiles.Count);
            result = 2; 
        }
        uiManager.ShowGameResult(result);
    }
    
}
