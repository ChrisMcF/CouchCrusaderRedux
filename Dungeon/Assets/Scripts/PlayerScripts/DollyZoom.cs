using UnityEngine;
using System.Collections;

public class DollyZoom : MonoBehaviour
{
	private Camera cam;
	public float endFOV;
	public Transform endPosition;
	public float zoomTime;
	public bool pingPong = false;
	public enum interpolations
	{
		EaseOut,
		Linear,
		EaseIn,
		EaseInOut
	} 
	public interpolations interpolation;
	public float delayPingPong = 1f;
	void Start ()
	{
        cam = GetComponent<Camera> ();
		StartCoroutine ("BeginDollyZoom");
	}

	void OnAwake ()
	{
        cam = GetComponent<Camera> ();
		StartCoroutine ("BeginDollyZoom");
	}

	void Update ()
	{
		//If zero then donot pingpong
		if (pingPong) {
			StartCoroutine ("BeginDollyZoom");
		}
	}

	IEnumerator BeginDollyZoom ()
	{
		bool wasPingPong = pingPong;// a bool to store whether the pingpong is active
		pingPong = false;
		Vector3 startPoint = transform.position;
		float startFOV = cam.fieldOfView;
		float counter = zoomTime;
		float linearInterpolationValue, highestValue = 0, lowestValue = 1, interpolationValue = 0;
		while (counter>0) {
			//Debug.Log ("Linear = " + counter/zoomTime + ", 2power = " + Mathf.Pow(counter/zoomTime, 2));
			linearInterpolationValue = counter / zoomTime;
			switch (interpolation) {
			case interpolations.Linear:
				interpolationValue = linearInterpolationValue;
				break;
			case interpolations.EaseOut:
				interpolationValue = Mathf.Pow (linearInterpolationValue, 2);
				break;
			case interpolations.EaseIn:
				//interpolationValue = (Mathf.Pow (linearInterpolationValue - 0.5f, 2)/25f * 100f);
				interpolationValue = Mathf.Sqrt (linearInterpolationValue);
				break;
			case interpolations.EaseInOut:
				if (linearInterpolationValue < 0.5)
					interpolationValue = Mathf.Pow (linearInterpolationValue, 2) * 2;
				else
					interpolationValue = ((Mathf.Sqrt (linearInterpolationValue) - 0.7f) / 30 * 50 + 0.5f);
				break;
			}
			
			counter -= Time.deltaTime;

			if (interpolationValue < lowestValue)
				lowestValue = interpolationValue;

			if (interpolationValue > highestValue)
				highestValue = interpolationValue;

			//Debug
			if (linearInterpolationValue < 0.5) {
				//interpolationValue = Mathf.Pow(linearInterpolationValue, 2);
				//Debug.Log ("largest less than half value = " + highestValue + ", Smallest less than half value = " + lowestValue);
			} else {
				//interpolationValue = Mathf.Sqrt(linearInterpolationValue);
				//Debug.Log ("largest greater than half value = " + highestValue + ", Smallest greater than half value = " + lowestValue);
			}

			if (interpolationValue != 0) {
				transform.position = Vector3.Lerp (startPoint, endPosition.position, 1 - interpolationValue);
                cam.fieldOfView = interpolationValue * (startFOV - endFOV) + endFOV;
			}
			yield return null;
		}

		
		//Debug.Log ("lowestValue value = " + lowestValue + ", Highest value = " + highestValue);
		//Set the existing varibles to allow for zoom out to the same spot
		endPosition.position = startPoint;
		endFOV = startFOV;
		yield return new WaitForSeconds (delayPingPong);
		if (wasPingPong)
			pingPong = true;
	}
}
