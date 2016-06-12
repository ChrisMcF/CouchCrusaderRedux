using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class CharacterSelectionManager : MonoBehaviour {
	public static CharacterSelectionManager manager;

	public GameObject missingControllers;

	public string nextScene = "Scylla";
	public GameObject mageActive;
	public GameObject mageInactive;
	public GameObject archerActive;
	public GameObject archerInactive;
	public GameObject paladinActive;
	public GameObject paladinInactive;
	public GameObject warriorActive;
	public GameObject warriorInactive;
	
	public float stickCooldown = 0.2f;
	
	public AudioClip flick, click, back;

	private int knownPlayers = 0;
	public CharacterSelectionMenu[] characterSelectionMenus = new CharacterSelectionMenu[0];

	void Awake()
	{
		missingControllers.SetActive (true);
		manager = this;
	}
	void Start () 
	{
		// Dissable all menus
		foreach (CharacterSelectionMenu menu in characterSelectionMenus) {
			menu.gameObject.SetActive(false);
		}

		//Error if there are no menu's assigned in the inspector
		if (characterSelectionMenus.Length == 0) 
		{
			Debug.LogError("player select panels are not setup, ad the pannels to the Character Selection manager...");
		}
	}
	void Update () 
	{
		// Reveal a new menu, when required
		if (XCI.GetNumPluggedCtrlrs () > knownPlayers) 
		{
			RevealCharacterSelectionMenu();
		}
		if (XCI.GetNumPluggedCtrlrs () > 0) {
			missingControllers.SetActive (false);
		} else {
			foreach (CharacterSelectionMenu c in characterSelectionMenus)
			{
				c.gameObject.SetActive (false);
			}
			knownPlayers = 0;
			missingControllers.SetActive (true);
		}
	}

	// This method reveals and initalizes the next character selection window when the number of plugged in controllers increases.
	void RevealCharacterSelectionMenu()
	{
		if (knownPlayers < characterSelectionMenus.Length) 
		{
			knownPlayers++;
			characterSelectionMenus [knownPlayers-1].gameObject.SetActive (true);
			characterSelectionMenus [knownPlayers-1].controllerIndex = knownPlayers;
		}
	}
	public void RevealAllCurrentCharacters ()
	{
		for (int i = 0; i< knownPlayers; i++) 
		{
			Debug.Log("Known Players is: " + knownPlayers + ", Character Selection Menus Length is: " + characterSelectionMenus.Length);
			characterSelectionMenus [i].RevealCurrentCharacter ();
		}
	}

}
