using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;

[RequireComponent(typeof(SphereCollider))]
public class BathSaltsTarget : MonoBehaviour
{
	//public float gravity = -9.8f; // Vertical velocity ( velocity = gravity * time )
	public float throwTime = 1f; 
	public Vector3[] throwPoints = new Vector3[3];
	public Vector3 targetOffset = Vector3.zero;
	public bool isPreDungeon = false;
	private SphereCollider sphereCollider;
	private Vector3 target;
	public Animator anim;
	public AudioClip splashSound;
	//public AudioClip poolDrainSound;
	private AudioSource audioSource;
//	private float horizontalVelocity;
//	private float verticalVelocity;
	private Vector3 startPos;
//	private bool drawPath;
	public float throwHeight = 1f;
	private ScyllaDungeonCleaner scyllaDungeonCleaner;
	public GameObject slimeFloor;

	private ScyllaBossClass scylla;

	//private GameObject[] bathSaltGameObjects;
	//private BathSaltsPickup[] bathSaltsPickups;

	private List<GameObject> saltsInTrigger = new List<GameObject> ();

	void Start ()
	{
		audioSource = GetComponent<AudioSource> ();
		sphereCollider = GetComponent<SphereCollider> ();
		sphereCollider.isTrigger = true;
		UpdateTargetPosition ();
        //bathSaltGameObjects = GameObject.FindGameObjectsWithTag("Salts");

        scylla = GameObject.Find("Scylla").GetComponent<ScyllaBossClass>();
        slimeFloor = GameObject.Find("slimefloor");
		scyllaDungeonCleaner = slimeFloor.GetComponent<ScyllaDungeonCleaner>();
	}

	// This coroutine throws the bath salts at the target
	public IEnumerator ThrowBathSaltsAtTarget (GameObject saltObject)
	{
		saltObject.transform.parent = null;
		throwPoints [0] = saltObject.transform.position;
		//Find the apex position of the throw based on the up/down ratio of the start & end points and the distance between them
		float upValue = throwHeight;
		float downValue = target.y - saltObject.transform.position.y + throwHeight;
		float positiveDownValue = downValue;
		if (downValue < 0)
			positiveDownValue *= -1;

		float ratio = upValue / (upValue + positiveDownValue);
		//Debug.Log (ratio);

		//Vector3 apexPosition = (upValue / (upValue + positiveDownValue) * (saltObject.transform.position - target));// + saltObject.transform.position;


		Vector3 apexPosition = ((target - saltObject.transform.position) * ratio) + saltObject.transform.position; 
		apexPosition = new Vector3 (apexPosition.x, saltObject.transform.position.y + throwHeight, apexPosition.z);
		throwPoints [1] = apexPosition;
		throwPoints [2] = target;
		Throw (throwPoints, saltObject);
		BathSaltsPickup bathSalts = saltObject.GetComponent<BathSaltsPickup> ();
		bathSalts.aButtonIcon.SetActive (false);
		yield return new WaitForSeconds (throwTime);
		/*
		while(counter>=0)
		{
			drawPath = true;
			timePassed+=Time.deltaTime;
			float lerpVal = timePassed/throwTime;
			saltObject.transform.position = Vector3.Lerp(startPos, target, lerpVal);
			counter -= Time.deltaTime;
			yield return null;
		}
		*/
		Debug.Log ("Bath Salt in Pool");
		if (isPreDungeon) {
			//slimeFloor.SetActive (false);
			audioSource.PlayOneShot(splashSound);
			anim.SetTrigger("DrainSlime");
			scyllaDungeonCleaner.cleanUp = true;

		} else {
			audioSource.PlayOneShot(splashSound);
			scylla.healingFinished = true;
			scylla.healingStart = false;
		}

		// Call other function here
		RemoveBathSalts (saltObject);
	}

	void RemoveBathSalts (GameObject salt)
	{
		Destroy (salt);

	}

	void UpdateTargetPosition ()
	{
		target = transform.position + targetOffset;
	}

