using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using XboxCtrlrInput;


public class PausedChar : MonoBehaviour
{

	public int controllerIndex;
	public bool coolingDown = false;
	public GameObject regularStyle;
	public GameObject twinStickStyle;
	public bool isTwinStickStyle = true;
	private AudioSource audioSource;

	private CharacterHandler charHandler;

	private GameObject[] playerObjects;
	private CharacterHandler[] playerCharacterHandlers;
	private BasePlayerClass[] basePlayerClasses;
	void Start ()
	{
		playerObjects = GameObject.FindGameObjectsWithTag ("Player");
		playerCharacterHandlers = new CharacterHandler[playerObjects.Length];
		basePlayerClasses = new BasePlayerClass[playerObjects.Length];
		for (int i = 0; i<playerObjects.Length; i++) {
			playerCharacterHandlers [i] = playerObjects [i].GetComponent<CharacterHandler> ();
			basePlayerClasses [i] = playerObjects [i].GetComponent<BasePlayerClass> ();
		}
		//Debug.Log (playerObjects.Length);
		//Debug.Log (playerCharacterHandlers.Length);
		//Debug.Log (basePlayerClasses.Length);
		audioSource = GetComponent<AudioSource> ();
		//LoadControllerStyles ();
	}

	void OnEnable ()
	{
		coolingDown = false;
	}

	/*
	void LoadControllerStyles ()
	{
		switch (controllerIndex) {
		case 1:
			if (GameController.gameController.player1ControllerStyle == GameController.ControllerStyles.TwinStick)
				SetupForTwinStick (true);
			else
				SetupForTwinStick (false);			
			break;
		case 2:
			if (GameController.gameController.player2ControllerStyle == GameController.ControllerStyles.TwinStick)
				SetupForTwinStick (true);
			else
				SetupForTwinStick (false);			
			break;
		case 3:
			if (GameController.gameController.player3ControllerStyle == GameController.ControllerStyles.TwinStick)
				SetupForTwinStick (true);
			else
				SetupForTwinStick (false);			
			break;
		case 4:
			if (GameController.gameController.player4ControllerStyle == GameController.ControllerStyles.TwinStick)
				SetupForTwinStick (true);
			else
				SetupForTwinStick (false);			
			break;
		}
	}
*/
	void SetupForTwinStick (bool isTrue)
	{
		if (isTrue) {
			isTwinStickStyle = true;
			twinStickStyle.SetActive (true);
			regularStyle.SetActive (false);
		} else {
			isTwinStickStyle = false;
			twinStickStyle.SetActive (false);
			regularStyle.SetActive (true);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (GameController.gameController.gamePaused) {
			if (!coolingDown && XCI.GetAxis (XboxAxis.LeftStickX, controllerIndex) < -0.5) {
				Debug.Log ("Left Stick x < -0.1");
				StartCoroutine (SelectNext ());
			}
			
			if (!coolingDown && XCI.GetAxis (XboxAxis.LeftStickX, controllerIndex) > 0.5) {
				Debug.Log ("Left Stick x > 0.1");
				StartCoroutine (SelectNext ());
			}
		}

	}
	public IEnumerator SelectNext ()
	{
		Debug.Log ("Check 1");
		coolingDown = true;
		isTwinStickStyle = !isTwinStickStyle;
		audioSource.PlayOneShot (PauseStef.instance.flick);
		if (isTwinStickStyle) {
			regularStyle.SetActive (false);
			twinStickStyle.SetActive (true);
		} else {
			regularStyle.SetActive (true);
			twinStickStyle.SetActive (false);
		}
		/*
		for (int i = 0; i<playerCharacterHandlers.Length; i++) {
			if (basePlayerClasses [i].controllerIndex == controllerIndex) {
				ApplyControllerStyle (playerCharacterHandlers [i]);
			}
		}*/

		//Debug.Log ("Check 2");
		//yield return new WaitForSeconds (PauseStef.instance.stickCooldown);
		float counter = 0f;
		//Debug.Log ("Check 3");
		float startTime = Time.realtimeSinceStartup;
		while (counter < PauseStef.instance.stickCooldown) {
			counter = Time.realtimeSinceStartup - startTime;
			//	Debug.Log("Counting down, at : " + counter);
			yield return null;
		}
		//Debug.Log ("Check 4");
		coolingDown = false;
		//Debug.Log ("Check 5");
	}
	/*
	void ApplyControllerStyle (CharacterHandler charHdlr)
	{
		if (isTwinStickStyle) {
			charHdlr.controllerStyle = CharacterHandler.ControllerStyle.TwinStick;
			switch (controllerIndex) {
			case 1:
				GameController.gameController.player1ControllerStyle = GameController.ControllerStyles.TwinStick;
				break;
			case 2:
				GameController.gameController.player2ControllerStyle = GameController.ControllerStyles.TwinStick;
				break;
			case 3:
				GameController.gameController.player3ControllerStyle = GameController.ControllerStyles.TwinStick;
				break;
			case 4:
				GameController.gameController.player4ControllerStyle = GameController.ControllerStyles.TwinStick;
				break;
			}
		} else {
			charHdlr.controllerStyle = CharacterHandler.ControllerStyle.Regular;
			switch (controllerIndex) {
			case 1:
				GameController.gameController.player1ControllerStyle = GameController.ControllerStyles.Regular;
				break;
			case 2:
				GameController.gameController.player2ControllerStyle = GameController.ControllerStyles.Regular;
				break;
			case 3:
				GameController.gameController.player3ControllerStyle = GameController.ControllerStyles.Regular;
				break;
			case 4:
				GameController.gameController.player4ControllerStyle = GameController.ControllerStyles.Regular;
				break;
			}
		}
	}
*/


}
