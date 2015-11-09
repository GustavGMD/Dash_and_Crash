using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CountDown : NetworkBehaviour {

    public event Action<bool> onGameStart;

    public Text countDisplay;    
    public float timer;
    [SyncVar]
    private float _currentTime = 0;	
	
	// Update is called once per frame
    public override void OnStartClient()
    {
        base.OnStartClient();
        for (int i = 0; i < GameObject.FindObjectsOfType<PlayerMovement>().Length; i++)
        {
            onGameStart += GameObject.FindObjectsOfType<PlayerMovement>()[i].EnableInput;
        }
        
    }

	void Update () {
        if (_currentTime < timer)
        {
            _currentTime += Time.deltaTime;
            if (Int32.Parse(countDisplay.text) != (int)(timer - _currentTime))
            {
                countDisplay.text = ((int)(timer - _currentTime)).ToString();
            }
        }
        else
        {
            if (onGameStart != null) onGameStart(true);
            gameObject.SetActive(false);
        }
	}
}
