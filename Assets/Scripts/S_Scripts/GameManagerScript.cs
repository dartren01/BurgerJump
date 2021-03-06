﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {

    private Transform player;
    public float maxHeightAchieved = 0f;
    public float currheight = 0f;

    public Text maxHeightText;
    public Text fpsText;
    public Text GroundRisingText;
    public float deltaTime;

    public GameObject ground;
    private GroundScript groundScript;
    public GameObject[] poolers;
    private ObjectPoolerScript[] platforms;

    public float platformPos = 10f;

    //private bool startClimb = false;
    public float heightAchievs;
    //public bool instantiatePlatforms = false;

    private bool isSped = false;

    private bool gameover;

	// Use this for initialization
	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;
        heightAchievs = 100f;
        gameover = false;
        groundScript = ground.GetComponent<GroundScript>();
        platforms = new ObjectPoolerScript[poolers.Length];
        GroundRisingText.enabled = false;
        for (int i = 0; i < poolers.Length; ++i)
        {
            platforms[i] = poolers[i].GetComponent<ObjectPoolerScript>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (player.position.y > maxHeightAchieved)
        {
            maxHeightAchieved = player.position.y;
        }

        //UI TEXTS
        maxHeightAchieved = Mathf.Round(maxHeightAchieved * 100f) / 100f;
        //currheight = Mathf.Round(player.position.y * 100f) / 100f;
        maxHeightText.text = "Score: " + Mathf.Round(maxHeightAchieved*6).ToString();
        //currentHeightText.text = "Current Height: " + currheight.ToString();

        float distFromGround = Mathf.Round(player.position.y - ground.transform.position.y -2f);

        //deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        //float fps = 1.0f / deltaTime;
        //fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();

        //starts moving ground
        if (maxHeightAchieved > 90f)
        {
            if (groundScript.enabled == false)
            {
                StartCoroutine("GroundRising");
            }
            groundScript.enabled = true;
            //instantiatePlatforms = true;
            
            for(int i = 0; i < platforms.Length; ++i)
            {
                GameObject platform = platforms[i].GetPooledObject();
                if (platform != null)
                {
                    platform.SetActive(true);
                }
            }
            
        }

        if(maxHeightAchieved > heightAchievs && groundScript.speed < 10)
        {
            groundScript.addSpeed(1);
            heightAchievs += 300f;
        }

        if(distFromGround > 120f && !isSped)
        {
            isSped = true;
            StartCoroutine("boostGround");
        }

        if(player.position.y < ground.transform.position.y && !gameover)
        {
            this.GetComponent<SceneManagerScript>().GameOver();
            gameover = true;
        }
    }

    public void addPlatformPos(float amount)
    {
        platformPos += amount;
    }

    IEnumerator boostGround()
    {
        groundScript.addSpeed(6f);
        yield return new WaitForSeconds(3f);
        groundScript.subtractSpeed(6f);
        isSped = false;
        yield break;
    }

    IEnumerator GroundRising()
    {
        GroundRisingText.enabled = true;
        yield return new WaitForSeconds(3f);
        GroundRisingText.enabled = false;
        yield break;
    }
}
