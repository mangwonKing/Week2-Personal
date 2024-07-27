using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//캐릭터의 행동과 스탯 관리
public class Character : MonoBehaviour
{
    enum Type
    {
        Player,
        Enemy
    }

    [SerializeField]
    Type type;

    //동전 던지기시 사용할 동전
    public int useMoveCoin;
    public int useActionCoin;

    public int coinCounts = 30;
    public List<Ground> ownedTiles; //소유한 땅
    public int turn;

    //이동 방향
    int[] dirX = { 0, 1, 0, -1 };
    int[] dirY = { 1, 0, -1, 0 }; //위, 우, 아래 , 좌
    int dirIndex = 0; //시작 dir = 0

    //현재 위치
    int posX = 0;
    int posY = 0;
    //맵 타일 정보
    BoardMapGenerator tile;

    public bool isInvest = false;
    public bool isMoveEnd = false; //이동이 끝났는지

    public int isSafe = 0; // 0 은 선택 안한 값
    public List<bool> coinFlipResult; //ui매니저 혹은 ui 가 이걸 가져가서 이미지를 띄우는데 쓴다.

    public Vector3 offset;

    private void Start()
    {
        tile = GameObject.Find("MapGenerator").GetComponent<BoardMapGenerator>();
        if(type == Type.Player )
        {
            offset = new Vector3(-0.25f,0,0);
        }
        else
        {
            offset = new Vector3(0.25f,0,0);
        }
        // Debug.Log("POSYX : " + posX + posY);
    }
    public void SetInitCharacterPos()
    {
        transform.position = tile.tileMap[0, 0].transform.position + offset;
    }
    // 캐릭터가 수행할 기능
    // 1. 턴시작시 동전 개수 정하기
    public  virtual int GetSelectedCoinCount() //동전 일단 3개까지 
    {
        //coinCounts -= useMoveCoin; // 코인 사용
        return useMoveCoin;
    }
    // 2. 동전 개수 정하면 동전 굴리기 
    public int MoveCoinFlip(int coinCount)
    {
        int move = 0;
        coinFlipResult.Clear(); //기존의 상태를 초기화
        for (int i = 0; i < coinCount; i++)
        {
            if (CoinFlip(50)) // 성공함녀 2, 실패하면 1 받게 그럼 2개를 굴리면 2~4 , 3개면 3~ 6 이 될 것
            {
                move +=2;
            }
            else
            {
                 move +=1; 
            }
            //Debug.Log(move + "횟수 이동 가능!");

        }
        if(move ==6)//보너스 코인  3개 제공
        {
            coinCount += 3;
            Debug.Log("전부 성공 보너스!");
        }
        return move;
    }
    
    public bool CoinFlip(float persentage) // 0~100 중에 50미만이 나오면 뒷면 실패 , 이상이 나오면 앞면 성공 
    {
        if(Random.Range(0,100) < 100 - persentage) // 60퍼면 0~40 이 실패 , 70퍼면 0~30이 실패 
        {
            Debug.Log("실패!");
            coinFlipResult.Add(false); // 코인 플립의 결과를 저장
            return false;
        }
        else
        {
            Debug.Log("성공!");
            coinFlipResult.Add(true);
            return true;
        }
    }
    // 3. 동전 결과 나오면 그만큼 이동하기
    public void Move(int moveCount)
    {
        Debug.Log("이동중입니다.");
        StartCoroutine(OneTimeOneMove(moveCount));
    }
    IEnumerator OneTimeOneMove(int moveCount)
    {
        isMoveEnd = false;
        while (moveCount > 0)
        {
            int ny = posY + dirY[dirIndex];
            int nx = posX + dirX[dirIndex];
            while (!tile.CheckRange(nx, ny)) // 해당 방향으로 전진 가능한지 검사
            {
                DirChange();
                nx = posX + dirX[dirIndex];
                ny = posY + dirY[dirIndex];
            }
            //Debug.Log(nx + "," + ny + " 위치로 이동!");
            transform.position = tile.tileMap[nx, ny].transform.position + offset; // 이동
            posX = nx; posY = ny;
            Debug.Log(posX + "," + posY + "로 이동");
            moveCount--;// 이동횟수 감소
            yield return new WaitForSeconds(1f);
        }
        isMoveEnd = true;

    }
    public virtual int SelectInvestMode()
    {
        return 0;
    }
    void DirChange()
    {
        dirIndex++;
        if (dirIndex >= 4)
        {
            dirIndex = 0;
        }
    }
    public virtual int SelectAction()
    {
        // 0 과 1 중 골라서 선택
        return 0;
    }

    public int GetPosx()
    {
        return posX;
    }
    public int GetPosy()
    {
        return posY;
    }
    public string GetCharacterType()
    {
        return System.Enum.GetName(typeof(Type), type);
    }

}
