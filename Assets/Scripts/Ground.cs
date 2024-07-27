using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    //������
    enum Owner
    {
        None,
        Player,
        Enemy
    }
    Owner owner;
    //������ ��(1ĭ)
    
    public List<Tuple<int,int>> nearGround;

    SpriteRenderer spriteRenderer;
    
    public BoardMapGenerator boardMapGenerator;
    //���� �̺�Ʈ ����
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
        owner = Owner.None;// ������ ����
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //GetOwner();    
    }
    void FindNearGround() //���� ����ġ ����
    {
        for(int i = 0;i < 4; i++)
        {
            int ny = posY + dy[i];
            int nx = posX + dx[i];
            //Debug.Log("�˻� ��ǥ" + nx + "," + nx);
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
        FindNearGround();// ���� ���� �ε��� ������
        //Debug.Log(System.Enum.GetName(typeof(Owner), owner));
        //Debug.Log("���� ��ĭ" +nearGround[0].Item1 +","+ nearGround[0].Item2);
        //Debug.Log("���� ��ĭ" +nearGround[1].Item1 +","+ nearGround[1].Item2);
        return System.Enum.GetName(typeof(Owner), owner);
    }

    public void Occupied(string ownerName) //���ɵǴ� ��
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
            // �÷��̾�
            Color color = Color.blue;
            color.a = 0.5f; // �����ϰ� �ؼ� �÷��̾� ���̵���
            spriteRenderer.color = color;
        }
        else
        {
            //��
            Color color = Color.red;
            color.a = 0.5f; 
            spriteRenderer.color = color;
        }
    }
    

}
