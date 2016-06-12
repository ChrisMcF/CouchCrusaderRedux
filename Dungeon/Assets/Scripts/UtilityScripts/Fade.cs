using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
	public static Fade fadeInstance;
	public float fadeInTime = 3f;
	public Image image;
	private Camera cam;
	private GameObject blackQuad;

	void Awake ()
	{
		//setup the instance
		if (fadeInstance == null)
			fadeInstance = this;
	}

	//Use this for initialization
	void Start ()
	{
        cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		//CreateBlackQuad ();
		CreateImage ();
		if (fadeInTime != 0)
			StartCoroutine ("FadeFromBlack", fadeInTime);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void CreateImage ()
	{

	}

	public void Fadeout (int timeToFadeout)
	{
		if (fadeInTime != 0) {
			StartCoroutine ("FadeToBlack", timeToFadeout);
		}
	}

	void CreateBlackQuad ()
	{
		blackQuad = GameObject.CreatePrimitive (PrimitiveType.Quad);
		blackQuad.transform.position = cam.transform.position + cam.transform.forward;
		blackQuad.transform.localScale = new Vector3 (900, 900, 900);
		//	MeshRenderer renderer = blackQuad.GetComponent<MeshRenderer> ();
		Material material = new Material (Shader.Find ("Transparent/Diffuse"));
		material.color = Color.black;
		//renderer.material = material;
	}

	IEnumerator FadeFromBlack (float fadeTime)
	{
		// enable the fade canvas
		if (image.transform.parent != null) {
			image.transform.parent.gameObject.SetActive (true);
		}
		float counter = fadeTime;
		while (counter >0) {
			image.color = Color.Lerp (new Color (0, 0, 0, 0), new Color (0, 0, 0, 1), counter / fadeTime);
			counter -= Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator FadeToBlack (float fadeTime)
	{
		// enable the fade canvas
		if (image.transform.parent != null) {
			image.transform.parent.gameObject.SetActive (true);
		}
		float counter = fadeTime;
		while (counter >0) {
			image.color = Color.Lerp (new Color (0, 0, 0, 1), new Color (0, 0, 0, 0), counter / fadeTime);
			counter -= Time.deltaTime;
			yield return null;
		}
	}
}
