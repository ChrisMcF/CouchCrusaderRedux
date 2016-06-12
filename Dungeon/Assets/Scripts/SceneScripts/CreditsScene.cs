using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
public class CreditsScene : MonoBehaviour {
	public string MainMenuString;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(XCI.GetButtonUp(XboxButton.X))
		{
			GameObject sceneInfo = GameObject.Find("Scene Specific Info");
			sceneInfo.GetComponent<SceneMusic>().enabled = true;
			Application.LoadLevel(MainMenuString);
			
		}
		if(XCI.GetButtonDown(XboxButton.Y))
		{
			Application.Quit();
		}
	}
}