using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour {
	public Transform Owner;
	public float spread;
	public float LightBase;
	public bool isOn = true;
	// Use this for initialization
	void Start () {
		LightBase = GetComponent<Light>().range;
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.rotation = Quaternion.Euler(Owner.eulerAngles+SprayDirection());
		if(isOn)
		GetComponent<Light>().range = (LightBase * Random.Range(0.7f,1.4f));
		else
			if(GetComponent<Light>().range > 0)
				GetComponent<Light>().range = Mathf.Lerp(GetComponent<Light>().range, 0f, Time.deltaTime*2) ;
	}
	

	Vector3 SprayDirection() 
	{
		float vx = (Random.Range(-spread, spread));
		float vy = (Random.Range(-spread, spread));
		float vz = 1.0f;
		return Owner.TransformDirection(new Vector3(vx,vy,vz));
	}
}
