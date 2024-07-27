using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public BoardMapGenerator boardMapGenerator; // 에디터에서 넣기

    Ground curGround;
    Ground near1;
    Ground near2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //플레이어 포지션을 받아서 해당 땅에 정보 가져오기 in -> call
    //해당 정보를 턴 매니저에게 넘겨주기 out //
    public string OwnerName(int posx, int posy)
    {
        curGround = boardMapGenerator.tileMap[posx, posy].GetComponent<Ground>(); //해당 위치의 땅 스크립트에 접근

        return curGround.GetOwner(); // 주인 이름 받아서 보내기

        
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
    

    //점령 정보 받고 해당 땅에 점령 적용하기 in/call
}
