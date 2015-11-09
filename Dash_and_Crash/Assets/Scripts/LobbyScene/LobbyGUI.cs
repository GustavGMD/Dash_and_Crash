using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;


public class LobbyGUI : NetworkBehaviour {

    public Canvas scoreboard;
    //Form Server to LocalPlayer
    public Text[] boardNames;
    public Text[] boardScores;
    public Image[] boardBackgrounds;
    public string playerName;
    public int score;
    public Color color;
    //From LocalPlayer to Server
    public Slider colorSelector;
    public InputField nameSelector;
    [SyncVar]
    public int _myIndex;

    private myLobbyManager _lobbyManager;

    public override void OnStartServer()
    {

    }

    public override void OnStartClient()
    {
        //base.OnStartClient(); 
        Debug.Log("start client");

        DontDestroyOnLoad(gameObject);
        //colorSelector = GameObject.Find("ColorSelector").GetComponent<Slider>();
        colorSelector.onValueChanged.AddListener(OnSliderValueChanged);
        nameSelector.onEndEdit.AddListener(OnInputFieldValueSet);
        _lobbyManager = GameObject.FindObjectOfType<myLobbyManager>();
        //_myIndex = _lobbyManager.ConnectionToIndex(_lobbyManager.client.connection.connectionId);
        Cmd_SetIndex(_myIndex);

        for (int i = 0; i < 4; i++)
        {
            if (i != _myIndex)
            {
                boardNames[i].gameObject.SetActive(false);
                boardScores[i].gameObject.SetActive(false);
                boardBackgrounds[i].gameObject.SetActive(false);
            }
        }

        //(_myIndex == 0) colorSelector.gameObject.SetActive(false);

        if (hasAuthority) Debug.Log("has authority");
        else Debug.Log("does not have authority");
        //colorSelector.gameObject.SetActive(false);

        SetLocalInput(false);
        StartCoroutine(ActivateLocalInput());

        //if(_lobbyManager.ConnectionToIndex(_lobbyManager.client.connection.connectionId) == _myIndex)
        //{
        //    colorSelector.gameObject.SetActive(true);
        //}
        LobbyGUI[] __temp = GameObject.FindObjectsOfType<LobbyGUI>();
        for (int i = 0; i < __temp.Length; i++)
        {
            if(__temp[i] != this)
            {
                __temp[i].UpdateForNewClient();
            }
        }

        color = Color.red;
        UpdateScoreBoard(); 
    }

    IEnumerator ActivateLocalInput()
    {
        yield return new WaitForSeconds(1);
        if (hasAuthority)
        {
            SetLocalInput(true);
        }

    }
    
    [Command]
    public void Cmd_SetIndex(int p_index)
    {
        _myIndex = p_index;
        Rpc_SetIndex(p_index);
    }

    [Command]
    public void Cmd_SetColor(Color p_color)
    {
        //Debug.Log(_lobbyManager.ConnectionToIndex(p_connectionID));
        color = p_color;
        Rpc_SetColor(p_color);
    }
    [Command]
    public void Cmd_SetScore(int p_score)
    {
        //Debug.Log(_lobbyManager.ConnectionToIndex(p_connectionID));
        score = p_score;
        Rpc_SetScore(p_score);
    }
    [Command]
    public void Cmd_SetName(string p_name)
    {
        playerName = p_name;
        Rpc_SetName(p_name);
    }

    [ClientRpc]
    public void Rpc_SetIndex(int p_index)
    {
        _myIndex = p_index;
    }

    [ClientRpc]
    public void Rpc_SetColor(Color p_color)
    {
        color = p_color;
        UpdateScoreBoard();
    }
    [ClientRpc]
    public void Rpc_SetScore(int p_score)
    {
        score = p_score;
        UpdateScoreBoard();
    }
    [ClientRpc]
    public void Rpc_SetName(string p_name)
    {
        playerName = p_name;
        UpdateScoreBoard();
    }
    
    public void UpdateScoreBoard()
    {        
        boardNames[_myIndex].text = playerName;
        boardScores[_myIndex].text = score.ToString();
        boardBackgrounds[_myIndex].color = color;
        boardNames[_myIndex].color = color;
        boardScores[_myIndex].color = color;
    }

    public void UpdateForNewClient()
    {
        Cmd_SetColor(color);
        Cmd_SetName(playerName);
        Cmd_SetScore(score);      
    }

    /**/
    public void OnSliderValueChanged(float p_value)
    {
        int __hue = (int)p_value;
        Color __color = HSVtoRGB(__hue, 1, 1);
        Debug.Log("changed color...");             
        Cmd_SetColor(__color);
    }
    public void OnInputFieldValueSet(string p_string)
    {
        playerName = p_string;
        Cmd_SetName(playerName);
    }
    public void SetLocalInput(bool p_value)
    {
        colorSelector.gameObject.SetActive(p_value);
        nameSelector.gameObject.SetActive(p_value);
    }
    /**/
    public Color HSVtoRGB(int p_h, float p_s, float p_v)
    {
        float __r = 0, __g = 0, __b = 0;
        float __C, __X, __m;

        __C = p_v * p_s;
        __X = __C * (1 - Mathf.Abs(((float)p_h / 60) % 2 - 1));
        __m = p_v - __C;

        if (0 <= p_h && p_h < 60)
        {
            __r = __C;
            __g = __X;
            __b = 0;
        }
        else if (60 <= p_h && p_h < 120)
        {
            __r = __X;
            __g = __C;
            __b = 0;
        }
        else if (120 <= p_h && p_h < 180)
        {
            __r = 0;
            __g = __C;
            __b = __X;
        }
        else if (180 <= p_h && p_h < 240)
        {
            __r = 0;
            __g = __X;
            __b = __C;
        }
        else if (240 <= p_h && p_h < 300)
        {
            __r = __X;
            __g = 0;
            __b = __C;
        }
        else if (300 <= p_h && p_h <= 360)
        {
            __r = __C;
            __g = 0;
            __b = __X;
        }

        //Debug.Log(new Color(__r, __g, __b, __m));
        return new Color(__r + __m, __g + __m, __b + __m);
    }
    /**/

}
