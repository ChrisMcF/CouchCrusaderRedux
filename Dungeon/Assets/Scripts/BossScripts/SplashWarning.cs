using UnityEngine;
using System.Collections;

public class SplashWarning : MonoBehaviour {
	[Range(0,1)]
	public float startAlpha = 0, middleAlpha = 1, endAlpha = 0;
	
	private Material splashWarningMaterial;

	// Use this for initialization
	void Start () {
		splashWarningMaterial = GetComponent<Renderer> ().materials[0];
		//StartCoroutine ("WarnPlayersAboutSplash", 3f);
	}

	public IEnumerator WarnPlayersAboutSplash(float warningTime)
	{
		float halfTime = warningTime / 2;
		StartCoroutine(LerpAlpha(true, halfTime));
		yield return new WaitForSeconds(halfTime);
		Debug.Log ("Half Time");
		StartCoroutine(LerpAlpha(false, halfTime));
		yield return new WaitForSeconds(halfTime);
		Debug.Log ("Full Time");
	}

	IEnumerator LerpAlpha(bool lerpingIn, float lerpTime)
	{
		Color startColor = new Color (splashWarningMaterial.GetColor ("_TintColor").r, splashWarningMaterial.GetColor ("_TintColor").g, splashWarningMaterial.GetColor ("_TintColor").b, startAlpha);
		Color middleColor = new Color (splashWarningMaterial.GetColor ("_TintColor").r, splashWarningMaterial.GetColor ("_TintColor").g, splashWarningMaterial.GetColor ("_TintColor").b, middleAlpha);
		Color endColor = new Color (splashWarningMaterial.GetColor ("_TintColor").r, splashWarningMaterial.GetColor ("_TintColor").g, splashWarningMaterial.GetColor ("_TintColor").b, endAlpha);
		float counter = 0f;
		while (counter <lerpTime) 
		{
			if(lerpingIn)
				//splashWarningMaterial.SetColor("_TintColor",  Color.Lerp (splashWarningMaterial.GetColor("_TintColor").r, splashWarningMaterial.GetColor("_TintColor").g, splashWarningMaterial.GetColor("_TintColor").b, Mathf.Lerp (startAlpha, middleAlpha, counter / lerpTime)));
				splashWarningMaterial.SetColor("_TintColor", Color.Lerp(startColor, middleColor, counter/lerpTime));
			else
				splashWarningMaterial.SetColor("_TintColor", Color.Lerp(middleColor, endColor, counter/lerpTime));
				//splashWarningMaterial.SetColor("_TintColor", new Color (splashWarningMaterial.GetColor("_TintColor").r, splashWarningMaterial.GetColor("_TintColor").g, splashWarningMaterial.GetColor("_TintColor").b, Mathf.Lerp (middleAlpha, endAlpha, counter / lerpTime)));
			counter+=Time.deltaTime;
			yield return null;
		}
	}


}
