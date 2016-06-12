using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
public class WinScene : MonoBehaviour
{
	public string MainMenuString;
	//public GameObject LeaderBoardPanel;
	// Use this for initialization
	void Start ()
	{
		//LeaderBoardPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!(GameController.gameController.playerName == null) && !(GameController.gameController.playerName == "")) {
			if (XCI.GetButtonDown (XboxButton.B)) {
				GameObject sceneInfo = GameObject.Find ("Scene Specific Info");
				sceneInfo.GetComponent<SceneMusic> ().enabled = true;
				Application.LoadLevel (MainMenuString);
			
			}
			if (XCI.GetButtonDown (XboxButton.A)) {
				Application.LoadLevel ("AdventureMap");
			}
			if (XCI.GetButtonDown (XboxButton.Y)) {
				Application.Quit ();
			}
//		if (XCI.GetButtonDown(XboxButton.X))
//		{
//			Debug.Log ("test");
//			if (LeaderBoardPanel.activeSelf)
//			LeaderBoardPanel.SetActive(false);
//
//			else 
//				LeaderBoardPanel.SetActive(true);
//
//		}

		}
	}
}