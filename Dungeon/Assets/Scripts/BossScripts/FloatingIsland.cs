using UnityEngine;
using System.Collections;

public class FloatingIsland : MonoBehaviour {
	public float amplitude;
	public float speed; 
	private float tempVal;
	private Vector3 tempPos;
	
	void Start () 
	{
		tempPos = transform.position;
		tempVal = transform.position.y;
	}
	
	void Update () 
	{        
		tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);
		transform.position = tempPos;
	}

}