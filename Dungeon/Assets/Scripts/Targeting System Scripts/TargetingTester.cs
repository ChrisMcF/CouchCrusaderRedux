using UnityEngine;
using System.Collections;

public class TargetingTester : MonoBehaviour {

	private TargetingClass _targetingClass;

	public Vector3 testPosition = new Vector3(0, 0.05f, 0);
	public Color testColor;
	public Vector2 testRectSize = new Vector2(1, 2);
	public float testCircleRadius = 1;
	public float testTimeActive = 1;

	public Vector4 testTrapezoidVertices;
	public float testScale;

	private GameObject _testInstance;

	// Use this for initialization
	void Start () {
		_targetingClass = GameObject.Find ("TargetingClass").GetComponent<TargetingClass> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("r")){			//Test creating a rectangle target by pressing 'r'
			_targetingClass.CreateTargetRect (testPosition, testRectSize, testColor, testTimeActive);
		}
		if (Input.GetKeyDown ("f")) {		//Test creating a circle target by pressing 'f'
			_targetingClass.CreateTargetCircle (testPosition, testCircleRadius, testColor, testTimeActive);
		}
		if (Input.GetKeyDown ("v")) {
			_targetingClass.CreateTargetTrapezoid(testTrapezoidVertices, testPosition, testScale, testColor, testTimeActive);
		}
		if (Input.GetKeyDown ("space")) {	//Test creating an instanced rectangle target by pressing 'space'
			_testInstance = _targetingClass.CreateTargetRect(testPosition, testRectSize, testColor, testTimeActive);
		}

		//Input for Left and Right movement of instanced rectangle target
		if (Input.GetAxis ("Horizontal") > 0 || Input.GetAxis ("Horizontal") < 0) {
			_testInstance.transform.Translate (Vector3.right*Input.GetAxis ("Horizontal")*Time.deltaTime);
		}
	}
}
