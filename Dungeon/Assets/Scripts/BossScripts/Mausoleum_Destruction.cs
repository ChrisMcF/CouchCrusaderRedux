using UnityEngine;
using System.Collections;

public class Mausoleum_Destruction : MonoBehaviour
{
	//Variable to control how long until the statue will start to fade
	public float timeUntilFade;
	//Variable to control how long it takes to fade out
	public float timeToFadeOut;
	//Variable to control the type of movement curve
	public iTween.EaseType curve;


	// Use this for initialization
	void Start ()
	{
		foreach (Transform childTransform in gameObject.transform) {
			iTween.ColorTo (childTransform.gameObject, iTween.Hash ("a", 0, "time", timeToFadeOut, "delay", timeUntilFade, "easetype", curve, "oncomplete", "DestroyRubble"));
		}
	}
	

	void DestroyRubble ()
	{
		foreach (Transform childTransform in gameObject.transform)
			Destroy (childTransform.gameObject);
		Destroy (gameObject);
	}

}
