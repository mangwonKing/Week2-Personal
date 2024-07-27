using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinCountUI : MonoBehaviour
{
    public TextMeshProUGUI characterCoin;
    public string characterType;

    
    public void ShowCoinCount(int coinCount)
    {
        characterCoin.text = characterType + coinCount;
    }
}
