using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XboxCtrlrInput;


public class PauseStef : MonoBehaviour 
{
	private GameObject HUD;
	public static PauseStef instance;

	public GameObject pauseMenu;
	public GameObject player1Panel, player2Panel, player3Panel, player4Panel;
	public int numberOfPlayers;

	public AudioClip flick, click, back;
	public float stickCooldown = 0.2f;

	public int knownPlayers = 0;

	public PausedChar[] pausedChars = new PausedChar[0];

	private GameObject[] playerObjects;
	private CharacterHandler[] playerCharacterHandlers;

	public GameObject instructions;
	public GameObject confirmQuit;
	public bool iWantToQuit = false;
	public string mainMenuToLoad;
	public float cooldown = 1.0f;
	//private BasePlayerClass[] basePlayerClasses;

	void Awake()
	{
		instance = this;
		//GameController.gameController.StopAllRumbles();
	}

	//private bool isGamePaused = false;
	//public GameObject style1;
	//public GameObject style2;
	//public CharacterHandler[] charHandlers;

	// Use this for initialization
	void Start ()
	{
		HUD = GameObject.Find ("HUD") as GameObject;
		playerObjects = GameObject.FindGameObjectsWithTag ("Player");
		playerCharacterHandlers = new CharacterHandler[playerObjects.Length];
		//basePlayerClasses = new BasePlayerClass[playerObjects.Length];
		for(int i = 0; i<playerObjects.Length; i++)
		{
			playerCharacterHandlers[i] = playerObjects[i].GetComponent<CharacterHandler>();
			//basePlayerClasses[i] = playerObjects[i].GetComponent<BasePlayerClass>();
		}

		foreach (PausedChar p in pausedChars) 
		{
			p.gameObject.SetActive(false);
		}

		numberOfPlayers = CheckPlayersInGame ();


	}

    
	// Update is called once per frame
	void LateUpdate () 
	{
        for (int i = 0; i < GameController.gameController.players.Count; i++)
        {
            if (XCI.GetButtonDown(XboxButton.Start, i + 1) && !GameController.gameController.gamePaused)
            {
                foreach (CharacterHandler c in playerCharacterHandlers)
                {
                    c.enabled = false;
                }
                HUD.SetActive(false);
                Time.timeScale = 0f;
                GameController.gameController.gamePaused = true;
                pauseMenu.SetActive(true);
                RevealPlayerPauseMenu();
            }
            else if (XCI.GetButtonDown(XboxButton.Start, i + 1) && GameController.gameController.gamePaused)
            {
                foreach (CharacterHandler c in playerCharacterHandlers)
                {
                    c.enabled = true;
                }
                HUD.SetActive(true);
                Time.timeScale = 1f;
                GameController.gameController.gamePaused = false;
                pauseMenu.SetActive(false);
            }
            //Press Y to show controller instructions
            if (XCI.GetButton(XboxButton.Y, i + 1) && GameController.gameController.gamePaused)
            {
                instructions.SetActive(true);
            }

            // Release Y to hide instructions again
            if (XCI.GetButtonUp(XboxButton.Y, i + 1) && GameController.gameController.gamePaused)
            {
                instructions.SetActive(false);
            }

            // Press X to quit
            if (((XCI.GetButtonDown(XboxButton.X, i + 1)) && GameController.gameController.gamePaused && !iWantToQuit))
            {
                confirmQuit.SetActive(true);
                iWantToQuit = true;
            }
            else if (XCI.GetButtonDown(XboxButton.X, i + 1) && GameController.gameController.gamePaused && iWantToQuit)
            {
                confirmQuit.SetActive(false);
                iWantToQuit = false;
            }

            if (((XCI.GetButtonUp(XboxButton.A, i + 1)) && GameController.gameController.gamePaused && iWantToQuit))
            {
                Application.LoadLevel(mainMenuToLoad);
            }
        }
        

	}

	

	void RevealPlayerPauseMenu()
	{
		for (int i = 0; i < XCI.GetNumPluggedCtrlrs(); i++) 
		{
			Debug.Log (pausedChars[i].gameObject.name);
			pausedChars[i].gameObject.SetActive(true);
			pausedChars[i].controllerIndex = i+1;
		}
	}

	int CheckPlayersInGame()
	{
		return XCI.GetNumPluggedCtrlrs ();
	}

}
