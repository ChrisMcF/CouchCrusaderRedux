using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
/*
 * This script maintains a list of points which are used for trail collision detection.
 */

//[RequireComponent(typeof(TrailRenderer))]
public class TrailCollision : MonoBehaviour 
{
	//[Tooltip("Enable this checkbox to detect along the outer edges of the Trail")]
	// public bool detectOutline = false;
	//[Tooltip("Enable this checkbox to detect along the center of the Trail")]
	//public bool detectCenter = true;

	private int numberOfPlayers;
	public TrailRenderer trailRenderer;
	private List<Vector3> trailCenterPoints = new List<Vector3>(); 	
	//private List<RaycastHit> hits = new List<RaycastHit>();
	private Vector3 lastDrop = Vector3.zero;

	[Tooltip("A layermask to control what objects/colliders are detected")]
	public LayerMask detect = -1;
	[Tooltip("Set this value to match the \"Min Vertex Distance\" value in the trail renderer")]
	public float dropRange = 1f; //The equivolent value on the trail renderer has not been exposed in Unity API
	//private float dropRange;
	//public float decaysAfter = 5f;
	//private TrailRenderer trailRenderer;
	[Tooltip("Set this value to match the \"Time\" value in the trail renderer")]
	public float decaysAfter = 4f;
	[Tooltip("Adjust this value to control how much damage is applied to the player who collides with the trail renderer")]
	public float damage = 500f;
	//private List <BasePlayerClass> playerBaseClasses = new List<BasePlayerClass>();// = new BasePlayerClass[numberOfPlayers]();
	//private GameObject[] players = new GameObject[4];

	void Start () 
	{
		//Ensure that damage hurts the player rather than heals the player.
		if (damage > 0)
			damage = damage * -1;


		//trailRenderer = GetComponent<TrailRenderer> ();
		lastDrop = transform.position;
		//players = GameObject.FindGameObjectsWithTag ("Player");

		//CheckNumberOfPlayers();
		//playerBaseClasses = new BasePlayerClass[numberOfPlayers]();
		//for (int i = 0; i < players.Length; i++) 
		//{
		//	if(players[i] != null)
		//		playerBaseClasses[i] = players[i].GetComponent<BasePlayerClass>();
		//}
		//foreach (GameObject p in players) 
		//decaysAfter = trailRenderer.time;
	}

	void Update () 
	{
		//Drop a new segment if the distance between the current location and the previous drop is greater than or equal to the dropRange value
		if (Vector3.Distance (transform.position, lastDrop) >= dropRange) {
			DropSegment (transform.position);
		}
	}
	void FixedUpdate () 
	{
		//RayCastBetweenPoints ();
		HurtPlayers();
	}

	//this drops a new segment or ads a new position to the trailCenterPoints list
	void DropSegment(Vector3 dropLocation)
	{
		lastDrop = dropLocation;
		trailCenterPoints.Add (dropLocation);
		StartCoroutine ("DecayTrailSegment");
	}
	
	//This coroutine is called each time a new segment section is dropped. it waits for the allocated ammount of time before deleting the last point in the 
	IEnumerator DecayTrailSegment()
	{
		//Debug.Log("New trail segment is begining to decay");
		yield return new WaitForSeconds (decaysAfter);
		if(trailCenterPoints.Count > 0)
			trailCenterPoints.RemoveAt (0);
	}

	//This function linecasts between each point in the trailCenterPoints list
	void HurtPlayers()
	{
		//hits.Clear ();
	
		if (trailCenterPoints.Count > 0) // if there is more than 1 point in the trail.
		{
			for (int i = 0; i < trailCenterPoints.Count -1; i++) 

			{
				RaycastHit hit = new RaycastHit ();
				//Debug.Log ("Trail check: " + i);
				if (Physics.Linecast (trailCenterPoints [i], trailCenterPoints [i + 1], out hit)) 
				{
					if(hit.transform.tag == "Player")
						hit.transform.gameObject.SendMessage("AdjustCurHealth", damage * Time.fixedDeltaTime, SendMessageOptions.RequireReceiver);
					/*
					for (int j = 0; j < GameController.gameController.players.Count; j++) 
					{
						if (hit.transform.gameObject == GameController.gameController.players[j].playerObject) 
						{
							Debug.Log ("Trail is hurting player: " + GameController.gameController.players [j].playerObject.name);
							GameController.gameController.players [j].playerClass.AdjustCurHealth (damage * Time.fixedDeltaTime);
						}
					}
					*/
				}
			}
		}
	}


	//this function draws icons at each point in the trailCenterPoints list and draws a visual representation of the linecasts.
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		for (int i = 0; i < trailCenterPoints.Count - 1; i++)  
		{
			Gizmos.DrawIcon (trailCenterPoints[i], "TrailMarker3.png");
			if(i != trailCenterPoints.Count)
			{
				Gizmos.DrawLine(trailCenterPoints[i], trailCenterPoints[i+1]);
			}
		}
	}

	public void StartNewTrail()
	{
		trailRenderer.time = decaysAfter;
	}

	public IEnumerator ClearTrail()
	{
		trailRenderer.time = -1;
		StopAllCoroutines();
		trailCenterPoints.Clear();
		//trailCenterPoints.RemoveAll();
		yield return new WaitForEndOfFrame();

	}
}
