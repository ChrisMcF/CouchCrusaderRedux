using UnityEngine;
using System.Collections;

public class MoveIslandBridge : MonoBehaviour {

	public enum BridgeStates {AtStart, MovingToDestination, AtDestination, MovingToStart};
	public BridgeStates bridgeState;
	public Transform moveToHere;
	private Vector3 startPosition;
	public float moveTime = 3f;
	public iTween.EaseType easing = iTween.EaseType.easeInOutCubic;
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}

	public IEnumerator MoveIslandFromStartPosition()
	{
		bridgeState = BridgeStates.MovingToDestination;
		Vector3[] movePositions = new Vector3[2];
		movePositions [0] = transform.position;
		movePositions [1] = moveToHere.position;
		iTween.MoveTo (gameObject, iTween.Hash ("path", movePositions, "time", moveTime, "easetype", easing));
		Debug.Log ("Before Yield");
		yield return new WaitForSeconds (moveTime);
		Debug.Log ("After Yield");
		bridgeState = BridgeStates.AtDestination;
	}

	public IEnumerator ReturnIslandToStartPosition()
	{
		bridgeState = BridgeStates.MovingToStart;
		Vector3[] movePositions = new Vector3[2];
		movePositions [0] = transform.position;
		movePositions [1] = startPosition;
		iTween.MoveTo (gameObject, iTween.Hash ("path", movePositions, "time", moveTime, "easetype", easing));
		Debug.Log ("Before Yield");
		yield return new WaitForSeconds (moveTime);
		Debug.Log ("After Yield");
		bridgeState = BridgeStates.AtStart;
	}

}
