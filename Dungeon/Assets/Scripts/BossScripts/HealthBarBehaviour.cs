using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarBehaviour : MonoBehaviour {

	float _maxHealth;
	float _currentHealth;
	float _healthDecreaseEffect_start;
	float _healthDecreaseEffect_end;

	//public float _healthDecreaseEffect_startDelay;
	public float _healthDecreaseEffect_lifetime;

	Image _currentHealth_image;
	Image _healthDecreaseEffect_image;

	Camera _mainCamera;

	// Use this for initialization
	void Start () 
	{
		_currentHealth_image = transform.FindChild("CurrentHealth").GetComponent<Image>();
		_healthDecreaseEffect_image = transform.FindChild("HealthDecreaseEffect").GetComponent<Image>();
		_mainCamera = Camera.main;
		//_maxHealth = GetComponentInParent<HealthBar>().maxHealth;
		//_currentHealth = _maxHealth;
		_healthDecreaseEffect_start = 0;
		_healthDecreaseEffect_end = 0;
	}

	//call this from other scripts to initialize the healthbar.
	public void InitializeMaxAndCurrentHealth(float maxHealth)
	{
		_maxHealth = maxHealth;
		_currentHealth = maxHealth;
	}

	// Update is called once per frame
	void Update () {

		//Always faces the active camera with a billboard effect
		transform.LookAt(transform.position + _mainCamera.transform.rotation*Vector3.forward, _mainCamera.transform.rotation*Vector3.up);

		//Update the fill amount of current health
		_currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
		_currentHealth_image.fillAmount = _currentHealth/_maxHealth;
	}



	public void SetCurrentHealth(float value){
        if (value < _currentHealth){

			_healthDecreaseEffect_start = _currentHealth;
			_healthDecreaseEffect_end = Mathf.Clamp (value, 0, _healthDecreaseEffect_start);
			_currentHealth = value;

			StopCoroutine("DecreaseHealthEffect");
			StartCoroutine("DecreaseHealthEffect");
		}
		else {

			_currentHealth = value;
		}
	}



	IEnumerator DecreaseHealthEffect(){

		float i = 0.0f;
		float _speed = 1.0f/_healthDecreaseEffect_lifetime;
		//Update fill amount of _healthDecreaseEffect_image every frame
		while (i < 1.0f){
			
			i += Time.deltaTime*_speed;
			//transform.position = Vector3.Lerp (_startPos, _endPos, i);
			_healthDecreaseEffect_image.fillAmount = Mathf.Lerp (_healthDecreaseEffect_start/_maxHealth, _healthDecreaseEffect_end/_maxHealth, i);
			
			yield return null;
		}
	}
	public float GetCurrentHealth()
	{
		return _currentHealth;
	}
	public float GetMaxHealth()
	{
		return _maxHealth;
	}
}
