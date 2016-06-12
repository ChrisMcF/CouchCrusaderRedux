using UnityEngine;
using System.Collections;

public class AbraMeleeSphere : MonoBehaviour {

	public GameObject abraSpherePrefab;
//	GameObject _sphere;
//
//	public Transform spawnPos;
//	public float startRadius;
//	public float endRadius;
//	public float expandTime;
//
//	Vector3 _startScale;
//	Vector3 _endScale;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown("a")){
			StartCoroutine("CreateExpandingSphere");
		}

		if (Input.GetKeyDown ("s")){
			Instantiate (abraSpherePrefab, transform.position, abraSpherePrefab.transform.rotation);
		}
	}

//	public IEnumerator CreateExpandingSphere (){
//
//		_sphere = (GameObject)Instantiate (abraSpherePrefab, spawnPos.position, abraSpherePrefab.transform.rotation);
//
//		_startScale = new Vector3(1, 1, 1)*startRadius;
//		_endScale = new Vector3(1, 1, 1)*endRadius;
//		_sphere.transform.localScale = _startScale;
//
//		float i = 0.0f;
//		float _speed = 1.0f/expandTime;
//
//		while (i < 1.0f){
//
//			i += Time.deltaTime*_speed;
//			_sphere.transform.localScale = Vector3.Lerp (_startScale, _endScale, i);
//
//			yield return null;
//		}
//
//		Destroy(_sphere);
//	}


}
