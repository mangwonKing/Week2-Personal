using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardMapGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab;

    public int gridWidth = 10;        // 그리드의 가로 크기
    public int gridHeight = 10;       // 그리드의 세로 크기

    public GameObject[,] tileMap; // 타일맵 저장 배열

    public bool isGenerate = false;
    // Start is called before the first frame update
    void Start()
    {
        tileMap = new GameObject[gridWidth, gridHeight];
        //GenerateMap();
    }

    public bool CheckRange(int nx, int ny) //유효범위 검사 
    {
        if(nx < 0 || ny < 0 || nx >= gridWidth || ny >= gridHeight)
        {
            return false;
        }
        return true;
    }
    public void GenerateMap()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Vector3 tilePosition = new Vector3(x, gridHeight - 1, 0);
            GameObject tile = Instantiate(hexTilePrefab, tilePosition, Quaternion.Euler(0, 0, 0));
            
            tile.transform.parent = this.transform;
            tileMap[x, gridHeight - 1] = tile;
            Ground ground = tile.GetComponent<Ground>();
            ground.SetPos(x,gridHeight - 1);


        }

        // 하단 가장자리 타일 생성
        for (int x = 0; x < gridWidth; x++)
        {
            Vector3 tilePosition = new Vector3(x, 0, 0);
            GameObject tile = Instantiate(hexTilePrefab, tilePosition, Quaternion.Euler(0, 0, 0));
            tile.transform.parent = this.transform;
            tileMap[x, 0] = tile;
            Ground ground = tile.GetComponent<Ground>();
            ground.SetPos(x, 0);
        }

        // 좌측 가장자리 타일 생성
        for (int y = 0; y < gridHeight; y++)
        {
            Vector3 tilePosition = new Vector3(0, y, 0);
            GameObject tile = Instantiate(hexTilePrefab, tilePosition, Quaternion.Euler(0, 0, 0));
            tile.transform.parent = this.transform;
            tileMap[0, y] = tile;
            Ground ground = tile.GetComponent<Ground>();
            ground.SetPos(0, y);
        }

        // 우측 가장자리 타일 생성
        for (int y = 0; y < gridHeight; y++)
        {
            Vector3 tilePosition = new Vector3(gridWidth - 1, y, 0);
            GameObject tile = Instantiate(hexTilePrefab, tilePosition, Quaternion.Euler(0, 0, 0));
            tile.transform.parent = this.transform;
            tileMap[gridWidth - 1, y] = tile;
            Ground ground = tile.GetComponent<Ground>();
            ground.SetPos(gridWidth - 1, y);
        }

        isGenerate = true;

    }
    
}
