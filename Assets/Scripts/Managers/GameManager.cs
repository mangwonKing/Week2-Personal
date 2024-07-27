using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
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
