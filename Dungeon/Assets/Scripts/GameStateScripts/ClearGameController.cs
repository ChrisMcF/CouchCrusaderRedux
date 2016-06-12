using UnityEngine;
using System.Collections;

public class ClearGameController : MonoBehaviour
{

	private GameObject gameController;
	// Use this for initialization
	void Start ()
	{
		if (gameController = GameObject.Find ("GameController")) {
			Destroy (gameController);
		}
	}
}
