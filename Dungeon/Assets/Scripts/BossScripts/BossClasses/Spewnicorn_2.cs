using UnityEngine;
using System.Collections;

public class Spewnicorn_2 : BaseBossClass
{
    [SerializeField]
    private float meleeCooldown;
    [SerializeField]
    private float meleeCooldownTimer;
    [SerializeField]
    private float spewCooldown;
    [SerializeField]
    private float spewCooldownTimer;

    private CharacterController _charController;
    public GameObject _spewParticles;



    private float moveSpeed;
    private bool encounterStarted = false;

    void Start()
    {
        moveSpeed = _moveSpeed;
        BossStart();
        meleeCooldownTimer = meleeCooldown;
        spewCooldownTimer = spewCooldown;
        _charController = GetComponent<CharacterController>();
        healthBar.enabled = false;
    }

    void Update()
    {
        if (!encounterStarted)
        {
            return;
        }
            

        AnimatorStateInfo _info = _animator.GetCurrentAnimatorStateInfo(0);
        if (!_info.IsTag("InAttack"))
        {
            if (_animator.GetInteger("AttIndex") != 0)
            {
                _animator.SetInteger("AttIndex", 0);
            }
        }

        Player target = GetPlayerWithHigestThreat();

        float distanceToTarget = Vector3.Distance(target.playerObject.transform.position, transform.position);

        if (meleeCooldownTimer > 0)
            meleeCooldownTimer -= Time.deltaTime;
        if (spewCooldownTimer > 0)
            spewCooldownTimer -= Time.deltaTime;

        if (meleeCooldownTimer <= 0 && distanceToTarget < 4f)
        {
            meleeCooldownTimer = meleeCooldown;
            _animator.SetInteger("AttIndex", 1);
        }
        else if (spewCooldownTimer <= 0 && distanceToTarget > 4f)
        {
            moveSpeed = 0;
            _animator.SetInteger("AttIndex", 2);
        }

        MoveTowardsTarget(target);

    }

    public void StartEncounter()
    {
        healthBar.enabled = true;
        encounterStarted = true;
    }

    void StartSpewAttack()
    {
        _spewParticles.SetActive(true);
    }

    void EndSpewAttack()
    {
        _spewParticles.SetActive(false);
        moveSpeed = _moveSpeed;
        spewCooldownTimer = spewCooldown;
    }

    void CallAttack(int index)
    {
        switch(index)
        {
            case 1:
                MeleeAttack(6f, 10f, 0.2f);
                break;
            default:
                StartSpewAttack();
                break;
        }
    }

    public void MeleeAttack(float range, float damage, float arc)
    {
        //check all loaded objects of tag
        foreach (Player targ in GameController.gameController.players)
        {
            //are they close enough to me?
            if (Vector3.Distance(targ.playerObject.transform.position, transform.position) < range)
            {
                //are they within the scan FOV?
                Vector3 _dir = (targ.playerObject.transform.position - transform.position).normalized;
                if (Vector3.Dot(transform.forward, _dir) > arc)
                {
                    targ.playerClass.AdjustCurHealth(-damage); //deal the damage
                }
            }
        }
    }

    void MoveTowardsTarget(Player target)
    {
        Vector3 offset = target.playerObject.transform.position - transform.position;

        
        //Get the difference.
        if (offset.magnitude > 2f)
        {
            //If we're further away than .1 unit, move towards the target.
            //The minimum allowable tolerance varies with the speed of the object and the framerate. 
            // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
            if (moveSpeed > 0)
                offset = offset.normalized * moveSpeed;
            else
                offset = offset.normalized;
            

            Quaternion rot = new Quaternion();
            rot.SetLookRotation(offset);
            transform.rotation = rot;
            //normalize it and account for movement speed.
            if (moveSpeed > 0)
                _charController.Move(offset * Time.deltaTime);
            //actually move the character.
        }
    }


}
