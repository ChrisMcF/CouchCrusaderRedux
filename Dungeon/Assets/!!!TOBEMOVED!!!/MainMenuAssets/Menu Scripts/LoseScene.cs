using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
public class LoseScene : MonoBehaviour {

	void Update () 
	{
        if (XCI.GetButtonDown(XboxButton.X))
        {
            Application.LoadLevel("MainMenu");
        }
        if (XCI.GetButtonDown(XboxButton.Y))
        {
            Application.Quit();
        }
    }
}