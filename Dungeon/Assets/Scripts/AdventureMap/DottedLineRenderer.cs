using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PathHandler))]
public class DottedLineRenderer : MonoBehaviour
{

	//Public GameObject to hold the gameObject to be instantiated.
	public GameObject spriteObj;
	//Public GameObject which will be used to contain all the sprites.
	public GameObject spriteHolder;

	private GameObject instantiateObj;
	private Sprite _sprite;
	private Vector3 _oldPos;
	private Vector2 _spriteSize;
	private float _spriteRot;
	private bool _createStep;
	//Holds the data of the Game Controller Object



	// Use this for initialization
	void Start ()
	{
		//Get the sprite component of the gameobject
		_sprite = spriteObj.GetComponent<SpriteRenderer> ().sprite;
		//Store the size of the Sprite as a vector 2
		_spriteSize.x = _sprite.bounds.size.x * spriteObj.transform.localScale.x;
		_spriteSize.y = _sprite.bounds.size.y * spriteObj.transform.localScale.y;
		//Allow the step to be created.
		_createStep = true;
		if (GameController.gameController.linePosition != null && GameController.gameController.lineRotation != null) {
			for (int i = 0; i < GameController.gameController.linePosition.Count-1; i++) {
				instantiateObj = Instantiate (spriteObj, GameController.gameController.linePosition [i], GameController.gameController.lineRotation [i]) as GameObject;
				//Parents the instantiated object to the correct empty GameObject
				instantiateObj.transform.SetParent (spriteHolder.transform);
			}
		}
		//Create the first step
		CreateStep ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		//Call the function to check the distance between the current point and prior sprite
		//Then set the bool to the return.
		_createStep = DistanceCheck ();

		//Call the Create Step function to create new step
		CreateStep ();
	}


	void CreateStep ()
	{
		//Check if a new step is able to be created
		if (_createStep) {
			//Calculate the angle between the last object and the current position of the empty gameobject in 2D Space
			_spriteRot = Mathf.Atan2 (transform.position.z - _oldPos.z, transform.position.x - _oldPos.x) * 180 / Mathf.PI;
			//Instantiate the sprite as a gameobject and store it.
			instantiateObj = Instantiate (spriteObj, transform.position, Quaternion.Euler (new Vector3 (90, -_spriteRot, 0))) as GameObject;
			//Set the old position variable to the correct point on the sprite
			_oldPos = instantiateObj.transform.Find ("OldPos").transform.position;
			//Parents the instantiated object to the correct empty GameObject
			instantiateObj.transform.SetParent (spriteHolder.transform);
			GameController.gameController.linePosition.Add (instantiateObj.transform.position);
			GameController.gameController.lineRotation.Add (instantiateObj.transform.rotation);
		}
	}


	bool DistanceCheck ()
	{
		//Check if there is a step already instantiated to avoid runtime errors
		if (instantiateObj != null) {
			//Get the distance between the current position of the gameobject and the last spawned object, and compare it to the sprites size
			if (Vector3.Distance (instantiateObj.transform.position, transform.position) > _spriteSize.x) {
				//Return true
				return true;
			}
		}
		//if either condition is null, return false
		return false;
	}
}





















