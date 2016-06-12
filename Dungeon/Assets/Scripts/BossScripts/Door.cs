using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public enum doorStates {open, closing, closed, opening};
	public doorStates doorState;
	public GameObject doorGameObject;
	public float raiseTime = 1f;
	public Vector3 raisedPosition = new Vector3(0, 5.24f, 0);
	public Vector3 loweredPosition = Vector3.zero;
	private AudioSource audioSource;
	public AudioClip doorSound;
	// Use this for initialization
	void Start () 
	{
		audioSource = GetComponent<AudioSource> ();
		InitaliseDoor ();
		//StartCoroutine ("OpenDoor");
		//StartCoroutine ("CloseDoor");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	//Set the door to match it's inital state.
	void InitaliseDoor()
	{
		if (doorState == doorStates.closed || doorState == doorStates.opening) 
		{
			doorGameObject.transform.localPosition = loweredPosition;
		}
		if (doorState == doorStates.open || doorState == doorStates.closing) 
		{
			doorGameObject.transform.localPosition = raisedPosition;
		}
	}

	//if the door is closed open it.
	public IEnumerator OpenDoor()
	{
		//Debug.Log ("Starting Open Door Coroutine");
		if (doorState == doorStates.closed) 
		{
			//Debug.Log ("Changing state to opening");
			doorState = doorStates.opening;
			audioSource.PlayOneShot(doorSound);
		}


		if (doorState == doorStates.opening)
		{
			//Debug.Log ("Opening Door");
			float counter = raiseTime;
			float timePassed = 0;
			while(counter>=0)
			{
				timePassed+=Time.deltaTime;
				float lerpVal = timePassed/raiseTime;
				doorGameObject.transform.localPosition = Vector3.Lerp(loweredPosition, raisedPosition, lerpVal);
				counter -= Time.deltaTime;
				yield return null;
			}
			doorState = doorStates.open;
		}
	}

	public IEnumerator CloseDoor()
	{
		//Debug.Log ("Starting Close Door Coroutine");
		if (doorState == doorStates.open) 
		{
			//Debug.Log ("Changing state to closing");
			doorState = doorStates.closing;
			audioSource.PlayOneShot(doorSound);

		}
		
		
		if (doorState == doorStates.closing)
		{
			//Debug.Log ("Closing Door");

			float counter = raiseTime;
			float timePassed = 0;
			while(counter>=0)
			{
				timePassed+=Time.deltaTime;
				float lerpVal = timePassed/raiseTime;
				doorGameObject.transform.localPosition = Vector3.Lerp(raisedPosition, loweredPosition, lerpVal);
				counter -= Time.deltaTime;
				yield return null;
			}
			doorState = doorStates.closed;
		}
	}
}
