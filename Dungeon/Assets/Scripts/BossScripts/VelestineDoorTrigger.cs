using UnityEngine;
using System.Collections;

public class VelestineDoorTrigger : MonoBehaviour {

	private int playerCount;
	private int playersInTrigger = 0;
	public AudioClip doorOpenSound;
	private SceneMusic sceneMusic;
	private bool triggered = false;
	public GameObject[] doors;
	public GameObject colliders;
	private GameObject velestine;
	private AudioSource audioSource;
	public float raiseTime = 2f;
	public float DoorYOffset = 3.48f;
	// Use this for initialization
	void Start ()
	{

		audioSource = GetComponent<AudioSource> ();
		sceneMusic = GameObject.FindGameObjectWithTag ("LevelSpecific").GetComponent<SceneMusic> ();
		velestine = GameObject.Find("Velestine");
		playerCount = GameController.gameController.players.Count;
		colliders.SetActive (false);
		//door = GameObject.Find("Door");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			playersInTrigger += 1;
			if (playersInTrigger == playerCount && !triggered)
			{
				colliders.SetActive(true);
				// platform.GetComponent<PlatformScript>().MovePlatform(platformReturnTarget);
				velestine.GetComponent<VeleBoss>().StartEncounter();

				if(sceneMusic.musicArray.Length > 1)
					sceneMusic.PlayMusicArrayIndex(1);
				//Vector3 newPos = door.transform.position;
				//newPos.y += 2.1f;
				//door.GetComponent<SimpleMoveObject>().StartMove(newPos, 2f);
				StartCoroutine("RaiseAllDoors");
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

	IEnumerator RaiseAllDoors()
	{
		foreach (GameObject door in doors) 
		{
			yield return new WaitForSeconds(1f);
			StartCoroutine("RaiseDoor", door);
		}
	}


	IEnumerator RaiseDoor(GameObject door)
	{
		audioSource.PlayOneShot(doorOpenSound);
		Vector3 startPos = door.transform.position;
		Vector3 endPos = door.transform.position + new Vector3(0, DoorYOffset, 0);
		float counter = 0;
		while (counter <= raiseTime) {
			door.transform.position = Vector3.Lerp(startPos, endPos, counter/raiseTime);
			counter += Time.deltaTime;
			yield return null;
		}
	}
}
