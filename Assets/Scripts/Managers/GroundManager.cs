using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public BoardMapGenerator boardMapGenerator; // �����Ϳ��� �ֱ�

    Ground curGround;
    Ground near1;
    Ground near2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //�÷��̾� �������� �޾Ƽ� �ش� ���� ���� �������� in -> call
    //�ش� ������ �� �Ŵ������� �Ѱ��ֱ� out //
    public string OwnerName(int posx, int posy)
    {
        curGround = boardMapGenerator.tileMap[posx, posy].GetComponent<Ground>(); //�ش� ��ġ�� �� ��ũ��Ʈ�� ����

        return curGround.GetOwner(); // ���� �̸� �޾Ƽ� ������

        
    }
    public void OccupyGround(string ownerName)
    {
        curGround.Occupied(ownerName);
    }
    public void OccupyThreeGround(string ownerName)
    {
        near1 = boardMapGenerator.tileMap[curGround.nearGround[0].Item1, curGround.nearGround[0].Item2].GetComponent<Ground>();
        near2 = boardMapGenerator.tileMap[curGround.nearGround[1].Item1, curGround.nearGround[1].Item2].GetComponent<Ground>();
        curGround.Occupied(ownerName);
        near1.Occupied(ownerName);
        near2.Occupied(ownerName);

    }
    

    //���� ���� �ް� �ش� ���� ���� �����ϱ� in/call
}
