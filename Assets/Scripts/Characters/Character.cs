using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//ĳ������ �ൿ�� ���� ����
public class Character : MonoBehaviour
{
    enum Type
    {
        Player,
        Enemy
    }

    [SerializeField]
    Type type;

    //���� ������� ����� ����
    public int useMoveCoin;
    public int useActionCoin;

    public int coinCounts = 30;
    public List<Ground> ownedTiles; //������ ��
    public int turn;

    //�̵� ����
    int[] dirX = { 0, 1, 0, -1 };
    int[] dirY = { 1, 0, -1, 0 }; //��, ��, �Ʒ� , ��
    int dirIndex = 0; //���� dir = 0

    //���� ��ġ
    int posX = 0;
    int posY = 0;
    //�� Ÿ�� ����
    BoardMapGenerator tile;

    public bool isInvest = false;
    public bool isMoveEnd = false; //�̵��� ��������

    public int isSafe = 0; // 0 �� ���� ���� ��
    public List<bool> coinFlipResult; //ui�Ŵ��� Ȥ�� ui �� �̰� �������� �̹����� ���µ� ����.

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
    // ĳ���Ͱ� ������ ���
    // 1. �Ͻ��۽� ���� ���� ���ϱ�
    public  virtual int GetSelectedCoinCount() //���� �ϴ� 3������ 
    {
        //coinCounts -= useMoveCoin; // ���� ���
        return useMoveCoin;
    }
    // 2. ���� ���� ���ϸ� ���� ������ 
    public int MoveCoinFlip(int coinCount)
    {
        int move = 0;
        coinFlipResult.Clear(); //������ ���¸� �ʱ�ȭ
        for (int i = 0; i < coinCount; i++)
        {
            if (CoinFlip(50)) // �����Գ� 2, �����ϸ� 1 �ް� �׷� 2���� ������ 2~4 , 3���� 3~ 6 �� �� ��
            {
                move +=2;
            }
            else
            {
                 move +=1; 
            }
            //Debug.Log(move + "Ƚ�� �̵� ����!");

        }
        if(move ==6)//���ʽ� ����  3�� ����
        {
            coinCount += 3;
            Debug.Log("���� ���� ���ʽ�!");
        }
        return move;
    }
    
    public bool CoinFlip(float persentage) // 0~100 �߿� 50�̸��� ������ �޸� ���� , �̻��� ������ �ո� ���� 
    {
        if(Random.Range(0,100) < 100 - persentage) // 60�۸� 0~40 �� ���� , 70�۸� 0~30�� ���� 
        {
            Debug.Log("����!");
            coinFlipResult.Add(false); // ���� �ø��� ����� ����
            return false;
        }
        else
        {
            Debug.Log("����!");
            coinFlipResult.Add(true);
            return true;
        }
    }
    // 3. ���� ��� ������ �׸�ŭ �̵��ϱ�
    public void Move(int moveCount)
    {
        Debug.Log("�̵����Դϴ�.");
        StartCoroutine(OneTimeOneMove(moveCount));
    }
    IEnumerator OneTimeOneMove(int moveCount)
    {
        isMoveEnd = false;
        while (moveCount > 0)
        {
            int ny = posY + dirY[dirIndex];
            int nx = posX + dirX[dirIndex];
            while (!tile.CheckRange(nx, ny)) // �ش� �������� ���� �������� �˻�
            {
                DirChange();
                nx = posX + dirX[dirIndex];
                ny = posY + dirY[dirIndex];
            }
            //Debug.Log(nx + "," + ny + " ��ġ�� �̵�!");
            transform.position = tile.tileMap[nx, ny].transform.position + offset; // �̵�
            posX = nx; posY = ny;
            Debug.Log(posX + "," + posY + "�� �̵�");
            moveCount--;// �̵�Ƚ�� ����
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
        // 0 �� 1 �� ��� ����
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
