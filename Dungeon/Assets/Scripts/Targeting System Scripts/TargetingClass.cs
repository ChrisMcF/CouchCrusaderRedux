using UnityEngine;
using System.Collections;

public class TargetingClass : MonoBehaviour {

	public GameObject targetPlane;		//Prefab of a plane (with rectMaterial as the default material
	public Material rectMaterial;		//Material used for creating a rectangle target (CreateTargetRect())
	public Material circleMaterial;		//Material used for creating a circle target (CreateTargetCircle())
										//NOTE: Both materials' Rendering Mode must be set to 'Fade'
	[Range(0.0f, 1.0f)]
	public float startAlpha = 0.3f;			//Starting alpha of the Target Plane.	Default 0.3
	[Range(0.0f, 1.0f)]						//	The animation loops between these two alphas
	public float endAlpha = 0.6f;			//Ending alpha of the Target Plane.		Default 0.6
	public float alphaLerpTime = 1.0f;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	// TargetingClass Functions



	//Takes parameters: Position, Size, Color, Time Active (if _timeActive = 0, doesn't destroy). Returns the instanced GameObject
	public GameObject CreateTargetRect (Vector3 _pos, Vector2 _size, Color _color, float _timeActive) {

		//Assigns the newly created instance of the prefab to a GameObject variable
		GameObject _targetRect = (GameObject)Instantiate (targetPlane, _pos, transform.rotation);

		if (_timeActive > 0) {
			//Destroys the instanced GameObject after '_timeActive' seconds
			Destroy (_targetRect, _timeActive);
		}

		//Sets the instanced GameObject's material to the Target Rect material
		_targetRect.GetComponent<Renderer> ().material = rectMaterial;

		//Sets the instanced GameObject's color to the specified color, but keeps the alpha of public float 'alpha'
		_targetRect.GetComponent<Renderer> ().material.color = new Color(_color.r, _color.g, _color.b, startAlpha);

		//Sets the instanced GameObject's local scale to the size specified
		_targetRect.transform.localScale = new Vector3(_size.x*0.1f, 1, _size.y*0.1f);

		//Animates the alpha of the instanced GameObject from 'startAlpha' to 'endAlpha'
		iTween.ColorTo (_targetRect, iTween.Hash ("a", endAlpha, "time", alphaLerpTime, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.pingPong));

		return _targetRect;
	}



	//Takes parameters: Position, Radius, Color, Time Active (if _timeActive = 0, doesn't destroy). Returns the instanced GameObject
	public GameObject CreateTargetCircle (Vector3 _pos, float _radius, Color _color, float _timeActive) {

		//Assigns the newly created instance of the prefab to a GameObject variable
		GameObject _targetCircle = (GameObject)Instantiate (targetPlane, _pos, transform.rotation);

		if (_timeActive > 0) {
			//Destroys the instanced GameObject after '_timeActive' seconds
			Destroy (_targetCircle, _timeActive);
		}

		//Sets the instanced GameObject's material to the Target Circle material
		_targetCircle.GetComponent<Renderer> ().material = circleMaterial;

		//Sets the instanced GameObject's color to the specified color, but keeps the alpha of public float 'alpha'
		_targetCircle.GetComponent<Renderer> ().material.color = new Color(_color.r, _color.g, _color.b, startAlpha);

		//Sets the instanced GameObject's local scale to the radius specified
		_targetCircle.transform.localScale = new Vector3(_radius*0.1f, 1, _radius*0.1f);

		//Animates the alpha of the instanced GameObject from 'startAlpha' to 'endAlpha'
		iTween.ColorTo (_targetCircle, iTween.Hash ("a", endAlpha, "time", alphaLerpTime, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.pingPong));

		return _targetCircle;
	}



