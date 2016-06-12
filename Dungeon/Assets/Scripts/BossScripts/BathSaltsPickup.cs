using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;

//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BathSaltsPickup : MonoBehaviour
{
	public GameObject target;
	public static int saltCount = 0;
	private SphereCollider sphereCollider;
	private bool pickedUp = false;
	public GameObject aButtonIcon;
	public List<GameObject> playersInTrigger = new List<GameObject> ();
	public Vector3 carryOffset;
	[Range(0,1)]
	public float
		movementSpeed;
	public int ID;
	private Rigidbody rb;
	[HideInInspector]
	public bool
		thrown = false;

	private bool _clearPlayers = false;
	
	void Start ()
	{
		saltCount++;
		ID = saltCount;
		sphereCollider = GetComponent<SphereCollider> ();
		sphereCollider.isTrigger = true;
		rb = GetComponent<Rigidbody> ();
		rb.isKinematic = true;
	}
  
	void PickupSalts (GameObject player)
	{
		PlayerArrow playerArrow = player.transform.Find ("Player Arrow").GetComponent<PlayerArrow> ();
		playerArrow.PointToThisTarget (target.transform);//Reveal Player Arrow

		aButtonIcon.SetActive (false);
        
		transform.position = player.transform.position + carryOffset;
        transform.position = player.transform.position + new Vector3(0,player.GetComponent<CharacterController>().bounds.size.y + carryOffset.y,0);
        transform.parent = player.transform;
		pickedUp = true;
		GetComponent<SphereCollider> ().enabled = false;
	}

    void DropSaltsOnDeath(GameObject player)
    {
        PlayerArrow playerArrow = player.transform.Find("Player Arrow").GetComponent<PlayerArrow>();
        playerArrow.DisableArrow();
        aButtonIcon.SetActive(true);
        transform.position = player.transform.position + new Vector3(0,1,0);
        transform.parent = null;
        pickedUp = false;
        sphereCollider.enabled = true;
		playersInTrigger.Remove (player);
    }

	void Update ()
	{
        if (pickedUp && transform.parent != null && transform.parent.GetComponent<BasePlayerClass>()._isDead)
        {
            DropSaltsOnDeath(transform.parent.gameObject);
        }

		if (!pickedUp) {

			GameObject p;
			for (int i = playersInTrigger.Count - 1; i >= 0; i--) {
				Debug.Log ("i = " + i);
				//foreach (GameObject p in playersInTrigger) {
				p = playersInTrigger [i];
				if (p != null) {
					switch (p.GetComponent<BasePlayerClass> ().controllerIndex) {
					case 1:
				//Check for player 1 a button
						if (XCI.GetButtonUp (XboxButton.A, 1)) {
							PickupSalts (p.gameObject);
							p.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
							_clearPlayers = true;
						}
						break;
					case 2:
				//Check for player 2 a button
						if (XCI.GetButtonUp (XboxButton.A, 2)) {
							PickupSalts (p.gameObject);
							p.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
							_clearPlayers = true;
						}
						break;
					case 3:
				//Check for player 3 a button
						if (XCI.GetButtonUp (XboxButton.A, 3)) {
							PickupSalts (p.gameObject);
							p.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
							_clearPlayers = true;
						}
						break;
					case 4:
				//Check for player 4 a button
						if (XCI.GetButtonUp (XboxButton.A, 4)) {
							PickupSalts (p.gameObject);
							p.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
							_clearPlayers = true;
						}
						break;
					}
				
					if (_clearPlayers) {
						playersInTrigger.Clear ();
						_clearPlayers = false;
						break;
					}
				}
			}
		}
	}
  
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player" && !pickedUp) {
			playersInTrigger.Add (other.gameObject);
			aButtonIcon.SetActive (true);
		}
	}
//	void OnTriggerStay (Collider other)
//	{
//		if (other.gameObject.tag == "Player" && !pickedUp) {
//			foreach (GameObject currentPlayer in playersInTrigger) {
//				switch (currentPlayer.GetComponent<BasePlayerClass> ().controllerIndex) {
//				case 1:
//	        		//Check for player 1 a button
//					if (XCI.GetButtonUp (XboxButton.A, 1)) {
//						PickupSalts (other.gameObject);
//						other.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
//					}
//					break;
//				case 2:
//	        		//Check for player 2 a button
//					if (XCI.GetButtonUp (XboxButton.A, 2)) {
//						PickupSalts (other.gameObject);
//						other.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
//					}
//					break;
//				case 3:
//	        		//Check for player 3 a button
//					if (XCI.GetButtonUp (XboxButton.A, 3)) {
//						PickupSalts (other.gameObject);
//						other.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
//					}
//					break;
//				case 4:
//	        		//Check for player 4 a button
//					if (XCI.GetButtonUp (XboxButton.A, 4)) {
//						PickupSalts (other.gameObject);
//						other.gameObject.GetComponent<CharacterHandler> ().DisableActions ();
//					}
//					break;
//				}
//			}
//		}
//	}  
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player" && !pickedUp) {
			playersInTrigger.Remove (other.gameObject);
			aButtonIcon.SetActive (false);
		}
	}
}