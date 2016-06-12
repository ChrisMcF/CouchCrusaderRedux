using UnityEngine;
using System.Collections;

public class CheckForLossCondition : MonoBehaviour {
	private float startTime = 1f;

    private bool playersDead = false;

	// Update is called once per frame
	void Update () {
		if (startTime > 0)
			startTime -= Time.deltaTime;
		else
			CheckForLoss ();
	}

	//If all players are dead, load the defeat screen
	void CheckForLoss()
	{
		if (GameController.gameController.players.Count == 0 && !playersDead) 
		{
            playersDead = true;
			GameController.gameController.StartCoroutine ("Defeat");
		}
	}
}