	//
	public GameObject CreateTargetPolygon(){

		Mesh _mesh = new Mesh ();

		Vector3[] _verts = new Vector3[4];
		Vector2[] _uvs = new Vector2[4];
		int[] _tris = new int[6] {0, 1, 2, 2, 1, 3};

		_verts [0] = -Vector3.right + Vector3.forward;
		_verts [1] = Vector3.right + Vector3.forward;
		_verts [2] = -Vector3.right - Vector3.forward;
		_verts [3] = Vector3.right - Vector3.forward;

		_uvs [0] = new Vector2 (0.0f, 1.0f);
		_uvs [1] = new Vector2 (1.0f, 1.0f);
		_uvs [2] = new Vector2 (0.0f, 0.0f);
		_uvs [3] = new Vector2 (1.0f, 0.0f);

		_mesh.vertices = _verts;
		_mesh.triangles = _tris;
		_mesh.uv = _uvs;

		GameObject _targetPoly = new GameObject ();
		_targetPoly.AddComponent<MeshFilter> ().mesh = _mesh;
		_targetPoly.AddComponent<MeshRenderer> ();

		return _targetPoly;

	}


	
	//First parameter: Position of vertices on a 2x2 grid from -1 to 1 (see below). The vertices are plotted along the horizontal axis.

	/*
	 *                1
	 *  X_____________|_____________W
	 *   \            |            /
	 *    \           |           /		Defined by 'new Vector4(-1, -0.3, 0.3, 1)
	 *     \          |          /                               X    Y    Z   W
	 *      \         |         /
	 * -1-------------0------------1
	 *        \       |       /
	 *         \      |      /
	 *          \     |     /
	 *           \____|____/
	 *           Y    |    Z
	 *               -1
	 */

	//Other parameters: Position, Local Scale, Color, Time Active
	public GameObject CreateTargetTrapezoid(Vector4 _vertices, Vector3 _pos, float _scale, Color _color, float _timeActive){
		
		Mesh _mesh = new Mesh ();
		
		Vector3[] _verts = new Vector3[6];
		Vector2[] _uvs = new Vector2[6];
		int[] _tris = new int[12] {0, 1, 2, 1, 3, 2, 2, 3, 4, 3, 5, 4};
		
		_verts [0] = _vertices.x*Vector3.right + Vector3.forward;
		_verts [1] = _vertices.y*Vector3.right + Vector3.forward;
		_verts [2] = _vertices.y*Vector3.right - Vector3.forward;
		_verts [3] = _vertices.z*Vector3.right + Vector3.forward;
		_verts [4] = _vertices.z*Vector3.right - Vector3.forward;
		_verts [5] = _vertices.w*Vector3.right + Vector3.forward;
		
//		_uvs [0] = new Vector2 (_vertices.x, 1.0f);
//		_uvs [1] = new Vector2 (_vertices.y, 1.0f);
//		_uvs [2] = new Vector2 (_vertices.y, 0.0f);
//		_uvs [3] = new Vector2 (_vertices.z, 1.0f);
//		_uvs [4] = new Vector2 (_vertices.z, 0.0f);
//		_uvs [5] = new Vector2 (_vertices.w, 1.0f);
		
		_mesh.vertices = _verts;
		_mesh.triangles = _tris;
		_mesh.uv = _uvs;
		
		GameObject _targetTrapezoid = new GameObject ();
		_targetTrapezoid.AddComponent<MeshFilter> ().mesh = _mesh;
		_targetTrapezoid.AddComponent<MeshRenderer> ();

		_targetTrapezoid.transform.position = _pos;

		if (_timeActive > 0) {
			//Destroys the instanced GameObject after '_timeActive' seconds
			Destroy (_targetTrapezoid, _timeActive);
		}
		
		//Sets the instanced GameObject's material to the Target Circle material
		_targetTrapezoid.GetComponent<Renderer> ().material = rectMaterial;
		
		//Sets the instanced GameObject's color to the specified color, but keeps the alpha of public float 'alpha'
		_targetTrapezoid.GetComponent<Renderer> ().material.color = new Color(_color.r, _color.g, _color.b, startAlpha);
		
		//Sets the instanced GameObject's local scale to the radius specified
		_targetTrapezoid.transform.localScale = new Vector3(_scale, 1, _scale);
		
		//Animates the alpha of the instanced GameObject from 'startAlpha' to 'endAlpha'
		iTween.ColorTo (_targetTrapezoid, iTween.Hash ("a", endAlpha, "time", alphaLerpTime, "easetype", iTween.EaseType.easeInOutSine, "looptype", iTween.LoopType.pingPong));
		
		return _targetTrapezoid;
		
	}
}
