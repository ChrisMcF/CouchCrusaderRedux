using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbraMeleeSphereBehaviour : MonoBehaviour {

	public float startRadius;
	public float endRadius;
	public float expandTime;
	public float expandPower;
	public float damage;

	Vector3 _startScale;
	Vector3 _endScale;

	public float m_SpeedH = 0.1f;
	public float m_SpeedV = 0.1f;

	[HideInInspector]
	public List<GameObject> _damagedPlayers;

	Material _material;

	BasePlayerClass _basePlayerClass;




	// Use this for initialization
	void Start () {

		_material = GetComponent<Renderer>().material;

		StartCoroutine("ExpandSphere");
	}

	IEnumerator ExpandSphere(){
		
		_startScale = new Vector3(1, 1, 1)*startRadius;
		_endScale = new Vector3(1, 1, 1)*endRadius;
		transform.localScale = _startScale;
		
		float i = 0.0f;
		float _step = 0.818f;
		//Vector3 _stepScale = _startScale + ((_endScale - _startScale) * _step);
		float _speed = 1.0f/expandTime;
		
		while (i < 1.0f){
			
			i += Time.deltaTime*_speed;

			if (i < _step){

				transform.localScale = Vector3.Lerp (_startScale, _endScale, (Mathf.Pow (i, 3))/0.7f);
			}
			if (i >= _step){

				transform.localScale = Vector3.Lerp (_startScale, _endScale, ((Mathf.Sin ((1.7f*Mathf.PI*i) - (1.2f*Mathf.PI)))/2f) + 0.5f);
			}
			//transform.localScale = Vector3.Lerp (_startScale, _endScale, Mathf.Pow(i, expandPower));
			//transform.localScale = Vector3.Lerp (_startScale, _endScale, Mathf.Pow(i, 3) * (3*i * (2 * i - 5) + 10));

			float newOffsetH = Time.time * m_SpeedH;
			float newOffsetV = Time.time * m_SpeedV;
			
			_material.mainTextureOffset = new Vector2(newOffsetH, newOffsetV);
			
			yield return null;
		}

		i = 0.0f;

		while (i < 1.0f){
			
			i += Time.deltaTime*_speed;

			transform.localScale = Vector3.Lerp (_endScale, Vector3.zero, Mathf.Pow(i, expandPower));
			
			float newOffsetH = Time.time * m_SpeedH;
			float newOffsetV = Time.time * m_SpeedV;
			
			_material.mainTextureOffset = new Vector2(newOffsetH, newOffsetV);
			
			yield return null;
		}

		_damagedPlayers.Clear();

		Destroy (gameObject);
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Player"){

			_basePlayerClass = coll.gameObject.GetComponent<BasePlayerClass>();

			if (!_damagedPlayers.Contains(coll.gameObject) && !_basePlayerClass._isDead){

				_damagedPlayers.Add (coll.gameObject);

				coll.gameObject.SendMessage("AdjustCurHealth", damage);
			}
		}
	}
}
