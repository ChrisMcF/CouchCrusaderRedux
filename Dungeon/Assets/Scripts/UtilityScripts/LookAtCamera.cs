using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour
{
	private Transform cameraTransform;
	public Vector3 upDirection = new Vector3(0,0,1);
	void Start()
	{
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
	}

	void Update()
	{
		transform.LookAt(cameraTransform,upDirection);
	}

}