using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    
    public static GameManagerScript instance { get; private set; }
    public event EventHandler OnDeathEvent;
    public event EventHandler OnSuccessEvent;
    public int liveCount = 3;
    public int score = 0;
    public float difficultyFactor = 1.0f;
    public float roundTime = 30f;
    public int successCount = 0;

    private void Awake()
    {
        //Setting up singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void DeathEvent(object sender, EventArgs e)
    {
        OnDeathEvent?.Invoke(sender, e);
    }
    public void SuccessEvent(object sender, EventArgs e)
    {
        OnSuccessEvent?.Invoke(sender, e);
    }
    public void MoveToLevel1()
    {
        liveCount = 3;
        score = 0;
        roundTime = 30f;
        successCount = 0;
        difficultyFactor = 1.0f;
        SceneManager.LoadScene("Level1");
    }
    public void MoveToLevel2()
    {
        roundTime = 30f;
        successCount = 0;
        SceneManager.LoadScene("Level2");
    }
    public void MoveToLevel3()
    {
        roundTime = 30f;
        successCount = 0;
        difficultyFactor = 1.5f;
        SceneManager.LoadScene("Level3");
    }
    public void MoveToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
