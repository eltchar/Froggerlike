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
    public int liveCount = 3;
    public float difficultyFactor = 1.0f;

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
    public void MoveToLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void MoveToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
