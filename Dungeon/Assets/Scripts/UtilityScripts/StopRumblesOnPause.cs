using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using XInputDotNetPure;

public class StopRumblesOnPause : MonoBehaviour {
	
	void OnEnable () {
		StopAllRumbles ();
	}


	public void StopAllRumbles()
	{
		Debug.Log ("Stopping Rumbles");
		for (int i = 0; i < XboxCtrlrInput.XCI.GetNumPluggedCtrlrs(); i++) 
		{
			GamePad.SetVibration ((PlayerIndex)i, 0f, 0f);
		}
		Debug.Log ("Stopped");
	}
}
