using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleStart : MonoBehaviour
{
    public Button startButton;
    public Button guideButton;

    public Button returnButton;

    public Canvas title;
    public Canvas guide;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
        guideButton.onClick.AddListener(OnGuideButtonClick);
        returnButton.onClick.AddListener(OnReturnButtonClick);
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(1);
    }
    public void OnGuideButtonClick()
    {
        title.gameObject.SetActive(false);
        guide.gameObject.SetActive(true);
    }
    public void OnReturnButtonClick()
    {
        title.gameObject.SetActive(true);
        guide.gameObject.SetActive(false);
    }
}
