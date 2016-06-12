using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XboxCtrlrInput;

[RequireComponent(typeof(AudioSource))]

public class CharacterSelectionMenu : MonoBehaviour
{
	private int currentSelection = 0;

	private GameObject mageActive;
	private GameObject mageInactive;
	private GameObject archerActive;
	private GameObject archerInactive;
	private GameObject paladinActive;
	private GameObject paladinInactive;
	private GameObject warriorActive;
	private GameObject warriorInactive;
	private Vector3 startingScale = new Vector3 (12, 12, 12);
	private Vector3 growthOffset = new Vector3 (13, 13, 13);
	public float growthTime = 0.3f;

	public Text characterName;

	private static bool loading;

	//public GameObject marker;

	//private Ray ray;
	public static int numberOfReadyPlayers;// = 0;
	private Vector3 hitPoint;

	private Vector3 startPos;
	public GameObject selectedPlayer;

	private RectTransform rectTransform;

	private Vector3 offset;


	private AudioSource audioSource;

	public Vector3 characterLocation;
	public Vector3 characterRotation;
	public List<GameObject> selectableClasses;// = new List<GameObject>();
	public GameObject[] unselectableClasses;
	private bool coolingDown = false;
	
	public int controllerIndex;
	public enum CharacterSelectionMenuStates
	{
		PressAToJoin,
		CharacterSelection,
		CharacterSelected,
		Ready,
		Disconnected}
	;
	[HideInInspector]
	public CharacterSelectionMenuStates
		currentState;
	private Camera cam;
	private GameObject pressAToJoin, disconnected, ready, leftRightArrows, characterDescription;
	private GameObject[] menuObjects;
	private GameObject[] characterObjects;

