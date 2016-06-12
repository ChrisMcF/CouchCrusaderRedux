using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class BaseClass : MonoBehaviour
{
    public string charName;

    public enum ActionType { Melee, Ranged };
    public ActionType action1Type;
    public ActionType action2Type;
    public ActionType action3Type;

    public float action1Range;
    public float action1MeleeArc;
    public GameObject action1Projectile;
    public float action1Damage;

    public float action2Range;
    public float action2MeleeArc;
    public GameObject action2Projectile;
    public float action2Damage;

    public float action3Range;
    public float action3MeleeArc;
    public GameObject action3Projectile;
    public float action3Damage;

	private GameObject playerPanel;
	private Slider healthBar;

    [HideInInspector]
	public bool isDead = false;

    [Range(0, 100)]
    public float _curHealth, _maxHealth, _specialMeterVal;
    [Range(10, 100)]
    public float _attack;
    [Range(200, 400)]
    public float _moveSpeed;
    [Range(0,1)]
    public float _defense, _critChance;
    [Range(0, 2)]
    public float specialIncrement;
    
    public int _controllerIndex;

    private bool attacking = false;
    private float attackCooldown = 2f;
    private Animator _animator;

	public void BaseStart()
	{
		playerPanel = GameObject.Find ("PlayerPanel " + _controllerIndex);
		if (playerPanel != null) {
			healthBar = playerPanel.transform.FindChild ("Player_HealthBar").gameObject.GetComponent<Slider> ();
			healthBar.maxValue = _maxHealth;
			healthBar.value = _maxHealth;
		}
	}

    public void BaseUpdate ()
    {

        _animator = GetComponent<Animator>();   

        if (!attacking)
        {
            if (Input.GetAxis("RT_player" + _controllerIndex.ToString()) == 1)
            {
                Debug.Log("Light Attack");
                attacking = true;
                attackCooldown = 1f;
                _animator.SetInteger("AttIndex", 1);
            }
            if (Input.GetAxis("LT_player" + _controllerIndex.ToString()) == 1)
            {
                Debug.Log("Heavy Attack");
                attacking = true;
                attackCooldown = 2f;
                _animator.SetInteger("AttIndex", 2);
            }

            if (Input.GetButtonDown("LB_player" + _controllerIndex.ToString())) 
			{ 
				Debug.Log("Special Attack"); 
				attacking = true;
				attackCooldown = 2f;
				_animator.SetInteger("AttIndex", 3);
			}
            //Do something here (Start animation?)
            if (Input.GetButtonDown("RB_player" + _controllerIndex.ToString())) 
			{ 
				if(!isDead)
				{
					Debug.Log("Dodge"); 
					AdjustCurHealth (- 20);
				}
			}
			//Do something here (Start animation?)
			if(_curHealth == 0 && isDead == false)
			{
					

			}
					
        }
        else
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0f) attacking = false;
        }

    }


    void AdjustSpecialMeter(float amount)
    {
        _specialMeterVal += amount * specialIncrement;
    }

	public void AdjustCurHealth(float val)
	{
		Debug.Log ("Hit! " + val);

		// positive values is HEAL, negative is DAMAGE
		if(val<0)
			_curHealth += (val - (val * _defense));
		else
			_curHealth += val;
		
		//clamp current health between min & max
		_curHealth = Mathf.Clamp (_curHealth, 0f, _maxHealth);

		healthBar.value = _curHealth;

		//Check if dead
		if (_curHealth == 0)
		{
            GameController.gameController.RemovePlayerFromList(gameObject);
			isDead = true;
            GetComponent<CharacterController>().enabled = false;
            GetComponent<CharacterHandler>().enabled = false;
            _animator.SetTrigger("Death");
			//Destroy(gameObject);
		}
		//Check for hit
		if (val < 0 && _curHealth >0) 
		{
			_animator.SetTrigger("TakeDamage");
		}
	}


}
