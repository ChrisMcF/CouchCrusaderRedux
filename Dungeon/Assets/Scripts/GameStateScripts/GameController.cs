using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Player
{
	public GameObject playerObject;
	public BasePlayerClass playerClass;
}

public class GameController : MonoBehaviour
{

	public static GameController gameController;


	public List<Player> players;
	public List<Player> deadPlayers;

	public Player[] allPlayers;

	public enum PlayerClasses
	{
		Unselected,
		Archer,
		Paladin,
		Warrior,
		Mage
	}
	public enum ControllerStyles
	{
		TwinStick,
		Regular
	}

	public PlayerClasses player1;
	public PlayerClasses player2;
	public PlayerClasses player3;
	public PlayerClasses player4;

	//public ControllerStyles player1ControllerStyle;
	//public ControllerStyles player2ControllerStyle;
	//public ControllerStyles player3ControllerStyle;
	//public ControllerStyles player4ControllerStyle;

	public List<Vector3> linePosition;
	public List<Quaternion> lineRotation;
	public List<string> bossList;
	public List<Vector3>NodeArray;
	public string lastBossDefeated;

	public List<Vector3> BossPosition;
	public int bossesDefeated;
	//public GameObject[] playerObjects;

	public bool gamePaused = false;
    
    public float gameTimer;
    public string currentBossName;
    public Text timerText;
    public string playerName;

    //private bool won;
    //private bool lost;

    void Awake ()
	{
		// Ensure that there is only one instance
		if (gameController == null)
        {
#if !UNITY_EDITOR
            GameObject devHacks = GameObject.Find("DevHacks");
            if (devHacks != null)
                Destroy(devHacks);
#endif
            DontDestroyOnLoad(gameObject);
			gameController = this;
		} else {
			Destroy (gameObject);
		}
	}
	void Start ()
	{

	}

	void Update ()
	{
        if (timerText && !gamePaused)
        {
            gameTimer += Time.deltaTime;
            float minutes = Mathf.Floor(gameTimer / 60);
            float seconds = Mathf.Floor(gameTimer % 60);
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

	//This method gets called from the Initialize players script
	public void Initialize ()
	{
		players = new List<Player> ();
		deadPlayers = new List<Player> ();
		// Populate the players List
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
			Player p;
			p.playerObject = g;
			p.playerClass = g.GetComponent<BasePlayerClass> ();
            if (!p.playerObject.name.Contains("Clone"))
            {
                players.Add(p);
            }
			
		}
		allPlayers = players.ToArray ();

        timerText = GameObject.Find("GameTimer").GetComponent<Text>();

        currentBossName = GameObject.Find("Boss_Title").GetComponent<Text>().text;

        gameTimer = 0;
        timerText.text = "00:00";
	}

	// Removes a dead player from the players list
	public void RemovePlayerFromList (GameObject playerToRemove)
	{
		for (int i = players.Count-1; i >= 0; i--)
		{
			Player p = players[i];
			if (p.playerObject == playerToRemove) {
				p.playerObject.GetComponent<CharacterHandler> ().enabled = false;
				players.Remove (p);
				deadPlayers.Add(p);
			}
		}
	}

	// Loads the Victory Screen
	IEnumerator Victory ()
	{
        yield return new WaitForSeconds (3.5f);
		Fade.fadeInstance.StartCoroutine ("FadeToBlack", 1f);
		yield return new WaitForSeconds (1f);
		//if (lost)
		//	yield return null;

		//yield return new WaitForSeconds (2);
		DestroyAllPlayers ();
		Application.LoadLevel ("Win");
	}

	void DestroyAllPlayers ()
	{
		foreach (Player p in allPlayers) {
			Destroy (p.playerObject);
		}
	}

	// Loads the defeat screen
	//This script gets called from the CheckForLossConditionScript (So that it can be removed from the menu scenes but included in the boss fights).
	IEnumerator Defeat ()
    {
        StopCoroutine ("Victory");
		yield return new WaitForSeconds (3.5f);
		Fade.fadeInstance.StartCoroutine ("FadeToBlack", 1f);
		yield return new WaitForSeconds (1f);
		//if (won)
		//	yield return null;
		DestroyAllPlayers ();

		Application.LoadLevel ("Lose");
	}


    public string CurrentBossName
    {
        get
        {
            return currentBossName;
        }

        set
        {
            currentBossName = value;
        }
    }

}
