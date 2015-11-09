using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class PowerUpManager : NetworkBehaviour {

    public List<PowerUp> availablePool;
    public List<PowerUp> activePool;
    public PowerUp[] spawnedLocation;       //The object in a secific position represents its location index in the spawnPoints array
    public List<Transform> spawnPoints;

    [SyncVar]
    public int spawnedCount = 0;
    [SyncVar]
    public int spawnLimit;

    public override void OnStartServer()
    {
 	    base.OnStartServer();
    
        spawnedLocation = new PowerUp[spawnPoints.Count];
        spawnLimit = spawnPoints.Count;

        StartCoroutine(SpawnRoutine());
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        spawnedLocation = new PowerUp[spawnPoints.Count];
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < spawnLimit; i++)
        {
            Spawn();
        }
    }

    public bool Spawn()
    {
        //4 operações + atualizar o count
        if (availablePool.Count > 0 && spawnedCount < spawnLimit)
        {
            int __index = GetSpawnPoint();
            availablePool[0].transform.position = spawnPoints[__index].position;
            spawnedLocation[__index] = availablePool[0];
            activePool.Add(availablePool[0]);
            availablePool.RemoveAt(0);
            spawnedCount++;

            Rpc_Spawn(__index);
            return true;
        }
        else
        {
            return false;
        }
    }

    //4 operações + atualizar o count
    public void PowerUpCaught(PowerUp p_pu)
    {
        //remove the PowerUp from a spawnedlocation
        //and position it far from the viewport
        int __num = -1;
        for (int i = 0; i < spawnedLocation.Length; i++)
        {
            if (spawnedLocation[i] == p_pu)
            {
                spawnedLocation[i] = null;
                p_pu.transform.position = new Vector3(100, 100, 0);
                __num = i;
                break;
            }
        }
        //remove the PowerUp from the active Pool
        activePool.Remove(p_pu);
        //put it back to the available pool
        availablePool.Add(p_pu);
        //update the spawnCount variable
        spawnedCount--;

        Debug.Log(gameObject.ToString() + " " + __num);
        Rpc_Remove(__num);
    }

    public int GetSpawnPoint(){
        int i;
        for (i = 0; i < spawnPoints.Count; i++)
        {
            if (spawnedLocation[i] == null) break;
        }

        return i;
    }

    [ClientRpc]
    public void Rpc_Remove(int p_index)
    {
        if (!isServer)
        {
            spawnedLocation[p_index].transform.position = new Vector3(100, 100, 0);
            activePool.Remove(spawnedLocation[p_index]);
            availablePool.Add(spawnedLocation[p_index]);
            spawnedLocation[p_index] = null;
            //spawnedCount--;
        }
    }

    [ClientRpc]
    public void Rpc_Spawn(int p_index)
    {
        if (!isServer)
        {
            availablePool[0].transform.position = spawnPoints[p_index].position;
            spawnedLocation[p_index] = availablePool[0];
            activePool.Add(availablePool[0]);
            availablePool.RemoveAt(0);
        }
        //spawnedCount++;
    }
}
