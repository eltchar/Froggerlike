using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    //other
    private TextMeshProUGUI lifeCounterText;
    void Start()
    {
        GameManagerScript.instance.OnDeathEvent += PlayerDeath;
        GameManagerScript.instance.liveCount = 3;
        lifeCounterText = GameObject.Find("LivesTextCount").GetComponent<TextMeshProUGUI>();
        lifeCounterText.text = GameManagerScript.instance.liveCount.ToString();

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            GameManagerScript.instance.MoveToMainMenu();
        }
    }

    private void PlayerDeath(object sender, EventArgs e)
    {
        print("Opps i died!");
        GameManagerScript.instance.liveCount -= 1;
        lifeCounterText.text = GameManagerScript.instance.liveCount.ToString();
    }

    private void OnDestroy()
    {
        GameManagerScript.instance.OnDeathEvent -= PlayerDeath;
    }
}
