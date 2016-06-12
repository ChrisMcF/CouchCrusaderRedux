using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
using XInputDotNetPure;

public class StopVibrations : MonoBehaviour
{

	void Start ()
	{
		StopAllRumbles ();
	}

	public void StopAllRumbles ()
	{
        if (GameController.gameController.players.Count != 0)
        {
            for (int i = GameController.gameController.players.Count; i > 0; i--)
            {
                GamePad.SetVibration((PlayerIndex)i, 0f, 0f);
            }
        }

	}
}
