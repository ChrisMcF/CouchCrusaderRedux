using UnityEngine;
using System.Collections;

public class EnemyClass : MonoBehaviour
{

	public float _curHealth;
	public float _maxHealth;

	public void AdjustCurHealth (float val)
	{
		Debug.Log ("!");
		_curHealth += val; // positive values is HEAL, negative is DAMAGE
		
		Debug.Log ("Health: " + _curHealth);
		Debug.Log ("dAMAGE: " + val);
		if (_curHealth < 0)
			_curHealth = 0;

		if (_curHealth > _maxHealth)
			_curHealth = _maxHealth;

		if (_curHealth == 0) {
			Destroy (gameObject);
		}
	}
}
