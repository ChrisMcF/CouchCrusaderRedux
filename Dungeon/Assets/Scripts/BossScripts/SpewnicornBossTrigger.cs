using UnityEngine;
using System.Collections;

public class SpewnicornBossTrigger : MonoBehaviour {


    private int playerCount;
    private int playersInTrigger = 0;
	public AudioClip doorOpenSound;
	private SceneMusic sceneMusic;
	private bool triggered = false;

   // private GameObject platformReturnTarget;
    //private GameObject platform;
    private GameObject doorwayStone;
    private GameObject spewnicorn;
	private AudioSource audioSource;
	void Start ()
    {
		audioSource = GetComponent<AudioSource> ();
		sceneMusic = GameObject.FindGameObjectWithTag ("LevelSpecific").GetComponent<SceneMusic> ();
        spewnicorn = GameObject.Find("Spewnicorn");
        playerCount = GameController.gameController.players.Count;
        doorwayStone = GameObject.Find("DoorwayStone");
        //platform = GameObject.Find("Platform");
        //platformReturnTarget = GameObject.Find("PlatformReturnTarget");
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInTrigger += 1;
            if (playersInTrigger == playerCount && !triggered)
			{
               // platform.GetComponent<PlatformScript>().MovePlatform(platformReturnTarget);
                spewnicorn.GetComponent<Spewnicorn_2>().StartEncounter();
				audioSource.PlayOneShot(doorOpenSound);
				sceneMusic.PlayMusicArrayIndex(1);
                Vector3 newPos = doorwayStone.transform.position;
                newPos.y -= 10f;
                doorwayStone.GetComponent<SimpleMoveObject>().StartMove(newPos, 4f);
				triggered = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInTrigger -= 1;
        }
    }
}
