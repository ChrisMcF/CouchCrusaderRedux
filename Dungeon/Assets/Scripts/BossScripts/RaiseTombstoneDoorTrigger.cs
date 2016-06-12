using UnityEngine;
using System.Collections;

public class RaiseTombstoneDoorTrigger : MonoBehaviour {

	float raiseTime = 1f;
	private int playersInTrigger = 0;
	bool raised = false;
	public GameObject ToombstoneDoor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			playersInTrigger++;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(playersInTrigger == GameController.gameController.players.Count && !raised)
			StartCoroutine("RaiseToombstoneDoor");
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			playersInTrigger--;
		}
	}

	IEnumerator RaiseToombstoneDoor()
	{
		//cameraShake.StartShake(raiseStageTime);
		Vector3 startPos = ToombstoneDoor.transform.localPosition;
		Vector3 endPos = new Vector3 (0, 10, 0);
		float timer = raiseTime;
		float ammount = 0;
		while (timer > 0)
		{
			ammount = timer/raiseTime;
			ToombstoneDoor.transform.localPosition = Vector3.Lerp (startPos, endPos, 1-ammount);
			timer -= Time.deltaTime;
			yield return null;
		}
	}
}
