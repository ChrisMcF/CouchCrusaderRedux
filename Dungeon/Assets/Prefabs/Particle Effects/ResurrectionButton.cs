using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ResurrectionButton : MonoBehaviour {

	public Material wipeMaterial;
	public GameObject yButtonIcon;
	private List<Player> playersInTrigger = new List<Player>(); 
	public bool isPlayerGraveStone = false;
	public PlayerGrave playerGrave;

    [HideInInspector]
    public Material tempMat;
	private MeshRenderer myMR;

	// Use this for initialization
	void Awake ()
    {
		myMR = GetComponentInChildren<MeshRenderer> ();//.material = tempMat;
		myMR.material = new Material (wipeMaterial);
	}

	void OnEnable()
	{
		// tempMat = wipeMaterial;

		tempMat = myMR.material;
		yButtonIcon.SetActive (false);
		tempMat.SetFloat ("_Cutoff", 1);
		
	}
	void Update()
	{
		if (playersInTrigger.Count > 0) {
			IgnoreDeadPlayers();
			ShowButton();
		} else {
			HideButton();
		}
	}
	
	void HideButton()
	{
		yButtonIcon.SetActive(false);
	}
	
	void ShowButton()
	{
		yButtonIcon.SetActive(true);
	}
	
	// Removes dead players from the players in trigger list
	void IgnoreDeadPlayers()
	{

		for (int i = playersInTrigger.Count-1; i >= 0; i--)
		//foreach (Player p in playersInTrigger) 
		{
			if(playersInTrigger[i].playerClass._isDead)
			{
				playersInTrigger.Remove(playersInTrigger[i]);
			}
		}
	}

	// Wipes the ResBar with a value between 1 and 0 (called from the resurection script)
	public void WipeFromValue(float val)
	{
		val = Mathf.Clamp (val, 0.001f, 1f);
        tempMat.SetFloat("_Cutoff", val); 
	}	

	// Add the entering player to the players list
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			BasePlayerClass playerBaseClass =  other.gameObject.GetComponent<BasePlayerClass>();
			Player currentPlayer = new Player();
			currentPlayer.playerClass = playerBaseClass;
			currentPlayer.playerObject = other.gameObject;
			playersInTrigger.Add (currentPlayer);
		}
	}
	
	// Remove the Exiting player from the players list
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			BasePlayerClass playerBaseClass =  other.gameObject.GetComponent<BasePlayerClass>();
			Player currentPlayer = new Player();
			currentPlayer.playerClass = playerBaseClass;
			currentPlayer.playerObject = other.gameObject;
			playersInTrigger.Remove (currentPlayer);
		}
	}



	// Defaults the wipe material back to 1
	void OnDestroy()
	{
        tempMat.SetFloat("_Cutoff", 1); 
	}
}
