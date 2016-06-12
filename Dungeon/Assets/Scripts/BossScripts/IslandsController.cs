using UnityEngine;
using System.Collections;

public class IslandsController : MonoBehaviour 
{
	public GameObject frontIsland;
	public GameObject thisIsland;
	public GameObject backIsland;
	private int playerCount;
	bool isPlayerDetected = false;
	public Animator anim;

	// Use this for initialization
	void Start () 
	{
		//LowerIslands ();
		anim = thisIsland.GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () 
	{

		anim.SetBool ("isPlayerDetected", isPlayerDetected);
	}
	void OnTriggerStay(Collider collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "Player") 
		{
			Debug.Log ("Player Detected");
			isPlayerDetected = true;
			//RaiseIslands ();
		} 			
	}
//	void OnTriggerEnter(Collider collisionInfo)
//	{
//		if (collisionInfo.gameObject.tag == "Player") 
//		{
//			playerCount += 1;
//			collisionInfo.transform.parent = transform;
//		}
//		if (playerCount == GameController.gameController.players.Count)
//		{
//			isPlayerDetected = true;
//		}
//	}
	void OnTriggerExit(Collider collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "Player") 
		{
			Debug.Log ("Player Gone");
			isPlayerDetected = false;
			//LowerIslands ();
		} 			
	}
	public void RaiseIslands()
	{
		anim.Play("WalkableIsland");

			//frontIsland.SetActive(true);
			//thisIsland.SetActive(true);
			//backIsland.SetActive(true);
		}
	public void LowerIslands()
	{
		anim.Play("UnwalkableIsland");
			//frontIsland.SetActive(false);
			//thisIsland.SetActive(false);
			//backIsland.SetActive(false);
	}

}
