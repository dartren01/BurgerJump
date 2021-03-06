﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class N_SpectateCam : NetworkBehaviour {

    //when this script is enabled, disable components, camera locks on to first player in list, or allows for free movement

    private const float Y_Angle_Min = -89f;
    private const float Y_Angle_Max = 89f;

    [Header("SETUP FIELDS")]
    [SerializeField]
    Behaviour[] DisableForSpectate;
    [SerializeField]
    List<Transform> PlayersInGame;
    [SerializeField]
    Camera cam;
    [SerializeField]
    Camera sceneCamera;

    //public Transform camTransform;
    public float cameraSens;

    private float distance = 10f;
    private float currentX = 0f;
    private float currentY = 0f;

    private int playerIndex = 0;

    public Text playerNameText;
    public Text username;

    private void Start()
    {
        //camTransform = transform;
        //playerIndex = 0;
        cameraSens = PlayerPrefs.GetFloat("localSens", 6);
    }

    private void OnEnable()
    {
        if (isLocalPlayer)
        {
            if (isServer)
            {
                RpcDisableComponents();
            }
            else
            {
                CmdDisableComponents();
            }
            getPlayers();
            if (PlayersInGame.Count < 1) return; //go back to lobby
        }
    }


    void Update ()
    {
        if (!isLocalPlayer)
            return;
        if (N_PauseMenu.isOn) return;
        if(PlayersInGame.Count < 1)
        {
            //go back to lobby
            return;
        }
        currentX += Input.GetAxis("Mouse X") * cameraSens;
        currentY += Input.GetAxis("Mouse Y") * cameraSens;

        currentY = Mathf.Clamp(currentY, Y_Angle_Min, Y_Angle_Max);
        if (PlayersInGame[playerIndex].tag == "Spectator" || PlayersInGame[playerIndex]==null)
        {
            getPlayers();
            RotateThroughPlayers();
        }

        //rotate through players
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateThroughPlayers();
        }

    }

    private void LateUpdate()
    {
        if (!isLocalPlayer)
            return;
        if (PlayersInGame.Count < 1)
            return;
        ThirdPersonPos();
        cam.transform.localRotation = Quaternion.identity;
    }
    
    [Command]
    public void CmdDisableComponents()
    {
        RpcDisableComponents();
    }

    [ClientRpc]
    void RpcDisableComponents()
    {
        foreach (Behaviour behaviour in DisableForSpectate)
        {
            behaviour.enabled = false;
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
        username.enabled = false;
    }

    //OnPerson
    public void getPlayers()
    {
        //clear current list of players
        PlayersInGame.Clear();
        GameObject[] playerList = GameObject.FindGameObjectsWithTag("player");
        //find every player in game and add them to the list
        foreach (GameObject player in playerList)
        {
            if(player.GetComponent<MeshRenderer>().enabled)
            {
                PlayersInGame.Add(player.transform);
            }
        }
    }
    //OnPerson
    void RotateThroughPlayers()
    {
        if (playerIndex+1 < PlayersInGame.Count)
            playerIndex++;
        else
            playerIndex = 0;

        playerNameText.text = "Spectating " + PlayersInGame[playerIndex].GetComponent<N_Player>().username;
    }

    void ThirdPersonPos()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = PlayersInGame[playerIndex].position + rotation * dir;
        transform.LookAt(PlayersInGame[playerIndex].position);
    }

    private void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }

}
