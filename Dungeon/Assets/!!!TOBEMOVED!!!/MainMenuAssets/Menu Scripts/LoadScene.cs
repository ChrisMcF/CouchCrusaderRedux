using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
public class LoadScene : MonoBehaviour {
	public string levelToLoad;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(XCI.GetButtonUp(XboxButton.A))
		{
			Application.LoadLevel(levelToLoad);
		}
	}
}
