using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {
	public string CharacterScene;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Joystick1Button0)) 
		{
			Debug.Log ("A pressed");
			Application.LoadLevel(CharacterScene);
		}
		if (Input.GetKeyDown (KeyCode.Joystick1Button1)) 
		{
			Debug.Log ("B pressed");
			Application.Quit();
		}
	}


}
