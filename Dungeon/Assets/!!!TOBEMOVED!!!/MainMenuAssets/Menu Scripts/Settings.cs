using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	public string mainMenuScene;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetKeyDown (KeyCode.Joystick1Button0)) 
		{
			Debug.Log ("A pressed");
			//Volume
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button3)) 
		{
			Debug.Log ("Y pressed");
			Application.LoadLevel(mainMenuScene);
		}
	}
}
