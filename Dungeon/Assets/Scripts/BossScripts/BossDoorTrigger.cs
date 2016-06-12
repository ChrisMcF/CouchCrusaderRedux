using UnityEngine;
using System.Collections;

public class BossDoorTrigger : MonoBehaviour {
	public Door bossRoomDoor;
	public Door preDungeonDoor;
	private int numberOfPlayers;
	private GameObject[] players = new GameObject[4];
	private int playersInCollider;
	private SceneMusic sceneMusic;
	private bool bossRoomOpen = false;
	private ScyllaBossClass scylla;
	public GameObject bossHealthBar;
    private bool bossTriggered = false;
	// Use this for initialization
	void Start () 
	{
        bossHealthBar.SetActive (false);
		GameObject levelSpecificObject = GameObject.FindGameObjectWithTag ("LevelSpecific") as GameObject;
		sceneMusic = levelSpecificObject.GetComponent<SceneMusic> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
		scylla = GameObject.Find ("Scylla").GetComponent<ScyllaBossClass> ();
		CheckNumberOfPlayers ();
	}

	// Update is called once per frame
	void Update () {
		CheckAndCloseDoor ();
	}
	
	IEnumerator TransitionIntoBossFight()
	{
        if (bossTriggered) yield return null;

        sceneMusic.PlayMusicArrayIndex(1);
        //scylla.levelStart = true;
        bossHealthBar.SetActive (true);
        bossTriggered = true;
		scylla.StartCoroutine ("BeginBossEncounter");
	}

	void CheckNumberOfPlayers()
	{
		for (int i =0; i<players.Length; i++) 
		{
			if(players[i] != null)
				numberOfPlayers++;
		}
		
	}

	void OnTriggerEnter(Collider col)
	{
		//Debug.Log (col.transform.gameObject.tag);
		if (col.gameObject.tag == "Player") 
		{
			playersInCollider ++;

            if (playersInCollider == GameController.gameController.players.Count && !bossTriggered)
            {
                preDungeonDoor.StartCoroutine("CloseDoor");

                //yield return new WaitForSeconds(preDungeonDoor.raiseTime);
                bossRoomDoor.StartCoroutine("OpenDoor");
                //yield return new WaitForSeconds(bossRoomDoor.raiseTime);
               
                bossRoomOpen = true;
            }

        }
	}

	void OnTriggerExit(Collider col)
	{
		//Debug.Log (col.transform.gameObject.tag);
		if (col.gameObject.tag == "Player") 
		{
			playersInCollider -= 1;
            // Close the door when all players are in the boss room

            if (playersInCollider == 0 && !bossTriggered)
                StartCoroutine("TransitionIntoBossFight");
        }
	}

	void CheckAndCloseDoor()
	{
		//Debug.Log ("players in coll "+playersInCollider + ", boss room open: " + bossRoomOpen + ", Door state);
		if (playersInCollider == 0 && bossRoomOpen && bossRoomDoor.doorState == Door.doorStates.open) {
            //Debug.Log("Close door condition is true");

            bossRoomDoor.StartCoroutine ("CloseDoor");
		}
	}
}
