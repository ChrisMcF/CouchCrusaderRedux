using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(DottedLineRenderer))]
public class PathHandler : MonoBehaviour
{
    private string forcedBoss = "";

    //Sets of arrays to hold both the sorted and unsorted array for each section of the path
    public List<Transform> unShuffledNodes;
	public List<Transform> shuffledNodes;
	public List<Transform> transformPath;

	//Variable which controls the time for the movement to complete
	public int time;
	//Variable to control the type of movement curve
	public iTween.EaseType curve;
	//variable which controls the seed of the random path
	public int seed;
	//variable which controls the time before the line moves
	public int timeBeforeMove;
	//variable which controls the time before the next scnene loads
	public int timeBeforeSceneLoad;
	//variable which counts how many scenes it has travelled to
	public int _sceneCount = 0;
	//Holds the data of the Game Controller Object
	public List<string> _bossList;
	public List<Transform> bossTransform;
	public string lastBossDefeated;

	private bool fade = true;
	
	// Use this for initialization
	void Start ()
	{
		Invoke ("DoEverything", timeBeforeMove);
		if (GameController.gameController.bossesDefeated != 0) {
			_sceneCount = GameController.gameController.bossesDefeated;
		}
		if (GameController.gameController.bossList != null && GameController.gameController.bossList.Count != 0) {
			_bossList = GameController.gameController.bossList;
		}
		if (GameController.gameController.BossPosition != null && GameController.gameController.BossPosition.Count != 0) {
			bossTransform.Clear ();
			for (int i = 0; i < GameController.gameController.BossPosition.Count; i++) {
				GameObject GO = Instantiate (new GameObject (), GameController.gameController.BossPosition [i], Quaternion.identity) as GameObject;
				bossTransform.Add (GO.transform);
			}
		}
		if (GameController.gameController.NodeArray != null && GameController.gameController.NodeArray.Count != 0) {
			unShuffledNodes.Clear ();
			for (int i = 0; i < GameController.gameController.NodeArray.Count; i++) {
				GameObject GO = Instantiate (new GameObject (), GameController.gameController.NodeArray [i], Quaternion.identity) as GameObject;
				unShuffledNodes.Add (GO.transform);
			}
		}
	}

	//Draw Gizmos onto the scene to display the path that the object will take
	public void OnDrawGizmos ()
	{
		//Once again check if there are transforms in both arrays
		if (shuffledNodes != null && unShuffledNodes != null) {
			//Draw the gizmo onto the path
			iTween.DrawPathGizmos (transformPath.ToArray (), Color.blue);
			iTween.DrawPathGizmos (unShuffledNodes.ToArray (), Color.magenta);
		}
	}

	//Randomize Array function
	public List<Transform> RandomizeArray (int seed, List<Transform> _unShuffledNodeArray)
	{
		List<Transform> shuffledNodes = new List<Transform> ();
		//Copy the unShuffled array into the shuffled array
		for (int i = 0; i < (_unShuffledNodeArray.Count / _bossList.Count); i++) {
			shuffledNodes.Add (unShuffledNodes [i]);
		}
		//Declare a new random number using the seed
		System.Random prng = new System.Random (seed);
		//Create a for loop which will shuffle through the array, ingoring the first and last transforms, for the beginning and end points.
		for (int i = 1; i < (_unShuffledNodeArray.Count / _bossList.Count)- 2; i++) {
			//Recieve a new random number between the current number and the second last number
			int randomIndex = prng.Next (i, (_unShuffledNodeArray.Count / _bossList.Count) - 1);
			//Do a Swap
			var tempItem = shuffledNodes [randomIndex];
			shuffledNodes [randomIndex] = shuffledNodes [i];
			shuffledNodes [i] = tempItem;
		}
		Debug.Log (_unShuffledNodeArray.Count / _bossList.Count);
		return shuffledNodes;
	}

