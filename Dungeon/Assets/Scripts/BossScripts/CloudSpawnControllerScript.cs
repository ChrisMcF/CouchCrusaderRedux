using UnityEngine;
using System.Collections;

public class CloudSpawnControllerScript : MonoBehaviour {

	public GameObject spawn1;
	public GameObject spawn2;
	public GameObject spawn3;
	public GameObject spawn4;
	public GameObject spawn5;
	public GameObject spawn6;
	public GameObject spawn7;
	public GameObject spawn8;
	public GameObject spawn9;
	public GameObject spawn10;
	public GameObject spawn11;
	public GameObject spawn12;
	public GameObject spawn13;

	private bool spawn1Active;
	private bool spawn2Active;
	private bool spawn3Active;
	private bool spawn4Active;
	private bool spawn5Active;
	private bool spawn6Active;
	private bool spawn7Active;
	private bool spawn8Active;
	private bool spawn9Active;
	private bool spawn10Active;
	private bool spawn11Active;
	private bool spawn12Active;
	private bool spawn13Active;

	private int spawnSelector;
	private int counter;
	private bool go;

	// Use this for initialization
	void Start () {
		spawn1Active = false;
		spawn2Active = false;
		spawn3Active = false;
		spawn4Active = false;
		spawn5Active = false;
		spawn6Active = false;
		spawn7Active = false;
		spawn8Active = false;
		spawn9Active = false;
		spawn10Active = false;
		spawn11Active = false;
		spawn12Active = false;
		spawn13Active = false;
		counter = 0;
		go = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (go == true) {
			cloudControll ();
		}
	}
	
	void cloudControll()
    {
        spawnSelector = Random.Range (0, 14);
		
		if (spawnSelector == 1 & spawn1Active == false) {
			spawn1.SetActive(true);
			spawn1Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 2 & spawn2Active == false) {
			spawn2.SetActive(true);
			spawn2Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 3 & spawn3Active == false) {
			spawn3.SetActive(true);
			spawn3Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 4 & spawn4Active == false) {
			spawn4.SetActive(true);
			spawn4Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 5 & spawn5Active == false) {
			spawn5.SetActive(true);
			spawn5Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 6 & spawn6Active == false) {
			spawn6.SetActive(true);
			spawn6Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 7 & spawn7Active == false) {
			spawn7.SetActive(true);
			spawn7Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 8 & spawn8Active == false) {
			spawn8.SetActive(true);
			spawn8Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 9 & spawn9Active == false) {
			spawn9.SetActive(true);
			spawn9Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 10 & spawn10Active == false) {
			spawn10.SetActive(true);
			spawn10Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 11 & spawn11Active == false) {
			spawn11.SetActive(true);
			spawn11Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 12 & spawn12Active == false) {
			spawn12.SetActive(true);
			spawn12Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		if (spawnSelector == 13 & spawn13Active == false) {
			spawn13.SetActive(true);
			spawn13Active = true;
			counter = counter +1;
			go = false;
			StartCoroutine(Wait());
		}
		
		if (counter == 13) {
			gameObject.SetActive(false);
		}
	}

	IEnumerator Wait() {
//		yield return new WaitForSeconds (Random.Range (4, 11));
		yield return new WaitForSeconds (5);
		go = true;
	}
}
