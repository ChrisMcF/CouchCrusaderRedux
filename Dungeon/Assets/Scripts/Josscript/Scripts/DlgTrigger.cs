using UnityEngine;
using System.Collections;

public class DlgTrigger : MonoBehaviour {
	public bool used;
	public int index;

	public void OnTriggerEnter(Collider col)
	{
		if(!used)
		{
			if (col.CompareTag ("Player")) 
			{
				Camera.main.BroadcastMessage ("StartDialogue", index);
				
				used = true;
			}
		}
	}
	//public void OnTriggerExit(Collider col)
	//{

	//	col.GetComponent<DlgGUI> ().deactivateMesh();
	// }
}
