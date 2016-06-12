using UnityEngine;
using System.Collections;

public class ProgressionScript : MonoBehaviour {

	public bool route1AStart;
	public bool route1AEnd;
	public bool route1BStart;
	public bool route1BEnd;
	public bool route2AStart;
	public bool route2AEnd;
	public bool route2BStart;
	public bool route2BEnd;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Waypoint1AStart")
		{
			route1AStart = true;
//			Debug.LogError("Works");
		}
		
		if (other.tag == "Waypoint1BStart")
		{
			route1BStart = true;
		}
		
		if (other.tag == "Waypoint1AEnd")
		{
			route1AEnd = true;
		}
		
		if (other.tag == "Waypoint1BEnd")
		{
			route1BEnd = true;
		}
		
		if (other.tag == "Waypoint2AStart")
		{
			route2AStart = true;
		}
		
		if (other.tag == "Waypoint2BStart")
		{
			route2BStart = true;
		}
		
		if (other.tag == "Waypoint2AEnd")
		{
			route2AEnd = true;
		}
		
		if (other.tag == "Waypoint2BEnd")
		{
			route2BEnd = true;
		}
	}
}
