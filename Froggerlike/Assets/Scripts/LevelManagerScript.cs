using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{
    //ui elements
    private Image[] livesImage = new Image[3];
    private TextMeshProUGUI timeCounterText;
    private TextMeshProUGUI scoreCounterText;
    private GameObject pauseMenu;
    private TextMeshProUGUI defeatScoreText;
    private GameObject defeatWidget;
    private TextMeshProUGUI victoryScoreText;
    private GameObject victoryWidget;
    // other
    private bool isGamePaused = false;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        GameManagerScript.instance.OnDeathEvent += PlayerDeath;
        GameManagerScript.instance.OnSuccessEvent += PlayerSuccess;
        livesImage[0] = GameObject.Find("LivesIcon1").GetComponent<Image>();
        livesImage[1] = GameObject.Find("LivesIcon2").GetComponent<Image>();
        livesImage[2] = GameObject.Find("LivesIcon3").GetComponent<Image>();
        UpdateLives();
        timeCounterText = GameObject.Find("TimeTextCount").GetComponent<TextMeshProUGUI>();
        UpdateTime();
        scoreCounterText = GameObject.Find("ScoreTextCount").GetComponent<TextMeshProUGUI>();
        UpdateScore();
        pauseMenu = GameObject.Find("PausePanel");
        pauseMenu.SetActive(false);
        defeatScoreText = GameObject.Find("DefeatScoreText").GetComponent<TextMeshProUGUI>();
        defeatWidget = GameObject.Find("DefeatWidget");
        defeatWidget.SetActive(false);
        victoryScoreText = GameObject.Find("VictoryScoreText").GetComponent<TextMeshProUGUI>();
        victoryWidget = GameObject.Find("VictoryWidget");
        victoryWidget.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (!isGamePaused)
        {
            if (GameManagerScript.instance.roundTime > 0f)
            {
                GameManagerScript.instance.roundTime -= Time.deltaTime;
            }
            else
            {
                GameManagerScript.instance.DeathEvent(this, EventArgs.Empty);
                AudioManager.instance.Play("TimeOutSFX");
            }
        }
        UpdateTime();
        UpdateScore();
    }

    private void PlayerDeath(object sender, EventArgs e)
    {
        if (GameManagerScript.instance.liveCount>1)
        {
            GameManagerScript.instance.liveCount -= 1;
            UpdateLives();
            GameManagerScript.instance.roundTime = 30f;
            UpdateTime();
            transform.position = startPos;

        }
        else
        {
            GameManagerScript.instance.liveCount -= 1;
            UpdateLives();
            AudioManager.instance.Play("GameOverSFX");
            Time.timeScale = 0f;
            defeatWidget.SetActive(true);
            defeatScoreText.text = GameManagerScript.instance.score.ToString();
        }
        
    }
    private void PlayerSuccess(object sender, EventArgs e)
    {
        if (GameManagerScript.instance.successCount < 2)
        {
            GameManagerScript.instance.successCount += 1;
            GameManagerScript.instance.score += Mathf.RoundToInt(GameManagerScript.instance.roundTime*GameManagerScript.instance.difficultyFactor);
            GameManagerScript.instance.roundTime = 30f;
            UpdateTime();
            transform.position = startPos;
            AudioManager.instance.Play("SuccessSFX");
        }
        else
        {
            AudioManager.instance.Play("VictorySFX");
            Time.timeScale = 0f;
            victoryWidget.SetActive(true);
            victoryScoreText.text = GameManagerScript.instance.score.ToString();
        }

    }

    private void UpdateTime()
    {
        timeCounterText.text = GameManagerScript.instance.roundTime.ToString("0.00");
    }
    private void UpdateScore()
    {
        scoreCounterText.text = GameManagerScript.instance.score.ToString();
    }

    private void UnpauseGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
        AudioManager.instance.Play("MainThemeBGM");
    }
    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
        AudioManager.instance.Pause("MainThemeBGM");
    }
    public void ReturnToMenu()
    {
        if (isGamePaused)
        {
            AudioManager.instance.Play("MainThemeBGM");
            Time.timeScale = 1f;
            isGamePaused = false;
        }
        GameManagerScript.instance.MoveToMainMenu();
    }
    public void MoveToLevel2()
    {
        GameManagerScript.instance.MoveToLevel2();
    }
    public void MoveToLevel3()
    {
        GameManagerScript.instance.MoveToLevel3();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            transform.Translate(Vector3.up);
            GameManagerScript.instance.score += 1;
            UpdateScore();
        }
    }
    private void UpdateLives()
    {
        switch (GameManagerScript.instance.liveCount)
        {
            case 3:
                livesImage[2].gameObject.SetActive(true);
                livesImage[1].gameObject.SetActive(true);
                livesImage[0].gameObject.SetActive(true);
                break;
            case 2:
                livesImage[2].gameObject.SetActive(false);
                livesImage[1].gameObject.SetActive(true);
                livesImage[0].gameObject.SetActive(true);
                break;
            case 1:
                livesImage[2].gameObject.SetActive(false);
                livesImage[1].gameObject.SetActive(false);
                livesImage[0].gameObject.SetActive(true);
                break;
            case 0:
                livesImage[2].gameObject.SetActive(false);
                livesImage[1].gameObject.SetActive(false);
                livesImage[0].gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        GameManagerScript.instance.OnDeathEvent -= PlayerDeath;
        GameManagerScript.instance.OnSuccessEvent -= PlayerSuccess;
    }
}
