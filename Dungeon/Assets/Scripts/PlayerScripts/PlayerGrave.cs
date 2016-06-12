using UnityEngine;
using System.Collections;

public class PlayerGrave : MonoBehaviour {
	public GameObject playerObject;// Set this to the GameObject of the player that the grave belongs to.

	public float raiseTime = 1f;
	public float endHeight = 5f;
	private Transform graveObjectTransform;
	private Vector3 bloomStartPosition;
	
	private GameObject bloomObject;
	private Transform bloomEnd;
	public float bloomTime = 3f;
	// Use this for initialization
	void Start () 
	{
		bloomObject = transform.Find("Bloom").gameObject as GameObject;
		bloomEnd = transform.Find ("BloomEndPosition");
		bloomStartPosition = bloomObject.transform.position;
		graveObjectTransform = transform.Find ("GraveStoneObject");
		StartCoroutine ("StartBloom");
		///TEST
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private IEnumerator StartBloom()
	{
		Debug.Log ("Started Bloom");
		Vector3 _startPos = bloomObject.transform.position;
		Vector3 _endPos = bloomEnd.position;
		float counter = 0;
		float lerpAmmount;
		//Update position every frame
		while (counter < bloomTime)
		{
			lerpAmmount = counter/bloomTime;
			bloomObject.transform.position = Vector3.Lerp (_startPos, _endPos, lerpAmmount);
			counter += Time.deltaTime;
			yield return null;
		}
		Debug.Log ("Finished Bloom");
		StartCoroutine("RaiseGrave");
	}
	public IEnumerator RaiseGrave()
	{
		Debug.Log ("Started Raise Grave");
		Vector3 _startPos = graveObjectTransform.position;
		Vector3 _endPos = new Vector3(graveObjectTransform.position.x, graveObjectTransform.position.y+endHeight, graveObjectTransform.position.z);
		float counter = 0;
		float lerpAmmount;
		//Update position every frame
		while (counter < raiseTime)
		{
			lerpAmmount = counter/raiseTime;
			graveObjectTransform.position = Vector3.Lerp (_startPos, _endPos, lerpAmmount);
			counter += Time.deltaTime;
			yield return null;
		}
		Debug.Log ("Finished Raise Grave");
	}

	public IEnumerator DestroyGrave()
	{
		Destroy (graveObjectTransform.gameObject);
		Debug.Log ("Started Unbloom");
		Vector3 _startPos = bloomEnd.position;
		Vector3 _endPos = bloomStartPosition;
		float counter = 0;
		float lerpAmmount;
		//Update position every frame
		while (counter < bloomTime)
		{
			lerpAmmount = counter/bloomTime;
			bloomObject.transform.position = Vector3.Lerp (_startPos, _endPos, lerpAmmount);
			counter += Time.deltaTime;
			yield return null;
		}
		Debug.Log ("Finished Unbloom");
		Destroy (gameObject);

	}

}