	//Randomises the array, and sets the position of the line spawner to the first transform
	public void GetNodes ()
	{
		if (shuffledNodes != null && unShuffledNodes != null) {
			//transformPath = new List<Transform> (unShuffledNodes.Count / _bossList.Count);
			transformPath = RandomizeArray (seed, unShuffledNodes);
			transformPath.Add (bossTransform [0]);
		}
	}

	//Function just generates a random seed
	public void RandomSeed ()
	{
		System.Random rng = new System.Random ((int)System.DateTime.Now.Ticks);
		seed = rng.Next (0, 100000);
	}
	
	public void DoEverything ()
	{
		//Randomises the seed
		RandomSeed ();
		//Randomize bosses
		RandomBoss (seed, _bossList);
		//Call the GetNodes function which Randomises the array and sets the beggining of the path to the first node
		GetNodes ();
		//As long as there are transforms in both arrays, call the next function
		if (shuffledNodes != null && unShuffledNodes != null) {
			//Set the next scene to invoke after the time for the animation to be completed as well as time before the next scene needs to be loaded
			Invoke ("Do_Zoom", time);
			//if (_sceneCount == 0) {
			//set the object to move on the path determined by the variables
			transform.position = unShuffledNodes [0].position;
			iTween.MoveTo (gameObject, iTween.Hash ("path", transformPath.ToArray (), "time", time, "easetype", curve));
			//}
		}
	}

	public List<string> RandomBoss (int seed, List<string> _stringList)
	{
		System.Random prng = new System.Random (seed);
		//Create a for loop which will shuffle through the array, ingoring the first and last transforms, for the beginning and end points.
		for (int i = 0; i < (_stringList.Count); i++) {
			//Recieve a new random number between the current number and the second last number
			int randomIndex = prng.Next (i, _stringList.Count);
			//Do a Swap
			var tempItem = _stringList [randomIndex];
			_stringList [randomIndex] = _stringList [i];
			_stringList [i] = tempItem;
			var tempItem2 = bossTransform [randomIndex];
			bossTransform [randomIndex] = bossTransform [i];
			bossTransform [i] = tempItem2;
		}
		return _stringList;
	}
	
	public void Do_Zoom ()
	{
		SaveToGameController ();
		Camera.main.GetComponent<DollyZoom> ().enabled = true;
		if (fade) {
			fade = false;
			Camera.main.GetComponent<Fade> ().Fadeout (timeBeforeSceneLoad);
		}
		Invoke ("LoadNextScene", timeBeforeSceneLoad);
	}

	public void LoadNextScene ()
	{
        if (forcedBoss != "")
        {
            Application.LoadLevel(forcedBoss);
        }
        else
        {
            Application.LoadLevel(lastBossDefeated);
        }

	}

	public void SaveToGameController ()
	{
		_sceneCount += 1;
		GameController.gameController.bossesDefeated = _sceneCount;

		lastBossDefeated = _bossList [0];
		GameController.gameController.lastBossDefeated = lastBossDefeated;

		GameController.gameController.NodeArray.Clear ();
		unShuffledNodes.RemoveRange (0, (unShuffledNodes.Count / _bossList.Count) - 1);
		for (int i = 0; i < unShuffledNodes.Count; i++) {
			GameController.gameController.NodeArray.Add (unShuffledNodes [i].position);
		}

		_bossList.RemoveRange (0, 1);
		GameController.gameController.bossList = _bossList;
		
		GameController.gameController.NodeArray.Insert (0, bossTransform [0].position);
		GameController.gameController.BossPosition.Clear ();
		bossTransform.RemoveRange (0, 1);
		for (int i = 0; i < bossTransform.Count; i++) {
			GameController.gameController.BossPosition.Add (bossTransform [i].position);
		}
	}

    void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            forcedBoss = "Scylla";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            forcedBoss = "Spewnicorn";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            forcedBoss = "Abra Cadaver";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            forcedBoss = "Velestine_2";
        }

        if (forcedBoss != "")
            Debug.Log("ForcedBoss = " + forcedBoss);
#endif

    }
}
