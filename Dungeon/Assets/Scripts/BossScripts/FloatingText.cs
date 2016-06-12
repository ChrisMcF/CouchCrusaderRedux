using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

    public float fadeTime = 4f;
    public Vector3 textOffset;

	void Start ()
    {
		Destroy (gameObject, fadeTime);
        transform.GetChild(0).transform.localPosition = textOffset;
        iTween.FadeTo(gameObject, 0, fadeTime);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(Camera.main.transform.position);
	}
}
