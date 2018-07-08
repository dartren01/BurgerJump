﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class N_PauseMenu : MonoBehaviour {

    public static bool isOn = false;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void leaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
