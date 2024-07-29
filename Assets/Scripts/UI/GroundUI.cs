using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroundUI : MonoBehaviour
{
    public TextMeshProUGUI characterGound;
    public string characterType;

    public void ShowGroundCount(int coinCount)
    {
        characterGound.text = characterType + coinCount;
    }
}
