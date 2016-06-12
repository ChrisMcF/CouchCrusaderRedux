using UnityEngine;
using System.Collections;

public class KillPlayers : MonoBehaviour {
	public GameObject playerGraveStone;
	public float sampleRange = 50f;
	public Vector3 graveRotation = new Vector3(270, 0, 0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && col.isTrigger == false) {
			BasePlayerClass basePlayerClass = col.gameObject.GetComponent<BasePlayerClass> ();
			if (basePlayerClass != null && !basePlayerClass._isDead) {
				col.gameObject.SendMessage ("AdjustCurHealth", -500000000f);
				Vector3 stonePosition;// = NavMesh.SamplePosition();
				Rigidbody rb = col.gameObject.AddComponent<Rigidbody> ();
				if (rb != null)
					rb.AddForce (-Vector3.up * 20f, ForceMode.Impulse);


				NavMeshHit hit;
				if (NavMesh.SamplePosition (col.transform.position, out hit, sampleRange, NavMesh.AllAreas)) {
					stonePosition = hit.position;
					GameObject graveStoneInstance = Instantiate (playerGraveStone, stonePosition, Quaternion.Euler (graveRotation))as GameObject;
					PlayerGrave grave = graveStoneInstance.GetComponent<PlayerGrave> ();
					if (grave != null)
						grave.playerObject = col.gameObject;
					else {
						Debug.LogError ("Could not get Player Grave");
					}
				} else {
					Debug.LogError ("Could not Find Edge");
				}

			}
		}
	}

}
