using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScyllaDungeonCleaner : MonoBehaviour {
	//public Color startColor;
	public bool cleanUp = false;

	public Color endColor, startColor;
	public float lerpingSpeed;
	public Renderer rend;
	public float newAlpha;
	public GameObject soapBubbles;


	public Transform endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start () 
	{
		rend = GetComponent<Renderer>();

		startTime = Time.time;
		journeyLength = Vector3.Distance(transform.position, endMarker.position);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (cleanUp == true) 
		{
			StartCoroutine ("CleanDungeon1");

		}
	}
	public IEnumerator CleanDungeon1 ()
	{
		soapBubbles.GetComponent<ParticleSystem>().Simulate(1f);
		float counter = 3f;
		while (counter>0) {
			rend.material.color = Color.Lerp (startColor, endColor, 1 - counter/3f);
			counter -= Time.deltaTime;
			yield return null;
		}

			//yield return new WaitForSeconds(3);
		Debug.Log ("color changed, disappearing now...");
		soapBubbles.SetActive (false);
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(transform.position, endMarker.position, fracJourney);
		yield return new WaitForSeconds(6);

		Destroy (gameObject);
	}
	
	
}
