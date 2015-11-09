using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class LobbyPlayerScript : NetworkLobbyPlayer
{
    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        //SendReadyToBeginMessage();
        Debug.Log(readyToBegin);
    }

    IEnumerator SetAsReady()
    {
        yield return new WaitForSeconds(2);
        SendReadyToBeginMessage();
    }
}
