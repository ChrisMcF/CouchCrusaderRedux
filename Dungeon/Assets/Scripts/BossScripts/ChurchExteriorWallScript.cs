using UnityEngine;
using System.Collections;

public class ChurchExteriorWallScript : MonoBehaviour {

	public GameObject ChurchExterior;
//	private bool active;

	// Use this for initialization
	void Start () {
//		active = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay()
	{
		ChurchExterior.SetActive(false);
	}
}
