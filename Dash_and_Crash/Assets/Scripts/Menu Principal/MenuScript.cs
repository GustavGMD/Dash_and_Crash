using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	//Rect button1;
	//Rect button2;
	//Rect button3;
	//Rect button4;
	//Rect button5;
	//Rect button6;

	// Use this for initialization
	void Start () {
		//button1 = new Rect (Screen.width / 2 - 50, Screen.height / 4 - 10, 100, 20);
		//button2 = new Rect (Screen.width / 2 - 50, Screen.height / 4 + 20, 100, 20);
		//button3 = new Rect (Screen.width / 2 - 50, Screen.height / 4 + 50, 100, 20);
		//button4 = new Rect (Screen.width / 2 - 50, Screen.height / 4 + 80, 100, 20);
		//button5 = new Rect (Screen.width / 2 - 50, Screen.height / 4 + 110, 100, 20);
		//button6 = new Rect (Screen.width / 2 - 50, Screen.height / 4 + 140, 100, 20);
	}

    /**
	void OnGUI() {
		if (GUI.Button (button1, "Single Player")) {
			print ("You clicked the button!");
		}
		if (GUI.Button (button2, "Multiplayer")) {
			Application.LoadLevel(1);
		}
		if (GUI.Button (button3, "Multiplayer - PC")) {
			Application.LoadLevel(1);
		}
        /**
		if (GUI.Button (button4, "Customize Unit")) {
			print ("You clicked the button!");
		}
		if (GUI.Button (button5, "Edit Level")) {
			print ("You clicked the button!");
		}
        /**
		if (GUI.Button (button6, "Exit Game")) {
			Application.Quit();
		}
		
	}
    /**/

    public void OnStartGame()
    {
        Application.LoadLevel(1);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