	public static List<GameObject> takenPlayerClasses; //= new List<GameObject> ();	
	// Use this for initialization
	void Start ()
	{
		loading = false;
		numberOfReadyPlayers = 0;
		selectableClasses = new List<GameObject> ();
		takenPlayerClasses = new List<GameObject> ();
		InitializeAndCheckForNullRefs ();
	}
	void OnEnable ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!XCI.IsPluggedIn (controllerIndex)) {
			currentState = CharacterSelectionMenuStates.Disconnected;
		}
		switch (currentState) {
		// PRESS A TO JOIN
		case CharacterSelectionMenuStates.PressAToJoin:
			HideAllMenuItemsExcept (pressAToJoin);
			//A button
			characterName.text = "";
			if (XCI.GetButtonDown(XboxButton.A, controllerIndex)) {
				RevealCurrentCharacter ();
				audioSource.PlayOneShot (CharacterSelectionManager.manager.click);
				currentState = CharacterSelectionMenuStates.CharacterSelection;
			}
			//B button
			if (XCI.GetButtonDown(XboxButton.B, controllerIndex)) {
				audioSource.PlayOneShot (CharacterSelectionManager.manager.back);
				Application.LoadLevel ("MainMenu");
			}
			break;

		//CHARACTER SELECTION
		case CharacterSelectionMenuStates.CharacterSelection:
			HideAllMenuItemsExcept (leftRightArrows);
			CheckIfCurrentPlayerIsTaken ();

			if (!coolingDown && XCI.GetAxis (XboxAxis.LeftStickX, controllerIndex) < -0.9) {
				selectedPlayer.transform.rotation = Quaternion.Euler (0, 180, 0);
				StartCoroutine ("SelectNext", -1);
			} else if (!coolingDown && XCI.GetAxis (XboxAxis.LeftStickX, controllerIndex) > 0.9) {
				selectedPlayer.transform.rotation = Quaternion.Euler (0, 180, 0);
				StartCoroutine ("SelectNext", 1);
			}

			if (XCI.GetAxis (XboxAxis.RightStickX, controllerIndex) < -0.2) {
				selectedPlayer.transform.Rotate (-Vector3.up * 200 * XCI.GetAxis (XboxAxis.RightStickX, controllerIndex) * Time.deltaTime);
			} else if (XCI.GetAxis (XboxAxis.RightStickX, controllerIndex) > 0.2) {
				selectedPlayer.transform.Rotate (-Vector3.up * 200 * XCI.GetAxis (XboxAxis.RightStickX, controllerIndex) * Time.deltaTime);
			}

			//A button
			if (XCI.GetButtonDown(XboxButton.A, controllerIndex) && selectableClasses.Contains (selectedPlayer) && !CheckTakenPlayerClassesFor (selectedPlayer)) {
				StartCoroutine ("GrowObject", selectedPlayer);
				takenPlayerClasses.Add (selectedPlayer);
				audioSource.PlayOneShot (CharacterSelectionManager.manager.click, 0.8f);
				//currentState = CharacterSelectionMenuStates.CharacterSelected;

				StoreCharacterSelection ();
				numberOfReadyPlayers++;
				currentState = CharacterSelectionMenuStates.Ready;
			}
			//B button
			if (XCI.GetButtonDown(XboxButton.B, controllerIndex)) {
				HideAllCharactersExcept (this.gameObject);	//Hide all characters
				audioSource.PlayOneShot (CharacterSelectionManager.manager.back, 0.6f);
				currentState = CharacterSelectionMenuStates.PressAToJoin;
			}
			break;

		//CHARACTER SELECTED
//		case CharacterSelectionMenuStates.CharacterSelected:
//			HideAllMenuItemsExcept (characterDescription);
//			//A button
//			if (XCI.GetButtonUp (XboxButton.A, controllerIndex)) {
//				StoreCharacterSelection ();
//				numberOfReadyPlayers++;
//				audioSource.PlayOneShot (CharacterSelectionManager.manager.click, 0.8f);
//				currentState = CharacterSelectionMenuStates.Ready;
//			}
//			//B button
//			if (XCI.GetButtonUp (XboxButton.B, controllerIndex)) {
//				StopCoroutine ("GrowObject");
//				StartCoroutine ("ShrinkObject", selectedPlayer);
//				takenPlayerClasses.Remove (selectedPlayer);
//				audioSource.PlayOneShot (CharacterSelectionManager.manager.back, 0.6f);
//				currentState = CharacterSelectionMenuStates.CharacterSelection;
//			}
//			//break;
//			
//			//A button
//			if (XCI.GetButtonUp (XboxButton.A, controllerIndex) && selectableClasses.Contains (selectedPlayer) && !CheckTakenPlayerClassesFor (selectedPlayer)) {
//				StartCoroutine ("GrowObject", selectedPlayer);
//				takenPlayerClasses.Add (selectedPlayer);
//				audioSource.PlayOneShot (CharacterSelectionManager.manager.click, 0.8f);
//				currentState = CharacterSelectionMenuStates.CharacterSelected;
//			}
//			//B button
//			if (XCI.GetButtonUp (XboxButton.B, controllerIndex)) {
//				HideAllCharactersExcept (this.gameObject);	//Hide all characters
//				audioSource.PlayOneShot (CharacterSelectionManager.manager.back, 0.6f);
//				currentState = CharacterSelectionMenuStates.PressAToJoin;
//			}
//			break;

		//READY
		case CharacterSelectionMenuStates.Ready:
			HideAllMenuItemsExcept (ready);
			CheckIfAllReady ();
			if (XCI.GetButtonDown(XboxButton.B, controllerIndex)) {
				numberOfReadyPlayers--;
				audioSource.PlayOneShot (CharacterSelectionManager.manager.back, 0.6f);
				//currentState = CharacterSelectionMenuStates.CharacterSelected;

				StopCoroutine ("GrowObject");
				StartCoroutine ("ShrinkObject", selectedPlayer);
				takenPlayerClasses.Remove (selectedPlayer);
				HideAllCharactersExcept (this.gameObject);	//Hide all characters
				RevealCurrentCharacter ();
				CharacterSelectionManager.manager.RevealAllCurrentCharacters();
				currentState = CharacterSelectionMenuStates.CharacterSelection;
			}
			break;

		//Disconnected
		case CharacterSelectionMenuStates.Disconnected:
			HideAllMenuItemsExcept (disconnected);
			break;
		}
	}
	void StoreCharacterSelection ()
	{
		switch (controllerIndex) {
		case 1:
			if (selectedPlayer.name == "Archer Placeholder Active(Clone)")
				GameController.gameController.player1 = GameController.PlayerClasses.Archer;
			if (selectedPlayer.name == "Mage Placeholder Active(Clone)")
				GameController.gameController.player1 = GameController.PlayerClasses.Mage;
			if (selectedPlayer.name == "Paladin Placeholder Active(Clone)")
				GameController.gameController.player1 = GameController.PlayerClasses.Paladin;
			if (selectedPlayer.name == "Warrior Placeholder Active(Clone)")
				GameController.gameController.player1 = GameController.PlayerClasses.Warrior;
			break;
		case 2:
			if (selectedPlayer.name == "Archer Placeholder Active(Clone)")
				GameController.gameController.player2 = GameController.PlayerClasses.Archer;
			if (selectedPlayer.name == "Mage Placeholder Active(Clone)")
				GameController.gameController.player2 = GameController.PlayerClasses.Mage;
			if (selectedPlayer.name == "Paladin Placeholder Active(Clone)")
				GameController.gameController.player2 = GameController.PlayerClasses.Paladin;
			if (selectedPlayer.name == "Warrior Placeholder Active(Clone)")
				GameController.gameController.player2 = GameController.PlayerClasses.Warrior;
			break;
		case 3:
			if (selectedPlayer.name == "Archer Placeholder Active(Clone)")
				GameController.gameController.player3 = GameController.PlayerClasses.Archer;
			if (selectedPlayer.name == "Mage Placeholder Active(Clone)")
				GameController.gameController.player3 = GameController.PlayerClasses.Mage;
			if (selectedPlayer.name == "Paladin Placeholder Active(Clone)")
				GameController.gameController.player3 = GameController.PlayerClasses.Paladin;
			if (selectedPlayer.name == "Warrior Placeholder Active(Clone)")
				GameController.gameController.player3 = GameController.PlayerClasses.Warrior;
			break;
		case 4:
			if (selectedPlayer.name == "Archer Placeholder Active(Clone)")
				GameController.gameController.player4 = GameController.PlayerClasses.Archer;
			if (selectedPlayer.name == "Mage Placeholder Active(Clone)")
				GameController.gameController.player4 = GameController.PlayerClasses.Mage;
			if (selectedPlayer.name == "Paladin Placeholder Active(Clone)")
				GameController.gameController.player4 = GameController.PlayerClasses.Paladin;
			if (selectedPlayer.name == "Warrior Placeholder Active(Clone)")
				GameController.gameController.player4 = GameController.PlayerClasses.Warrior;
			break;
		}
	}
	void HideAllMenuItemsExcept (GameObject currentGameObject)
	{
		foreach (GameObject g in menuObjects) {
			if (g == currentGameObject)
				g.SetActive (true);
			else
				g.SetActive (false);
		}
	}

	void HideAllCharactersExcept (GameObject character)
	{
		foreach (GameObject g in characterObjects) {
			if (g == character)
				g.SetActive (true);
			else
				g.SetActive (false);
		}
	}

	void InitializeAndCheckForNullRefs ()
	{
		audioSource = GetComponent<AudioSource> ();
		rectTransform = GetComponent<RectTransform> ();
		if (rectTransform == null) {
			Debug.LogError ("Character Selection Menu " + controllerIndex + " could not find the RectTransform");
		}
        // Find the camera through it's tag
        cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		if (cam == null) {
			Debug.LogError ("Character Selection Menu " + controllerIndex + " could not find the Main Camera, set the camera's tag to \"MainCamera\"");
		}
		// Find the "Press A To Join" GameObject in children
		pressAToJoin = transform.Find ("Press A To Join").gameObject;
		if (pressAToJoin == null) {
			Debug.LogError ("Character Selection Menu " + controllerIndex + " could not find the \"Press A To Join\" GameObject");
		}
		// Find the Disconnected GameObject in childeren
		disconnected = transform.Find ("Disconnected").gameObject;
		if (disconnected == null) {
			Debug.LogError ("Character Selection Menu " + controllerIndex + " could not find the \"Disconnected\" GameObject");
		}
		// Find the Ready GameObject in children
		ready = transform.Find ("Ready").gameObject;
		if (ready == null) {
			Debug.LogError ("Character Selection Menu " + controllerIndex + " could not find the \"Ready\" GameObject");
		}
		// Find The Left And Right Arrows Game Object
		leftRightArrows = transform.Find ("<>").gameObject;
		if (leftRightArrows == null) {
			Debug.LogError ("Character Selection Menu " + controllerIndex + " could not find the \"<>\" GameObject");
		}
		// Find the CharacterDescription GameObject; 
		characterDescription = transform.Find ("Character Description").gameObject;
		if (characterDescription == null) {
			Debug.LogError ("Character Selection Menu " + controllerIndex + " could not find the \"Character Description\" GameObject");
		}
		offset = Vector3.Normalize (characterLocation - cam.transform.position) * 8f;
		//GetTheCharacterLocation
		characterLocation = rectTransform.position - offset;
		//Debug.Log (tempCharacterLocation);
		//GameObject.Instantiate (marker, characterLocation, Quaternion.identity); 
		//StartCoroutine("DoRayCast");

		//Debug.Log (CharacterSelectionManager.manager.mageActive.name);
		//Debug.Log (Quaternion.Euler (characterRotation));
		//Setup Active/Inactive Characters.
		mageActive = (GameObject.Instantiate (CharacterSelectionManager.manager.mageActive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject);
		mageInactive = (GameObject.Instantiate (CharacterSelectionManager.manager.mageInactive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject);
		archerActive = GameObject.Instantiate (CharacterSelectionManager.manager.archerActive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject;
		archerInactive = GameObject.Instantiate (CharacterSelectionManager.manager.archerInactive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject;
		paladinActive = GameObject.Instantiate (CharacterSelectionManager.manager.paladinActive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject;
		paladinInactive = GameObject.Instantiate (CharacterSelectionManager.manager.paladinInactive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject;
		warriorActive = GameObject.Instantiate (CharacterSelectionManager.manager.warriorActive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject;
		warriorInactive = GameObject.Instantiate (CharacterSelectionManager.manager.warriorInactive, characterLocation, Quaternion.Euler (characterRotation)) as GameObject;

		//Setup Player Classes Array
		selectableClasses.Add (mageActive);
		selectableClasses.Add (archerActive);
		selectableClasses.Add (paladinActive);
		selectableClasses.Add (warriorActive);

		unselectableClasses = new GameObject[]{mageInactive, archerInactive, paladinInactive, warriorInactive};



		characterObjects = new GameObject[] {
			mageActive,
			mageInactive,
			archerActive,
			archerInactive,
			paladinActive,
			paladinInactive,
			warriorActive,
			warriorInactive
		}; 

		menuObjects = new GameObject[] {pressAToJoin, disconnected, ready, leftRightArrows, characterDescription}; 

		//Hide all characters
		HideAllCharactersExcept (this.gameObject);

		for (int i = 0; i < unselectableClasses.Length; i++) {
			//unselectableClasses[i].transform.position = hitPoint;
			unselectableClasses [i].transform.localScale = startingScale;
			//selectableClasses[i].transform.position = hitPoint;
			selectableClasses [i].transform.localScale = startingScale;
		}

	}

	IEnumerator SelectNext (int direction)
	{
		coolingDown = true;
		NextCharacter (direction);
		yield return new WaitForSeconds (CharacterSelectionManager.manager.stickCooldown);
		coolingDown = false;
	}
	//Pass in a +1 or -1 value to scroll through the list of active/inactive players depending on the joystic direction
	void NextCharacter (int characterIndex)
	{
		currentSelection += characterIndex;
		if (currentSelection > 3)
			currentSelection -= 4;//Set it back to zero etc
		if (currentSelection < 0)
			currentSelection += 4;//Set it back to 3 etc
		audioSource.PlayOneShot (CharacterSelectionManager.manager.flick, 0.4f);
		RevealCurrentCharacter ();
	}
	// This method checks the list of selectable classes to see if the provided GameObject is in there
	bool CheckSelectablePlayerClassesFor (GameObject player)
	{
		bool itemFound = false;
		foreach (GameObject g in selectableClasses) {
			if (player.name == g.name)
				itemFound = true;
		}
		return itemFound;
	}

	// This method checks the list of taken classes for a gameobject that matches the name of the provided object.
	bool CheckTakenPlayerClassesFor (GameObject player)
	{
		bool itemFound = false;
		foreach (GameObject g in takenPlayerClasses) {
			if (player.name != null) {
				if (player.name == g.name)

					itemFound = true;
			}
		}
		return itemFound;
	}

	void CheckIfCurrentPlayerIsTaken ()
	{

		if (CheckSelectablePlayerClassesFor (selectedPlayer) && CheckTakenPlayerClassesFor (selectedPlayer)) {
			Debug.Log ("Current player class was taken by another player");
			selectedPlayer = unselectableClasses [currentSelection];
			HideAllCharactersExcept (selectedPlayer);
		}
	}

	public void RevealCurrentCharacter ()
	{
		RevealCharacterName ();
		if (CheckTakenPlayerClassesFor (selectableClasses [currentSelection])) {
			selectedPlayer = unselectableClasses [currentSelection];
		} else {
			selectedPlayer = selectableClasses [currentSelection];
		}
		HideAllCharactersExcept (selectedPlayer);
	}

	IEnumerator GrowObject (GameObject player)
	{
		startPos = player.transform.localScale;
		float counter = growthTime;
		float timePassed = 0;
		while (counter>=0) {
			timePassed += Time.deltaTime;
			float lerpVal = timePassed / growthTime;
			player.transform.localScale = Vector3.Lerp (startPos, growthOffset, lerpVal);
			player.transform.rotation = Quaternion.Euler (Vector3.Lerp (player.transform.rotation.eulerAngles, new Vector3 (0, 180, 0), lerpVal));
			counter -= Time.deltaTime;
			yield return null;
		}
		Debug.Log ("Growth has finished");
	}

	IEnumerator ShrinkObject (GameObject player)
	{
		//startPos = player.transform.position;
		Vector3 shrinkFrom = player.transform.localScale;
		float counter = growthTime;
		float timePassed = 0;
		while (counter>=0) {
			timePassed += Time.deltaTime;
			float lerpVal = timePassed / growthTime;
			player.transform.localScale = Vector3.Lerp (shrinkFrom, startPos, lerpVal);
			counter -= Time.deltaTime;
			yield return null;
		}
		Debug.Log ("Shrink has finished");
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.magenta;
		//Gizmos.DrawLine (camera.transform.position, hitPoint);
	}

	void CheckIfAllReady ()
	{
		//Debug.Log ("Number of Controllers = " + XCI.GetNumPluggedCtrlrs () + ", Number of ready Players = " + numberOfReadyPlayers);
		if (numberOfReadyPlayers == XCI.GetNumPluggedCtrlrs () && !loading)
			StartCoroutine ("LoadNextScene");
	}

	IEnumerator LoadNextScene ()
	{
		loading = true;
		Fade.fadeInstance.StartCoroutine ("FadeToBlack", 0.3f);
		yield return new WaitForSeconds (0.3f);
		Application.LoadLevel (CharacterSelectionManager.manager.nextScene);
	}

	void RevealCharacterName ()
	{
		switch (currentSelection) {
		case 0://Mage
			characterName.text = "Atticus";
			break;
		case 1: //Archer
			characterName.text = "Latch";		
			break;
		case 2://Paladin
			characterName.text = "Makaria";
			break; 
		case 3:// Warrior
			characterName.text = "Yue";
			break;
		}
	}
}
