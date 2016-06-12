using UnityEngine;
using System.Collections;

public class PlayerArrow : MonoBehaviour {
	//public Color start, end;
	public Transform target;
	private bool revealArrow = false;
	//private Vector3 direction;
	//private Quaternion targetRotation;
	//public float speed = 5f;
	private Vector3 targetPostition;
	// public GameObject marker;
	// Use this for initialization
	private GameObject arrowSprite;
	void Start () 
	{
		arrowSprite = transform.Find ("White_Arrow_Sprite").gameObject;
		DisableArrow ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//A convenience method to point towards a specified target
	public void PointToThisTarget(Transform pointToThis)
	{
		target = pointToThis;
		StartCoroutine("PointToTarget");
	}

	public void DisableArrow()
	{
		revealArrow = false;
		arrowSprite.SetActive (false);
	}

	IEnumerator PointToTarget()
	{
		arrowSprite.SetActive (true);
		revealArrow = true;
		while (revealArrow) 
		{
			targetPostition = new Vector3( target.position.x, transform.position.y, target.position.z ) ;
			//Instantiate(marker, targetPostition, Quaternion.identity);
			transform.LookAt(targetPostition) ;
			yield return null;
		}
	}
}
