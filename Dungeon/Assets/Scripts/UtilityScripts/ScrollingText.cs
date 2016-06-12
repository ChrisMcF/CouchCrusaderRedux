using UnityEngine;
using System.Collections;

public class ScrollingText : MonoBehaviour {

	public Camera cam;
	public GameObject camTemp;
    public float fadeTime = 4f;

	// Use this for initialization
	void Start () {

		camTemp = GameObject.Find ("CameraAnchor");
		//cam = camTemp.GetComponentInChildren<Camera> ();
		GameObject camObject = GameObject.FindGameObjectWithTag ("MainCamera") as GameObject;
		cam = camObject.GetComponent<Camera>();
		float time = 2f;
		
		Vector3 temp = new Vector3 (0, time, 0);
		Vector3 temp2 = transform.parent.position + temp;

		
		iTween.MoveTo (gameObject, temp2, time);
		iTween.FadeTo (gameObject, 0, fadeTime);
		iTween.ShakeScale (gameObject, new Vector3 (0.5f, 0.5f, 0), time / 2f);
		
		
		Destroy (gameObject, fadeTime);

	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.parent.LookAt(cam.transform.position);
		//if (cam != null) 
		//{
		//	transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
		//}
	
	}
}
