using UnityEngine;
using System.Collections;

public class ParticleDamage : MonoBehaviour {

    public float particleDamage;

    public GameObject[] minions;

    private ParticleSystem parts;
    public ParticleCollisionEvent[] collisionEvents;

    public float spawnRate;
    private float timer;

    private bool allowSpawn;

    void Start()
    {
        timer = spawnRate;
        allowSpawn = true;
        parts = GetComponent<ParticleSystem>();
        collisionEvents = new ParticleCollisionEvent[16];
    }

    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (timer < 0 && allowSpawn == false)
        {
            allowSpawn = true;
        }
            

    }


    void OnParticleCollision(GameObject hitObject)
    {
        int safeLength = parts.GetSafeCollisionEventSize();
        if (collisionEvents.Length < safeLength)
            collisionEvents = new ParticleCollisionEvent[safeLength];

        int numCollisionEvents = parts.GetCollisionEvents(hitObject, collisionEvents);

        if (hitObject.tag == "Player")
        {
            hitObject.transform.root.SendMessage("AdjustCurHealth", -particleDamage, SendMessageOptions.DontRequireReceiver);
        }
        else if (allowSpawn && hitObject.name == "BossPlatformGround")
        {
            Vector3 pos = collisionEvents[numCollisionEvents- 1].intersection;
            Debug.Log("Spawn");
            allowSpawn = false;
            timer = spawnRate;
            int randomMinion = Random.Range(0, minions.Length);
            Instantiate(minions[randomMinion], pos, Quaternion.identity);
        }
            
    }
}

