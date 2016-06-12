using UnityEngine;
using System.Collections;

public class ScyllaSplashSafeZones : MonoBehaviour {

	public ScyllaBossClass scylla;

	// Use this for initialization
	void Start () {

		scylla = GameObject.Find ("Scylla").GetComponent<ScyllaBossClass> ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col)
	{
        for (int i = GameController.gameController.players.Count - 1; i >= 0; i--)
        {
            Player p = GameController.gameController.players[i];
            if (p.playerObject.name == col.gameObject.name)
				scylla.dmgPlayer.Add(p);
		}
	}

	void OnTriggerExit (Collider col)
	{
        for (int i = GameController.gameController.players.Count - 1; i >= 0; i--)
        {
            Player p = GameController.gameController.players[i];
            if (p.playerObject.name == col.gameObject.name)
				scylla.dmgPlayer.Remove(p);
		}
	}
}
