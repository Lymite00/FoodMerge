using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Elements")] 
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject warningPanel;
    
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text losePanelScoreText;

    [SerializeField] private Button restartButton;
    
    [Header("Settings")]
    public int score;
    public int destroyCount;
    
    public bool isFinishedGame = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        destroyCount = 3;
        
        losePanel.SetActive(false);
        restartButton.onClick.AddListener(ResetScene);
    }

    private void ResetScene()
    {
        destroyCount = 3;
        
        score = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    
    private void OpenLostScreen()
    {
        if (isFinishedGame)
        {
            losePanel.SetActive(true);
            losePanelScoreText.text = score.ToString("F0");
            
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void WarningPanel(bool state)
    {
        if (state)
        {
            warningPanel.SetActive(true);
        }
        else
        {
            warningPanel.SetActive(false);
        }
    }
    void Update()
    {
        scoreText.text = score.ToString("F0");
        if (isFinishedGame)
        {
            OpenLostScreen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void OpenUrl()
    {
        Application.OpenURL("https://linktr.ee/lymite");
    }
}
