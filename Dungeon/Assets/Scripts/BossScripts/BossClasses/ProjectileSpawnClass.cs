using UnityEngine;
using System.Collections;

public class ProjectileSpawnClass : MonoBehaviour {

	public GameObject minion, scylla;

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Ground")
		{
			Vector3 place = transform.position;
			Quaternion rot = transform.rotation;
			Destroy (gameObject);
			Instantiate (minion, place, rot);
		}
		else if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<BasePlayerClass>().AdjustCurHealth (-40);
			Destroy(gameObject);
		}
		else if (col.gameObject.tag == "Boss")
		{
			return;
		}
		else
		{
			Vector3 place = transform.position;
			Quaternion rot = transform.rotation;
			Destroy (gameObject);
			Instantiate (minion, place, rot);
		}
	}

	void Start () {

		scylla = GameObject.Find ("Scylla");

	}

	void FixedUpdate ()
	{
		Physics.IgnoreCollision (scylla.GetComponentInChildren<MeshCollider> (), GetComponent<Collider> ());
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (Vector3.forward * 10 * Time.deltaTime);
		
	}
}
