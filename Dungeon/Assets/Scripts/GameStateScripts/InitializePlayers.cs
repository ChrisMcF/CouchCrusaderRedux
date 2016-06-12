using UnityEngine;
using System.Collections;

public class InitializePlayers : MonoBehaviour {

	private GameObject[] players;
	private int numberOfPlayers;
	public Transform Player1SpawnPoint;
	public Transform Player2SpawnPoint;
	public Transform Player3SpawnPoint;
	public Transform Player4SpawnPoint;

	public GameObject Archer;
	public GameObject Mage;
	public GameObject Paladin;
	public GameObject Warrior;

	
	public GameObject playerPanel1;
	public GameObject playerPanel2;
	public GameObject playerPanel3;
	public GameObject playerPanel4;

	[HideInInspector]
	public GameObject player1;
	[HideInInspector]
	public GameObject player2;
	[HideInInspector]
	public GameObject player3;
	[HideInInspector]
	public GameObject player4;


    void Awake()
	{
		LoadPlayers ();
		GameController.gameController.Initialize ();
	}

	// Use this for initialization
	void Start () 
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
		CheckNumberOfPlayers ();
		SetupPlayerPositions ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetupPlayerPositions()
	{
        

        foreach (Player pl in GameController.gameController.players) 
		{
            GameObject p = pl.playerObject;
			BasePlayerClass currentBaseClass = p.GetComponent<BasePlayerClass>();
			switch(currentBaseClass.controllerIndex)
			{
			case 1:
				p.transform.position = Player1SpawnPoint.position;
				break;
			case 2:
				p.transform.position = Player2SpawnPoint.position;
				break;
			case 3:
				p.transform.position = Player3SpawnPoint.position;
				break;
			case 4:
				p.transform.position = Player4SpawnPoint.position;
				break;
			}
			//Debug.Log ("Attempting to enable the collider");
			CharacterController currentChar = p.GetComponent<CharacterController>();
			currentChar.enabled = true;
		}
	}

	void LoadPlayers ()
	{
        
	switch (GameController.gameController.player1) 
		{
		case GameController.PlayerClasses.Archer:
			player1 = GameObject.Instantiate(Archer, Player1SpawnPoint.position, Player1SpawnPoint.rotation) as GameObject;	
			player1.name = "Archer";
			break;
		case GameController.PlayerClasses.Mage:
			player1 = GameObject.Instantiate(Mage, Player1SpawnPoint.position, Player1SpawnPoint.rotation) as GameObject;	
			player1.name = "Mage";
			break;
		case GameController.PlayerClasses.Paladin:
			player1 = GameObject.Instantiate(Paladin, Player1SpawnPoint.position, Player1SpawnPoint.rotation) as GameObject;	
			player1.name = "Paladin";
			break;
		case GameController.PlayerClasses.Warrior:
			player1 = GameObject.Instantiate(Warrior, Player1SpawnPoint.position, Player1SpawnPoint.rotation) as GameObject;	
			player1.name = "Warrior";
			break;
		}
		switch (GameController.gameController.player2) 
		{
		case GameController.PlayerClasses.Archer:
			player2 = GameObject.Instantiate(Archer, Player2SpawnPoint.position, Player2SpawnPoint.rotation) as GameObject;	
			player2.name = "Archer";
			break;
		case GameController.PlayerClasses.Mage:
			player2 = GameObject.Instantiate(Mage, Player2SpawnPoint.position, Player2SpawnPoint.rotation) as GameObject;	
			player2.name = "Mage";
			break;
		case GameController.PlayerClasses.Paladin:
			player2 = GameObject.Instantiate(Paladin, Player2SpawnPoint.position, Player2SpawnPoint.rotation) as GameObject;	
			player2.name = "Paladin";
			break;
		case GameController.PlayerClasses.Warrior:
			player2 = GameObject.Instantiate(Warrior, Player2SpawnPoint.position, Player2SpawnPoint.rotation) as GameObject;	
			player2.name = "Warrior";
			break;
		}
		switch (GameController.gameController.player3) 
		{
		case GameController.PlayerClasses.Archer:
			player3 = GameObject.Instantiate(Archer, Player3SpawnPoint.position, Player3SpawnPoint.rotation) as GameObject;	
			player3.name = "Archer";
			break;
		case GameController.PlayerClasses.Mage:
			player3 = GameObject.Instantiate(Mage, Player3SpawnPoint.position, Player3SpawnPoint.rotation) as GameObject;	
			player3.name = "Mage";
			break;
		case GameController.PlayerClasses.Paladin:
			player3 = GameObject.Instantiate(Paladin, Player3SpawnPoint.position, Player3SpawnPoint.rotation) as GameObject;	
			player3.name = "Paladin";
			break;
		case GameController.PlayerClasses.Warrior:
			player3 = GameObject.Instantiate(Warrior, Player3SpawnPoint.position, Player3SpawnPoint.rotation) as GameObject;	
			player3.name = "Warrior";
			break;
		}
		switch (GameController.gameController.player4) 
		{
		case GameController.PlayerClasses.Archer:
			player4 = GameObject.Instantiate(Archer, Player4SpawnPoint.position, Player4SpawnPoint.rotation) as GameObject;	
			player4.name = "Archer";
			break;
		case GameController.PlayerClasses.Mage:
			player4 = GameObject.Instantiate(Mage, Player4SpawnPoint.position, Player4SpawnPoint.rotation) as GameObject;	
			player4.name = "Mage";
			break;
		case GameController.PlayerClasses.Paladin:
			player4 = GameObject.Instantiate(Paladin, Player4SpawnPoint.position, Player4SpawnPoint.rotation) as GameObject;	
			player4.name = "Paladin";
			break;
		case GameController.PlayerClasses.Warrior:
			player4 = GameObject.Instantiate(Warrior, Player4SpawnPoint.position, Player4SpawnPoint.rotation) as GameObject;	
			player4.name = "Warrior";
			break;
        }
        if (player1 != null)
            player1.GetComponent<BasePlayerClass>().controllerIndex = 1;
        else
            playerPanel1.SetActive(false);

        if (player2 != null)
            player2.GetComponent<BasePlayerClass>().controllerIndex = 1;
        else
            playerPanel2.SetActive(false);

        if (player3 != null)
            player3.GetComponent<BasePlayerClass>().controllerIndex = 1;
        else
            playerPanel3.SetActive(false);

        if (player4 != null)
            player4.GetComponent<BasePlayerClass>().controllerIndex = 1;
        else
            playerPanel4.SetActive(false);


        if (player2 != null)
		player2.GetComponent<BasePlayerClass> ().controllerIndex = 2;
		if(player3 != null)
		player3.GetComponent<BasePlayerClass> ().controllerIndex = 3;
		if(player4 != null)
		player4.GetComponent<BasePlayerClass> ().controllerIndex = 4;
	}


	void CheckNumberOfPlayers()
	{
		for (int i =0; i<players.Length; i++) 
		{
			if(players[i] != null)
				numberOfPlayers++;
		}
	}
}
