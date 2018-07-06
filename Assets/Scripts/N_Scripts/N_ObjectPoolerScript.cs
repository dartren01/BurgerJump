﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class N_ObjectPoolerScript : NetworkBehaviour {

    public static N_ObjectPoolerScript current;
    public GameObject pooledObject;
    public int pooledAmount = 20;

    List<GameObject> pooledObjects;

    private void Awake()
    {
        current = this;
    }

    public override void OnStartServer()
    {
        if (isServer)
        {
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < pooledAmount; ++i)
            {
                GameObject obj = Instantiate(pooledObject);
                NetworkServer.Spawn(obj);
                obj.SetActive(true);
                pooledObjects.Add(obj);
            }
        }
    }
    /*
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; ++i)
        {
            GameObject obj = Instantiate(pooledObject);
            NetworkServer.Spawn(obj);
            obj.SetActive(true);
            pooledObjects.Add(obj);
        }
    }

    
    [ClientRpc]
    void RpcSpawnObjects()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; ++i)
        {
            GameObject obj = Instantiate(pooledObject);
            NetworkServer.Spawn(obj);
            obj.SetActive(true);
            pooledObjects.Add(obj);
        }
    }*/
	
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObjects.Count; ++i)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        /*
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject);
            pooledObjects.Add(obj);
            return obj;
        }*/
        return null;
    }
}