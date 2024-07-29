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
        InvestCustom,
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
    private int ownerGround = 0; // 0 �� �� , 1�� ���� 2�� ���
    int playerAction = 0; //1 �� ���� 2 �� ����


    int investPersent = 70; // �⺻ 70 
    int selectMode = 0;

    int otherIdx;
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
        //���۽� ���� �˷��ֱ�
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
            Debug.Log("��������");
            state = GameState.EndGame;
        }
        else
        {
            otherIdx = characterIdx + 1 == 2 ? 0 : 1; // ���� �ε��� ����
            uiManager.InitCoinView();
            // �� ���� ����
            //if (!isSelectMoveCoin)
            //{
            //    //Debug.Log("�Ͻ���!");
            //    //state = GameState.SelectMoveCoin;
            //}
            Debug.Log("�Ͻ���!");
            state = GameState.SelectMoveCoin;
        }
    }
    void SelectMoveCoin()
    {
        uiManager.SelectMoveCoinViewOn(characterIdx); // ���� �� �� ������ ui ���

        coinCount = characters[characterIdx].GetSelectedCoinCount();
        
        if (coinCount > 0)
        {
            Debug.Log("�̵� ���� ����!");
            characters[characterIdx].coinCounts -= coinCount; // ��� ���θ�ŭ ���� , 1ȸ��
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts,characterIdx); // idx �� ĳ���� ����
            uiManager.SelectMoveCoinViewOff(characterIdx); // ���� ��ư ����

            state = GameState.MoveCoinFlip;
        }
    }

    void MoveCoinFlip()
    {
        moveCount = characters[characterIdx].MoveCoinFlip(coinCount);
        if (moveCount > 0)
        {
            Debug.Log("�̵� ���� ����!");
            uiManager.ShowMoveCoinTossUI(characters[characterIdx].coinFlipResult);// ui �Ŵ����� ����� �޳� �׷� �̶� ������ �������� �������.
            characters[characterIdx].coinFlipResult.Clear();// ������ ���� ����
            state = GameState.Move;
        }
    }

    void Move()
    {
        characters[characterIdx].Move(moveCount);
        Debug.Log("�̵�!");
        //�̵� ��� ���� �����
        state = GameState.WaitMove;
    }
    void WaitMove()
    {
        if (characters[characterIdx].isMoveEnd) //�̵��� ��ٸ�����
        {
            state = GameState.CheckOwner;
            uiManager.MoveResultOff();// �̵� �Ϸ������� ����
        }
    }
    void CheckOwner()
    {
        ownerName = groundManager.OwnerName(characters[characterIdx].GetPosx(), characters[characterIdx].GetPosy()); //�� ���� �ް�
        List<string> list = groundManager.GetNearOwner(); // ���� ���� ����
        if (ownerName == "None")
        {
            ownerGround = 0;
            //�󶥿��� ������ ����� Ȯ�� ���� 
            CalculatePersent(list);
        }
        else if (ownerName == characters[characterIdx].GetCharacterType())
        {
            Debug.Log("������ ��ҳ׿� ���� ���ʽ�!");
            characters[characterIdx].coinCounts += 2; // ������ ���� �ְ� �ൿ ����
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
            ownerGround = 1;
            CalculatePersent(list);
        }
        else
        {
            ownerGround = 2;
        }
        //Debug.Log(ownerName + "�� �Դϴ�.");
        if(characterIdx == 0)
            uiManager.GroundInfoOn(ownerGround); // �÷��̾�� ui Ű��
        
        state = GameState.SelectAction;
        
    }
    void CalculatePersent(List<string> list)
    {
        
        foreach (var item in list)
        {
            Debug.Log("������ ���� ���� : " + item);
            if (item != characters[characterIdx].GetCharacterType() && item != "None") // �󶥵� �ƴϰ� �̸��� �ٸ��ٸ� ������ ���� 
            {
                Debug.Log("���� Ȯ���� ���ҽ�ŵ�ϴ�.");
                investPersent -= 10;
            }
        }
        Debug.Log("�̹� ������ ������ ��� Ȯ���� " + investPersent + "%�Դϴ�.");
    }
    /*
     ������ �� ������ -> �� ���� + �ൿ���� ��ư�� ������ 

     Ȯ���� ������������ �׳� �� ����� �׸��� �� ���ô� ���� �������� �̷��� ���°� �� �� ���� ��������.
     �� / ����
     ������ + �ൿ���� -> ��� -> �� �ѱ��
     ����
     ������ + �ൿ���� -> ���ڴ� �ൿ����2 -> ��� -> �� �ѱ��

        �� �ؽ�Ʈ�� ����Ʈ�� ��� ownesrGround ���ڿ� �´� �迭 ����ϵ��� ����.

    �÷��̾�� �� ������ �� �ʿ������� ��ǻ���� ����
    � �ൿ�� �����ߴ��� �׸��� �� ��� �� ������ �� �� ����.
     */

    void SelectAction()
    {
        if (characterIdx == 0)
            uiManager.ActionSelectButtonOn();
        playerAction = characters[characterIdx].SelectAction(); // �÷��̾ ���ڸ� ���� ������ ���� ����
        
        if (playerAction > 0)
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


            if (characterIdx == 0)
            {
                uiManager.GroundInfoOff(ownerGround); // �׼� ���������� ������
                uiManager.ActionSelectButtonOff();
            }

        }
    }


    void SafeAction() // ���� �׼� 
    {
        Debug.Log("���� �׼�!");

        if (ownerGround == 0) //��
        {
            characters[characterIdx].coinCounts--; //���� 1�� �Ҹ�

            groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // ĳ������ �̸��� �Ѱ��ְ� ����
            characters[characterIdx].ownedTiles.Add(groundManager.curGround); // ĳ������ ���� ����Ʈ�� ����

            state = GameState.ConfirmEndTurn;
        }
        else if (ownerGround == 2) // �� ��
        {
            characters[characterIdx].coinCounts--;
            state = GameState.ConfirmEndTurn;
        }
        else // ���� �׳� �Ѿ��
        {
            state = GameState.ConfirmEndTurn;
        }
        uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
    }
    void InvestAction() // ���� �׼� 
    {
        Debug.Log("���ھ׼�!");
        bool isSuccess = true;
        // �� ���ο� ���� �ٸ��� �ϱ�
        //��
        if (ownerGround == 0 || ownerGround == 1) //�� Ȥ�� ����
        {
            characters[characterIdx].coinCounts -= 2; // 2�� �Ҹ��ؼ� ����

                                                      //���� �ø�
            for (int i = 0; i < 2; i++)
            {
                if (!characters[characterIdx].CoinFlip(investPersent)) // Ȯ�� ����
                {
                    isSuccess = false;
                    //����!
                }

            }
            uiManager.ShowActionCoinTossUI(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();
            if (isSuccess) // ����
            {
                groundManager.OccupyThreeGround(characters[characterIdx].GetCharacterType());
                characters[characterIdx].ownedTiles.Add(groundManager.curGround); // ĳ������ ���� ����Ʈ�� ����
                characters[characterIdx].ownedTiles.Add(groundManager.near1); // ĳ������ ���� ����Ʈ�� ����
                characters[characterIdx].ownedTiles.Add(groundManager.near2); // ĳ������ ���� ����Ʈ�� ����
            }
            
            state = GameState.ConfirmEndTurn;
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);
        }

        else // �� ��
        {
            // issuccess �� false �� �� �ϳ� ���ѱ�� �ϱ�
            selectMode = characters[characterIdx].SelectInvestMode();
            if (selectMode > 0)
            {
                state = GameState.InvestCustom;
            }
        }

    }
    // 2�� �����ϴ� �� , 3�� �����ϴ� �� �����ϱ�
    void InvestCustom()
    {
        bool isSuccess = true;
        if (selectMode == 1)
        {
            Debug.Log("2�� �����մϴ�!");
            // ���� 2�� �ø��� �� ���� ������ �� ���ұ� 
            characters[characterIdx].coinCounts -= 2; // 2�� ����
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);

            //70�� �״�� ���� �ϴ���
            //investPersent += 15; 

            for (int i = 0; i < 2; i++)
            {
                if (!characters[characterIdx].CoinFlip(investPersent)) // Ȯ�� ����
                {
                    isSuccess = false;
                    //����!
                }
            }
            uiManager.ShowActionCoinTossUI(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();
            if(isSuccess)
            {
                groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // �ش� �� ���ұ� ����
                characters[characterIdx].ownedTiles.Add(groundManager.curGround); // ĳ������ ���� ����Ʈ�� ����
                characters[otherIdx].ownedTiles.Remove(groundManager.curGround);// ���� ����Ʈ���� ����
                Debug.Log("�� ���ұ� ����!");
            }
            else
            {
                // ĳ������ ������ ����Ʈ���� ���ڸ� �ϳ� �������� ���Ѵ�.
                if (characters[characterIdx].ownedTiles.Count == 0)
                {
                    Debug.Log("���� ���� ���׿�?");
                }
                else
                {
                    int steelGround = Random.Range(0, characters[characterIdx].ownedTiles.Count); // 2���� 0~1 �߿� �ϳ� ���� 
                    string steelName = characterIdx == 0 ? "Enemy" : "Player"; // ����� �Ŵϱ� �ݴ��
                    
                    characters[characterIdx].ownedTiles[steelGround].Occupied(steelName);
                    characters[otherIdx].ownedTiles.Add(characters[characterIdx].ownedTiles[steelGround]); // �� ���ɰ� ���ÿ� ���� ����Ʈ�� �߰�
                    characters[characterIdx].ownedTiles.RemoveAt(steelGround); // �����׼� �ش� �ε��� �׶��� ����
                   
                    Debug.Log(" ���� ������ϴ�.");
                }
                
            }
            
        }
        else
        {
            //���� 3�� �ø� �� ���� ������ �ش� �� + �� ������ 2�� ���ұ�
            Debug.Log("3�� �����մϴ�!");
            characters[characterIdx].coinCounts -= 3; // 3�� ����
            uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);

            //�⺻ 70�۷� �ϱ�
            //investPersent += 20; // �ϴ� ������ 70�ۼ�Ʈ�� ���� 

            for (int i = 0; i < 3; i++)
            {
                if (!characters[characterIdx].CoinFlip(investPersent)) // Ȯ�� ����
                {
                    isSuccess = false;
                    //����!
                }
            }
            uiManager.ShowActionCoinTossUI(characters[characterIdx].coinFlipResult);
            characters[characterIdx].coinFlipResult.Clear();
            if(isSuccess) //������ �� ���ұ�!
            {
                groundManager.OccupyGround(characters[characterIdx].GetCharacterType()); // �ش� �� ���ұ� ����
                characters[characterIdx].ownedTiles.Add(groundManager.curGround); // ĳ������ ���� ����Ʈ�� ����
                characters[otherIdx].ownedTiles.Remove(groundManager.curGround);// ���� ����Ʈ���� ����

                Debug.Log("�� ���ұ� ����!");
                // ������ 2�� ���ұ� -> ���� �߰��ϱ� // ���� ���� 1���� �ѹ��� , 0���� �Ѿ��
                int ground = 2; // �⺻ 2�� ������

                if (characters[otherIdx].ownedTiles.Count == 1)
                {
                    ground = 1;
                    Debug.Log("������ ���� �ϳ��ۿ� ���׿�... ������ �Ѱ� �����帱�Կ�");
                    characters[characterIdx].coinCounts += 1;
                }
                else if(characters[otherIdx].ownedTiles.Count == 0)
                {
                    ground = 0;
                    Debug.Log("������ ���� ���׿�... ������ �����帱�Կ�");
                    characters[characterIdx].coinCounts += 3;
                }
                uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);

                for (int i = 0; i < ground; i++)
                {
                    int steelGround = Random.Range(0, characters[otherIdx].ownedTiles.Count); // ���̰��� ���� �� �ϳ��� ������
                    string steelName = characterIdx == 0 ? "Player" : "Enemy"; // ���Ҵ� �Ŵϱ� ����� ;

                    characters[otherIdx].ownedTiles[steelGround].Occupied(steelName); //���� �� ���ұ�
                    characters[characterIdx].ownedTiles.Add(characters[otherIdx].ownedTiles[steelGround]); //���ҾƼ� �� ����Ʈ�� �ֱ�
                    characters[otherIdx].ownedTiles.RemoveAt(steelGround); // ���� �ش� �ε��� �׶��� ����
                    Debug.Log("���� ���Ҿҽ��ϴ�.");
                }

            }
            else
            {
                // ĳ������ ������ ����Ʈ���� ���ڸ� �ϳ� �������� ���Ѵ�.
                if (characters[characterIdx].ownedTiles.Count == 0)
                {
                    Debug.Log("���� ���� ���׿�?");
                }
                else
                {
                    int steelGround = Random.Range(0, characters[characterIdx].ownedTiles.Count); // 2���� 0~1 �߿� �ϳ� ���� 
                    string steelName = characterIdx == 0 ? "Enemy" : "Player"; // ����� �Ŵϱ� �ݴ��

                    characters[characterIdx].ownedTiles[steelGround].Occupied(steelName);
                    characters[otherIdx].ownedTiles.Add(characters[characterIdx].ownedTiles[steelGround]); // �� ���ɰ� ���ÿ� ���� ����Ʈ�� �߰�
                    characters[characterIdx].ownedTiles.RemoveAt(steelGround); // �����׼� �ش� �ε��� �׶��� ����

                    Debug.Log(" ���� ������ϴ�.");
                }

            }

        }
        uiManager.ShowCoinCount(characters[characterIdx].coinCounts, characterIdx);

        state = GameState.ConfirmEndTurn; // �� �ѱ��
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
        characters[characterIdx].useMoveCoin = 0; // ���� ���� �̵����� �ʱ�ȭ 
       
        characterIdx = (characterIdx + 1) % characters.Length; // �÷��̾� ��ü
        if (characters[characterIdx].coinCounts <= 0)
        {
            //���� ĳ���Ͱ� ������ ������ ���� ���� ����
            characterIdx = (characterIdx + 1) % characters.Length;
        }
         //���� �÷��̾ ���� �Ͽ��� ����ߴ��͵� �ʱ�ȭ
        playerAction = 0;
        coinCount = 0;
        moveCount = 0;
        ownerGround = 0;
        ownerName = null;
        uiManager.SetSelectedCoinCount();// ��ư �ʱ�ȭ
        investPersent = 50; // ����Ȯ�� �ʱ�ȭ
        selectMode = 0; // ���� ��� �ʱ�ȭ
        state = GameState.StartTurn;
        
        
    }
    void EndGame()
    {
        Debug.Log("��������");
    }
    
}
