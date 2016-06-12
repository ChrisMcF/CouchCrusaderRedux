using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{

    public float minZoom = 10f;
    public float maxZoom = 30f;

    public float padding;
    public float smoothing;

    public float distanceMultiplier = 1.1f;

    public float zoomSpeed;
    public float moveSpeed;

    public LayerMask layerMask;

    private List<Player> players;

    private float longestDistance;

    private Vector3 newParentPos;

    // Use this for initialization
    void Start()
    {
        transform.LookAt(transform.parent);

        players = GameController.gameController.players;
        StartCoroutine("MoveCameraToNewPos");
    }

    // Update is called once per frame
    void Update()
    {   
        longestDistance = 0;

        for (int i = GameController.gameController.players.Count - 1; i >= 0; i--)
        {
            Player p1 = GameController.gameController.players[i];
            for (int j = GameController.gameController.players.Count - 1; j >= 0; j--)
            {
                Player p2 = GameController.gameController.players[j];
                if (p1.playerObject != p2.playerObject)
                {
                    float distance = Vector2.Distance(new Vector2(p1.playerObject.transform.position.x,p1.playerObject.transform.position.z), new Vector2(p2.playerObject.transform.position.x, p2.playerObject.transform.position.z));

                    if (distance > longestDistance)
                    {
                        longestDistance = distance;
                    }
                }
            }
        }

        longestDistance *= distanceMultiplier;
        

        RaycastHit rayHit;

        if (Physics.Raycast(transform.position, transform.forward, out rayHit, 100f, layerMask))
        {
            float distanceToGround = Vector3.Distance(transform.position, rayHit.point);

            longestDistance = Mathf.Clamp(longestDistance, minZoom, maxZoom);

            if (distanceToGround + padding < longestDistance)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition - transform.forward * zoomSpeed, smoothing * Time.deltaTime);
            }
            if (distanceToGround - padding > longestDistance)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + transform.forward * zoomSpeed, smoothing * Time.deltaTime);
            }
        }
    }

    void GetPlayerCentre()
    {
		if (players [0].playerObject != null) 
		{
			Bounds playerBounds = new Bounds ();

			playerBounds.center = players [0].playerObject.transform.position;

			foreach (Player p in players) {
				playerBounds.Encapsulate (p.playerObject.transform.position);
			}

			Vector3 playerCentre = new Vector3 (playerBounds.center.x, transform.parent.position.y, playerBounds.center.z);

			if (Vector3.Distance (playerCentre, newParentPos) > 0.3f)
				newParentPos = playerCentre;

		}
    }

    IEnumerator MoveCameraToNewPos()
    {
        while (true)
        {
            if (GameController.gameController.players.Count > 0)
            {
                GetPlayerCentre();
                transform.parent.position = Vector3.Lerp(transform.parent.position, newParentPos, moveSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                break;
            }
            
        }
    }


}
