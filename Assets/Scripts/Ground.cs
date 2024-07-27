using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    //소유권
    enum Owner
    {
        None,
        Player,
        Enemy
    }
    Owner owner;
    //인접한 땅(1칸)
    
    public List<Tuple<int,int>> nearGround;

    SpriteRenderer spriteRenderer;
    
    public BoardMapGenerator boardMapGenerator;
    //월드 이벤트 유무
    bool isWorldEvent = false;
    [SerializeField]
    int posX;
    [SerializeField]
    int posY;

    

    int[] dy = { 0, 1, 0, -1 };
    int[] dx = { 1, 0, -1, 0 };
    // Start is called before the first frame update
    void Start()
    {
        nearGround = new List<Tuple<int,int>>();
        owner = Owner.None;// 빈땅으로 시작
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //GetOwner();    
    }
    void FindNearGround() //인접 땅위치 저장
    {
        for(int i = 0;i < 4; i++)
        {
            int ny = posY + dy[i];
            int nx = posX + dx[i];
            //Debug.Log("검사 좌표" + nx + "," + nx);
            if (boardMapGenerator.CheckRange(nx,ny))// 
            {
                if ((ny != 0 && ny != boardMapGenerator.gridHeight - 1) && !(nx == 0 || nx == boardMapGenerator.gridWidth - 1)) continue;
                if ((nx != 0 && nx != boardMapGenerator.gridWidth - 1) && !(ny == 0 || ny == boardMapGenerator.gridHeight - 1)) continue;
                nearGround.Add(new Tuple<int, int>(nx,ny));
               
            }
        }
    }

    public void SetPos(int x, int y)
    {
        posX = x; posY = y;
    }
    public string GetOwner()
    {
        FindNearGround();// 인접 땅의 인덱스 가져옴
        //Debug.Log(System.Enum.GetName(typeof(Owner), owner));
        //Debug.Log("근접 한칸" +nearGround[0].Item1 +","+ nearGround[0].Item2);
        //Debug.Log("근접 한칸" +nearGround[1].Item1 +","+ nearGround[1].Item2);
        return System.Enum.GetName(typeof(Owner), owner);
    }

    public void Occupied(string ownerName) //점령되는 것
    {
        if (ownerName == "Player")
        {
            owner = Owner.Player;
        }
        else
        {
            owner = Owner.Enemy;
        }
        ColorChange();

    }
    void ColorChange()
    {
        if (owner == Owner.Player) 
        {
            // 플레이어
            Color color = Color.blue;
            color.a = 0.5f; // 투명하게 해서 플레이어 보이도록
            spriteRenderer.color = color;
        }
        else
        {
            //적
            Color color = Color.red;
            color.a = 0.5f; 
            spriteRenderer.color = color;
        }
    }
    

}