	void Throw (Vector3[] path, GameObject salts)
	{
		//Debug.Log ("Throwing the salts");
		//Debug.Log (path [0] + ", " + path [1] + ", " + path [2]);
		iTween.MoveTo (salts, iTween.Hash ("path", path, "time", throwTime, "easetype", iTween.EaseType.linear));
		saltsInTrigger.Clear ();
	}
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Salts") {
			BathSaltsPickup bathSalts = other.gameObject.GetComponent<BathSaltsPickup> ();
			saltsInTrigger.Add (other.gameObject);
			//Debug.Log ("salts in trigger" + saltsInTrigger.Count);
			bathSalts.aButtonIcon.SetActive (true);
		} 
	}

	//If the salts stay within the collision zone, check if the parent object (player) has pressed the a button.
	void Update ()
	{
		if (saltsInTrigger.Count > 0) {
			for (int i = saltsInTrigger.Count - 1; i > -1; i--) {
				//foreach (GameObject s in saltsInTrigger) {
				GameObject s = saltsInTrigger [i];
				if (s.gameObject.tag == "Salts") {
					if (s != null && s.transform.parent != null && s.transform.parent.gameObject != null) {
						BasePlayerClass playerBaseClass = s.transform.parent.gameObject.GetComponent<BasePlayerClass> ();
						CharacterHandler characterHandler = s.transform.parent.gameObject.GetComponent<CharacterHandler> ();
						if (playerBaseClass != null) {
							switch (s.transform.parent.gameObject.GetComponent<BasePlayerClass> ().controllerIndex) {
							case 1:
						//Check for player 1 a button
								if (XCI.GetButtonUp (XboxButton.A, 1)) {
									DisableArrowOnThis (s.gameObject);
									StartCoroutine ("ThrowBathSaltsAtTarget", s.gameObject);
									characterHandler.EnableActions ();
								}
								break;
							case 2:
						//Check for player 2 a button
								if (XCI.GetButtonUp (XboxButton.A, 2)) {
									DisableArrowOnThis (s.gameObject);
									StartCoroutine ("ThrowBathSaltsAtTarget", s.gameObject);
									characterHandler.EnableActions ();
								}
								break;
							case 3:
						//Check for player 3 a button
								if (XCI.GetButtonUp (XboxButton.A, 3)) {
									DisableArrowOnThis (s.gameObject);
									StartCoroutine ("ThrowBathSaltsAtTarget", s.gameObject);
									characterHandler.EnableActions ();
								}
								break;
							case 4:
						//Check for player 4 a button
								if (XCI.GetButtonUp (XboxButton.A, 4)) {
									DisableArrowOnThis (s.gameObject);
									StartCoroutine ("ThrowBathSaltsAtTarget", s.gameObject);
									characterHandler.EnableActions ();
								}
								break;
							}
						}
					}
				}
			}
		}
	}
	//}


//	//If the salts stay within the collision zone, check if the parent object (player) has pressed the a button.
//	void OnTriggerStay (Collider other)
//	{
//		if (other.gameObject.tag == "Salts") {
//			if (other != null && other.transform.parent != null && other.transform.parent.gameObject != null) {
//				BasePlayerClass playerBaseClass = other.transform.parent.gameObject.GetComponent<BasePlayerClass> ();
//				CharacterHandler characterHandler = other.transform.parent.gameObject.GetComponent<CharacterHandler> ();
//				if (playerBaseClass != null) {
//					switch (other.transform.parent.gameObject.GetComponent<BasePlayerClass> ().controllerIndex) {
//					case 1:
//						//Check for player 1 a button
//						if (XCI.GetButtonUp (XboxButton.A, 1)) {
//							DisableArrowOnThis (other.gameObject);
//							StartCoroutine ("ThrowBathSaltsAtTarget", other.gameObject);
//							characterHandler.EnableActions ();
//						}
//						break;
//					case 2:
//						//Check for player 2 a button
//						if (XCI.GetButtonUp (XboxButton.A, 2)) {
//							DisableArrowOnThis (other.gameObject);
//							StartCoroutine ("ThrowBathSaltsAtTarget", other.gameObject);
//							characterHandler.EnableActions ();
//						}
//						break;
//					case 3:
//						//Check for player 3 a button
//						if (XCI.GetButtonUp (XboxButton.A, 3)) {
//							DisableArrowOnThis (other.gameObject);
//							StartCoroutine ("ThrowBathSaltsAtTarget", other.gameObject);
//							characterHandler.EnableActions ();
//						}
//						break;
//					case 4:
//						//Check for player 4 a button
//						if (XCI.GetButtonUp (XboxButton.A, 4)) {
//							DisableArrowOnThis (other.gameObject);
//							StartCoroutine ("ThrowBathSaltsAtTarget", other.gameObject);
//							characterHandler.EnableActions ();
//						}
//						break;
//					}
//				}
//			}
//		}
//	}

	void DisableArrowOnThis (GameObject salts)
	{
		GameObject playerArrowObject = salts.transform.parent.Find ("Player Arrow").gameObject;
		PlayerArrow playerArrow = playerArrowObject.GetComponent<PlayerArrow> ();
		playerArrow.DisableArrow ();//Disable Player Arrow
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Salts") {
			BathSaltsPickup bathSalts = other.gameObject.GetComponent<BathSaltsPickup> ();
			saltsInTrigger.Remove (other.gameObject);
			bathSalts.aButtonIcon.SetActive (false);
		} 
	}
}