using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TurnManager turnManager;
    public BoardMapGenerator boardMapGenerator;
    public GroundManager groundManager;
    //플레이어 ,적 instanciate //
    public Character[] characters;
    bool isGameStart = false;
    bool isSpawn = false;
    private void Start()
    {
        boardMapGenerator.GenerateMap();// 맵 생성하기
        characters[0].coinCounts = 30;
        characters[1].coinCounts = 30; // 코인 개수 초기화


    }
    private void Update()
    {
        if(boardMapGenerator.isGenerate)
        {
            boardMapGenerator.isGenerate = false;
            characters[0].SetInitCharacterPos();
            characters[1].SetInitCharacterPos();
            isSpawn = true;
        }
         if(isSpawn)
        {
            isSpawn = false;
            StartCoroutine(turnManager.GameLoop());
        }
    }

}
