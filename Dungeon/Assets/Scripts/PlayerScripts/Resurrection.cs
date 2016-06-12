using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class Resurrection : MonoBehaviour {

	private GameObject resEffect;  
	public float resurrectionTime = 5f;
	//private GameObject _playerResurrecting;
	private Player _playerToResurrect;
	private BasePlayerClass myPlayerClass;
	public XboxCtrlrInput.XboxButton resurrectionButton;
	public bool inResArea = false;
	[HideInInspector]
	public Player targetPlayer;
	private ResurrectionButton resBut;
	private AudioSource resAudio;

	private InitializePlayers initializePlayers;

    public static bool playerResurrecting = false;


	void Start () 
	{
		resEffect = transform.Find ("ResEffect").gameObject as GameObject;
		resAudio = resEffect.GetComponent<AudioSource> ();
		resEffect.SetActive (false);
		resAudio.playOnAwake = true;
		myPlayerClass = GetComponent<BasePlayerClass> ();
		initializePlayers = GameObject.FindGameObjectWithTag ("LevelSpecific").GetComponent<InitializePlayers>();
		if(initializePlayers == null)
		{
			Debug.LogError("Scene specific info tag is not set to LevelSpecific");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void StartResurrection()
	{
        if (!playerResurrecting)
        {
            playerResurrecting = true;
            _playerToResurrect = targetPlayer;
            StartCoroutine("ResurrectPlayer");
        }
    }

	public void StartResurrection(Player playerToResurrect)
	{
        if (!playerResurrecting)
        {
            playerResurrecting = true;
            _playerToResurrect = playerToResurrect;
            StartCoroutine("ResurrectPlayer");
        }
	}
	private IEnumerator ResurrectPlayer()
	{

		BasePlayerClass currentPlayerBaseClass;
		Player currentPlayer = new Player();
		resEffect.SetActive (true);

		float counter = resurrectionTime;
		Debug.Log ("Started Ceck for y button coroutine");
		Debug.Log ("Counter = " + counter);
		while (counter > 0) 
		{
            
			if(!XCI.GetButton(resurrectionButton, myPlayerClass.controllerIndex))
			{
				InterruptResurrection();
				break;
			}
			counter -=Time.deltaTime;
			//Debug.Log(counter);
			//Debug.Log(XCI.GetButton(resurrectionButton, myPlayerClass.controllerIndex));
			resBut.WipeFromValue(counter/resurrectionTime);
			yield return null;
		}
		if (counter < 0) {
			GameController.gameController.deadPlayers.Remove (_playerToResurrect);
			//BasePlayerClass resPlayerBaseClass = _playerToResurrect.playerClass;
		
			int deadPlayerControllerIndex = _playerToResurrect.playerClass.controllerIndex;
			switch (deadPlayerControllerIndex) {
			/////// PLAYER 1
			case 1:
			//Destroy Old player
				Destroy (initializePlayers.player1);
	
			//Spawn Correct Player Class
				if (GameController.gameController.player1 == GameController.PlayerClasses.Archer)
				{
					initializePlayers.player1 = Instantiate (initializePlayers.Archer, transform.position, transform.rotation) as GameObject;
					initializePlayers.player1.name = "Archer";
				}
				else if (GameController.gameController.player1 == GameController.PlayerClasses.Mage)
				{
					initializePlayers.player1 = Instantiate (initializePlayers.Mage, transform.position, transform.rotation) as GameObject;
					initializePlayers.player1.name = "Mage";
				}
				else if (GameController.gameController.player1 == GameController.PlayerClasses.Paladin)
				{
					initializePlayers.player1 = Instantiate (initializePlayers.Paladin, transform.position, transform.rotation) as GameObject;
					initializePlayers.player1.name = "Paladin";
				}
				else if (GameController.gameController.player1 == GameController.PlayerClasses.Warrior)
				{
					initializePlayers.player1 = Instantiate (initializePlayers.Warrior, transform.position, transform.rotation) as GameObject;
					initializePlayers.player1.name = "Warrior";
				}
				//Set Controller Index
				currentPlayerBaseClass = initializePlayers.player1.GetComponent<BasePlayerClass> ();
				currentPlayerBaseClass.controllerIndex = 1;

				
				//Add to players List
				currentPlayer.playerObject = initializePlayers.player1;
				currentPlayer.playerClass = currentPlayerBaseClass;
				GameController.gameController.players.Add (currentPlayer);
				initializePlayers.player1.SendMessage("InitializePlayer", SendMessageOptions.RequireReceiver);
				break;
			/////// PLAYER 2
			case 2:
			//Destroy Old player
				Destroy (initializePlayers.player2);
			
			//Spawn Correct Player Class
				if (GameController.gameController.player2 == GameController.PlayerClasses.Archer)
				{
					initializePlayers.player2 = Instantiate (initializePlayers.Archer, transform.position, transform.rotation) as GameObject;
					initializePlayers.player2.name = "Archer";
				}
				else if (GameController.gameController.player2 == GameController.PlayerClasses.Mage)
				{
					initializePlayers.player2 = Instantiate (initializePlayers.Mage, transform.position, transform.rotation) as GameObject;
					initializePlayers.player2.name = "Mage";
				}
				else if (GameController.gameController.player2 == GameController.PlayerClasses.Paladin)
				{
					initializePlayers.player2 = Instantiate (initializePlayers.Paladin, transform.position, transform.rotation) as GameObject;
					initializePlayers.player2.name = "Paladin";
				}
				else if (GameController.gameController.player2 == GameController.PlayerClasses.Warrior)
				{
					initializePlayers.player2 = Instantiate (initializePlayers.Warrior, transform.position, transform.rotation) as GameObject;
					initializePlayers.player2.name = "Warrior";
				}
			//Set Controller Index
				currentPlayerBaseClass = initializePlayers.player2.GetComponent<BasePlayerClass> ();
				currentPlayerBaseClass.controllerIndex = 2;
			
				//Add to players List
				currentPlayer = new Player ();
				currentPlayer.playerObject = initializePlayers.player2;
				currentPlayer.playerClass = currentPlayerBaseClass;
				GameController.gameController.players.Add (currentPlayer);
				initializePlayers.player2.SendMessage("InitializePlayer", SendMessageOptions.RequireReceiver);

				break;
			/////// PLAYER 3
			case 3:
			//Destroy Old player
				Destroy (initializePlayers.player3);
			
			//Spawn Correct Player Class
				if (GameController.gameController.player3 == GameController.PlayerClasses.Archer)
				{
					initializePlayers.player3 = Instantiate (initializePlayers.Archer, transform.position, transform.rotation) as GameObject;
					initializePlayers.player3.name = "Archer";
				}
				else if (GameController.gameController.player3 == GameController.PlayerClasses.Mage)
				{
					initializePlayers.player3 = Instantiate (initializePlayers.Mage, transform.position, transform.rotation) as GameObject;
					initializePlayers.player3.name = "Mage";
				}
				else if (GameController.gameController.player3 == GameController.PlayerClasses.Paladin)
				{
					initializePlayers.player3 = Instantiate (initializePlayers.Paladin, transform.position, transform.rotation) as GameObject;
					initializePlayers.player3.name = "Paladin";
				}
				else if (GameController.gameController.player3 == GameController.PlayerClasses.Warrior)
				{
					initializePlayers.player3 = Instantiate (initializePlayers.Warrior, transform.position, transform.rotation) as GameObject;
					initializePlayers.player3.name = "Warrior";
				}
				//Set Controller Index
				currentPlayerBaseClass = initializePlayers.player3.GetComponent<BasePlayerClass> ();
				currentPlayerBaseClass.controllerIndex = 3;
			
			//Add to players List
				currentPlayer.playerObject = initializePlayers.player3;
				currentPlayer.playerClass = currentPlayerBaseClass;
				GameController.gameController.players.Add (currentPlayer);
				initializePlayers.player3.SendMessage("InitializePlayer", SendMessageOptions.RequireReceiver);

				break;
			/////// PLAYER 4
			case 4:
			//Destroy Old player
				Destroy (initializePlayers.player4);
			
			//Spawn Correct Player Class
				if (GameController.gameController.player4 == GameController.PlayerClasses.Archer)
				{
					initializePlayers.player4 = Instantiate (initializePlayers.Archer, transform.position, transform.rotation) as GameObject;
					initializePlayers.player4.name = "Archer";
				}
				else if (GameController.gameController.player4 == GameController.PlayerClasses.Mage)
				{
					initializePlayers.player4 = Instantiate (initializePlayers.Mage, transform.position, transform.rotation) as GameObject;
					initializePlayers.player4.name = "Mage";
				}
				else if (GameController.gameController.player4 == GameController.PlayerClasses.Paladin)
				{
					initializePlayers.player4 = Instantiate (initializePlayers.Paladin, transform.position, transform.rotation) as GameObject;
					initializePlayers.player4.name = "Paladin";
				}
				else if (GameController.gameController.player4 == GameController.PlayerClasses.Warrior)
				{
					initializePlayers.player4 = Instantiate (initializePlayers.Warrior, transform.position, transform.rotation) as GameObject;
					initializePlayers.player4.name = "Warrior";
				}
			//Set Controller Index
				currentPlayerBaseClass = initializePlayers.player4.GetComponent<BasePlayerClass> ();
				currentPlayerBaseClass.controllerIndex = 4;
			
			//Add to players List
				currentPlayer.playerObject = initializePlayers.player4;
				currentPlayer.playerClass = currentPlayerBaseClass;
				GameController.gameController.players.Add (currentPlayer);
				initializePlayers.player4.SendMessage("InitializePlayer", SendMessageOptions.RequireReceiver);

				break;
			default:
				Debug.LogError ("Unknown Controller Index");
				break;
			}
			inResArea = false;
			if(resBut.isPlayerGraveStone)
			{
				resBut.playerGrave.StartCoroutine("DestroyGrave");
				resBut.gameObject.SetActive(false);
            }
        }

        playerResurrecting = false;
        resEffect.SetActive (false);

    }
	public void InterruptResurrection()
	{
		//Debug.Log ("Rez is interrupted");
		StopCoroutine ("ResurrectPlayer");
		if(resBut !=null)
			resBut.tempMat.SetFloat("_Cutoff", 1); 
		resEffect.SetActive (false);
        playerResurrecting = false;
    }


    
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ResurrectionArea" && !playerResurrecting) 
		{
		
			resBut = other.GetComponent<ResurrectionButton>();
			if(!resBut.isPlayerGraveStone)
			{
				targetPlayer.playerObject = other.transform.parent.gameObject;
				targetPlayer.playerClass = targetPlayer.playerObject.GetComponent<BasePlayerClass>();
			} else {
				if(resBut.playerGrave.playerObject!= null)
				{
				targetPlayer.playerObject = resBut.playerGrave.playerObject;
				targetPlayer.playerClass = targetPlayer.playerObject.GetComponent<BasePlayerClass>();
				}
			}
			inResArea = true;	
		}
	}
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "ResurrectionArea")
        {
            if (inResArea && XCI.GetButtonDown(resurrectionButton, myPlayerClass.controllerIndex) && !myPlayerClass._isDead && !playerResurrecting)
            {
                StartResurrection();
            }
        }
    }

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "ResurrectionArea") 
		{
			InterruptResurrection();
			Debug.Log("Exited Res Area");
			inResArea = false;	
		}
	}
}
