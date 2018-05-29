﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript: MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject gameOverCanvas;
    public GameObject onScreenUICanvas;
    //if multiplayer, change this concept
    public GameObject player;
    public Text gameOverScore;
    public Text highScore;

    public bool isPaused = false;
    public bool isGameOver = false;

    private void Start()
    {
        Time.timeScale = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        highScore.text = PlayerPrefs.GetFloat("HighScore", 0).ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            PauseOrResume();
        }
    }

    public void loadlevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PauseOrResume()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPaused = true;
            //player.GetComponent<CameraScript>().enabled = false;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            //player.GetComponent<CameraScript>().enabled = true;
        }
        pauseCanvas.SetActive(isPaused);
        cameraPause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        onScreenUICanvas.SetActive(false);
        isPaused = true;
        cameraPause();
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        float score = gameObject.GetComponent<GameManagerScript>().maxHeightAchieved;
        score = Mathf.Round(score * 100f) / 100f;
        gameOverScore.text = score.ToString();

        if (score > PlayerPrefs.GetFloat("HighScore", 0))
        {
            PlayerPrefs.SetFloat("HighScore", score);
            highScore.text = score.ToString();
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }

    private void cameraPause()
    {
        CameraScript[] scripts = player.GetComponentsInChildren<CameraScript>();
        foreach (CameraScript cam in scripts)
        {
            cam.paused = isPaused;
        }
    }

}
