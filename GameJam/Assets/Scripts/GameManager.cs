﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject endOfLevelMenuUI;
    public GameObject Player1HasWonText;
    public GameObject Player2HasWonText;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void PlayerIsDeath(string playerName)
    {
        Time.timeScale = 0f;
        if (playerName == "Player1")
        {
            Player2HasWonText.SetActive(true);
        }
        if (playerName == "PLayer2")
        {
            Player1HasWonText.SetActive(true);
        }
        endOfLevelMenuUI.SetActive(true);
    }

}
