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
        CheckOwner,
        SelectAction,
        SafeAction,
        InvestAction,
        ConfirmEndTurn,
        EndTurn,
    }

    public Character[] characters;
    public CoinTossUI moveCoinTossUI;
    public CoinTossUI actionCoinTossUI;
    public GroundManager groundManager;

    private int characterIdx = 0;
    private int coinCount = 0;
    private int moveCount = 0;
    private int ownerGround = 0; // 0 �� �� , 1�� ���� 2�� ���
    int playerAction = 0; //1 �� ���� 2 �� ����
    
    //���� ���°���
    private GameState state;

    void Start()
    {
        characterIdx = 0;
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
            }
            yield return null;
        }
    }

    void StartTurn()
    {
        moveCoinTossUI.InitCoinView();
        actionCoinTossUI.InitCoinView();
        // �� ���� ����
        //if (!isSelectMoveCoin)
        //{
        //    //Debug.Log("�Ͻ���!");
        //    //state = GameState.SelectMoveCoin;
        //}
        Debug.Log("�Ͻ���!");
        state = GameState.SelectMoveCoin;
    }

    void SelectMoveCoin()
    {
        coinCount = characters[characterIdx].GetSelectedCoinCount();
        
        if (coinCount > 0)
        {
            Debug.Log("�̵� ���� ����!");

            characters[characterIdx].coinCounts -= coinCount; // ��� ���θ�ŭ ���� , 1ȸ��
            state = GameState.MoveCoinFlip;
        }
    }

    void MoveCoinFlip()
    {
        moveCount = characters[characterIdx].MoveCoinFlip(coinCount);
        if (moveCount > 0)
        {
            Debug.Log("�̵� ���� ����!");
            moveCoinTossUI.ShowCoinResult(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();// ������ ���� ����
            state = GameState.Move;
        }
    }

    void Move()
    {
        characters[characterIdx].Move(moveCount);
        Debug.Log("�̵�!");

        state = GameState.CheckOwner;
    }
    void CheckOwner()
    {
        ownerName = groundManager.OwnerName(characters[characterIdx].GetPosx(), characters[characterIdx].GetPosy()); //�� ���� �ް�
        if (ownerName == "None") ownerGround = 0;
        else if (ownerName == characters[characterIdx].GetCharacterType())
        {
            Debug.Log("������ ��ҳ׿� ���� ���ʽ�!");
            characters[characterIdx].coinCounts += 2; // ������ �ٷ� ���� �ְ� ������
            state = GameState.ConfirmEndTurn;
        }
        else ownerGround = 2;
        Debug.Log(ownerName + "�� �Դϴ�.");
        state = GameState.SelectAction;
    }
    //���� �׼� �����̶� ������ ���� ���� -> �׼��� �����ϸ� ������ ������ �ѱ�� �غ���.
    void SelectAction()
    {
        playerAction = characters[characterIdx].SelectAction(); // �÷��̾ ���ڸ� ���� ������ ���� ����
        if(playerAction > 0)
        {
            Debug.Log("�ൿ : " + playerAction); 
            if (playerAction == 1) // ����
            {
                Debug.Log(" ���� ���¸� �����߽��ϴ�.");
                state = GameState.SafeAction;
            }
            else if (playerAction == 2) //����
            {
                Debug.Log(" ���� ���¸� �����߽��ϴ�.");

                state = GameState.InvestAction;
            }
        }
    }
    //���� ����: ����Ʈ �׼ǿ��� �� ���縦 �ľ��ϰ� ������ ���� �ְ� �׼��� �����ϴ°� �ùٸ� ���� 
    void SafeAction() // ���� �׼� 
    {
        Debug.Log("���� �׼�!");

        if (ownerGround == 0) //��
        {
            characters[characterIdx].coinCounts--; //���� 1�� �Ҹ�
            groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // ĳ������ �̸��� �Ѱ��ְ� ����
            state = GameState.ConfirmEndTurn;
        }
        else if (ownerGround == 2) // �� ��
        {
            characters[characterIdx].coinCounts--;
            state = GameState.ConfirmEndTurn;
        }
        else // ������ �� �ִ��� ���� ���ִ��� ����
        {
            state = GameState.ConfirmEndTurn;
            Debug.Log(" ���� ����");
            return;
        }
    }
    void InvestAction() // ���� �׼� 
    {
        Debug.Log("���ھ׼�!");
        bool isSuccess = true;
        // �� ���ο� ���� �ٸ��� �ϱ�
        //�ϴ� 2�� ���� �����ؼ� �����°ɷ�
        characters[characterIdx].coinCounts -= 2; // 2�� �Ҹ��ؼ� ����
        //���� �ø�
        for(int i = 0; i<  2; i++)
        {
            if (!characters[characterIdx].CoinFlip(50))
            {
                isSuccess = false;
               //����!
            }// �ϴ� �� 50�� 
            
        }
        actionCoinTossUI.ShowCoinResult(characters[characterIdx].coinFlipResult);
        characters[characterIdx].coinFlipResult.Clear();
        if (isSuccess) // ����
        {
            groundManager.OccupyThreeGround(characters[characterIdx].GetCharacterType());
        }
        state = GameState.ConfirmEndTurn;
    }
    IEnumerator ConfirmEndTurn()
    {
        if (characterIdx == 0)
        {
            Debug.Log("���� �ѱ�ڽ��ϱ�? (F Ű�� ���� Ȯ��)");

            while (!Input.GetKeyDown(KeyCode.F))
            {
                yield return null; // F Ű �Է��� ��ٸ�
            }
        }
        else
        {
            Debug.Log("��ǻ���� ���� �������ϴ�.");
            yield return new WaitForSeconds(3f); // �ణ�� ������ �߰� (���� ����)
        }

        state = GameState.EndTurn;
    }
    void EndTurn()
    {
        Debug.Log("�� ���� �������� : " + characters[characterIdx].coinCounts);
        // �� ���� ����

        characterIdx = (characterIdx + 1) % characters.Length; // �÷��̾� ��ü

        //���� �÷��̾ ���� �Ͽ��� ����ߴ��͵� �ʱ�ȭ
        playerAction = 0;
        coinCount = 0;
        moveCount = 0;
        ownerGround = 0;
        ownerName = null;
        state = GameState.StartTurn;
        
    }
    
}
