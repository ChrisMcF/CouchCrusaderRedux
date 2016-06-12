using UnityEngine;
using System.Collections;

public class IslandTrigger : MonoBehaviour
{
	public MoveIslandBridge bridge; 
	private int playersOnBridge = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			playersOnBridge += 1;
			if (bridge.bridgeState == MoveIslandBridge.BridgeStates.AtStart || bridge.bridgeState == MoveIslandBridge.BridgeStates.MovingToStart) {
				bridge.StopCoroutine ("ReturnIslandToStartPosition");// Stop Lower
				bridge.StartCoroutine ("MoveIslandFromStartPosition");//Raise
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Player") {
			playersOnBridge -= 1;
			if (playersOnBridge == 0 && bridge.bridgeState == MoveIslandBridge.BridgeStates.AtDestination || bridge.bridgeState == MoveIslandBridge.BridgeStates.MovingToDestination) {
				StopCoroutine ("MoveIslandFromStartPosition");//Stop raise
				bridge.StartCoroutine ("ReturnIslandToStartPosition");//Lower
			}
		}
	}
}
