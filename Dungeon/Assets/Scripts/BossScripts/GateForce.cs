using UnityEngine;
using System.Collections;

public class GateForce : MonoBehaviour {

	float timer = 5f;
	bool hit;
	public GameObject Gate1, Gate2, Tombstone, Stages;
	int numberofPlayersInCollider = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;

		if (timer <= 0 && hit == false) {
			GetComponent<Rigidbody> ().isKinematic = false;
			GetComponent<Rigidbody> ().AddForce (Vector3.forward * 600f);
			hit = true;
			Destroy (Gate1, 3);
			Destroy (Gate2, 3);
			Invoke("Shakecam",0.1f);
			//iTween.MoveTo(Tombstone,iTween.Hash("position",new Vector3 (10f,0f,0f),"time", 3f, "easetype", iTween.EaseType.easeOutQuint));
			//MoveTombStone ();

		}

		if (hit == true && numberofPlayersInCollider == GameController.gameController.players.Count) 
		{
			//Tombstone.transform.localPosition=Vector3.MoveTowards (Tombstone.transform.localPosition, new Vector3 (0f,10f,0f), 7f*Time.deltaTime);
			MoveTombStone();
			if(Stages != null)
				Stages.transform.position=Vector3.MoveTowards (Stages.transform.position, new Vector3 (-2.490506f,18.17f,-7.785122f), 5f*Time.deltaTime);
		}
	

	}

	void MoveTombStone()
	{
		while (Tombstone.transform.localPosition!= new Vector3 (0f,10f,0f)) 
		{
			Tombstone.transform.localPosition = Vector3.MoveTowards (Tombstone.transform.localPosition, new Vector3 (0f, 10f, 0f), 0.1f * Time.deltaTime);
		}
	}

	void Shakecam()
	{
		Camera.main.GetComponent<CameraShake>().StartShake(1.5f);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
			numberofPlayersInCollider++;
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
			numberofPlayersInCollider--;
	}
}
