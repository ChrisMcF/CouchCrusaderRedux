using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {
	
	public GameObject target;

	public float moveTime;

	private int playerCount;


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
            other.transform.parent = null;
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerCount += 1;
            other.transform.parent = transform;
        }
		if (playerCount == GameController.gameController.players.Count)
		{
			StartCoroutine ("moveToPosition");
		}
	}

	IEnumerator moveToPosition()
	{
        yield return new WaitForSeconds(1f);
		Vector3 startPosition = transform.position;

		float counter = moveTime;

		while (counter > 0) {
			transform.position = Vector3.Lerp(startPosition, target.transform.position, 1-counter/moveTime);
			counter -= Time.deltaTime;
			yield return null;
		}
	}

    public void MovePlatform(GameObject moveTarget)
    {
        target = moveTarget;
        StartCoroutine("moveToPosition");
    }
}
