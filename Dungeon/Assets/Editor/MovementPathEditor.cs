using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(PathHandler))]
public class MovementPathEditor : Editor
{ 

	public SerializedProperty
		timeBefMove,
		timeAftMove,
		timeForMove;

	void OnEnable ()
	{
		timeBefMove = serializedObject.FindProperty ("timeBeforeMove");
		timeAftMove = serializedObject.FindProperty ("timeBeforeSceneLoad");
		timeForMove = serializedObject.FindProperty ("time");
	}

	public override void OnInspectorGUI ()
	{
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		serializedObject.Update ();


		//Declares the three Labels and tooltips to be displayed in the custom inspector
		GUIContent NodesToolTip1 = new GUIContent ("Path Nodes", "These are used to define the points which the path will cross. Make sure the first and last points are where you want the path to begin and end");
		GUIContent TimeBefToolTip = new GUIContent ("Time Before Move", "This controls the amount of time in seconds, which determines how long to wait before the path renderer moves");
		GUIContent TimeAftToolTip = new GUIContent ("Time Before Load", "This controls the amount of time in seconds to wait until the next scene has loaded when the path is finished.");
		GUIContent BossListToolTip = new GUIContent ("Boss List", "List of bosses which can be chosen from, also acts as the name of the scene the boss resides in");
		GUIContent BossTrListToolTip = new GUIContent ("Boss Transform", "transforms for the bosses in relation to the name list");
		GUIContent SeedToolTip = new GUIContent ("Seed", "This is the seed which will define the random path of the node. If there is a path you like, recording the number and inputing it again will yield the same path.");
		GUIContent TimeToolTip = new GUIContent ("Movement Time", "This variable affects the amount of seconds the path will need to complete.");
		GUIContent CurveToolType = new GUIContent ("Movement Curve", "Will change the type of easing that the path will take during its movement");
		
		//Get access to the script which holds the info
		PathHandler myScript = (PathHandler)target;
		
		if (myScript.unShuffledNodes [0] != null) {
			myScript.gameObject.transform.position = myScript.unShuffledNodes [0].position;
		}
		myScript.GetNodes ();

		//Always refresh the scene view so the Gizmo path is updated on change of values
		SceneView.RepaintAll ();

		//Controls and Displays the UnShuffled1 nodes so nodes can be added or removed
		SerializedProperty tps = serializedObject.FindProperty ("unShuffledNodes");
		//Begin the check for a change in the section
		EditorGUI.BeginChangeCheck ();
		//Display the node
		EditorGUILayout.PropertyField (tps, NodesToolTip1, true);
		//Stop checking for change
		if (EditorGUI.EndChangeCheck ())
				//Change variables if anything was modified
			serializedObject.ApplyModifiedProperties ();
		EditorGUIUtility.LookLikeControls ();

		//Controls and Displays the UnShuffled1 nodes so nodes can be added or removed
		SerializedProperty tps1 = serializedObject.FindProperty ("transformPath");
		//Begin the check for a change in the section
		EditorGUI.BeginChangeCheck ();
		//Display the node
		EditorGUILayout.PropertyField (tps1, NodesToolTip1, true);
		//Stop checking for change
		if (EditorGUI.EndChangeCheck ())
			//Change variables if anything was modified
			serializedObject.ApplyModifiedProperties ();
		EditorGUIUtility.LookLikeControls ();

		//Controls and Displays the Enum for the path types of iTween, Works similar to above
		SerializedProperty curveType = serializedObject.FindProperty ("curve");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (curveType, CurveToolType, true);
		if (EditorGUI.EndChangeCheck ())
			serializedObject.ApplyModifiedProperties ();
		EditorGUIUtility.LookLikeControls ();

		//Controls and Displays the UnShuffled1 nodes so nodes can be added or removed
		SerializedProperty tpsBoss = serializedObject.FindProperty ("_bossList");
		//Begin the check for a change in the section
		EditorGUI.BeginChangeCheck ();
		//Display the node
		EditorGUILayout.PropertyField (tpsBoss, BossListToolTip, true);
		//Stop checking for change
		if (EditorGUI.EndChangeCheck ())
			//Change variables if anything was modified
			serializedObject.ApplyModifiedProperties ();
		EditorGUIUtility.LookLikeControls ();

		//Controls and Displays the UnShuffled1 nodes so nodes can be added or removed
		SerializedProperty tpsBossTr = serializedObject.FindProperty ("bossTransform");
		//Begin the check for a change in the section
		EditorGUI.BeginChangeCheck ();
		//Display the node
		EditorGUILayout.PropertyField (tpsBossTr, BossTrListToolTip, true);
		//Stop checking for change
		if (EditorGUI.EndChangeCheck ())
			//Change variables if anything was modified
			serializedObject.ApplyModifiedProperties ();
		EditorGUIUtility.LookLikeControls ();

		//Show the timeBeforeMove variable, as well as allow it to be changed using a slider.
		EditorGUILayout.IntSlider (timeBefMove, 0, 5, TimeBefToolTip);
		serializedObject.ApplyModifiedProperties ();

		//Show the timeBeforeSceneLoad variable, as well as allow it to be changed using a slider.
		EditorGUILayout.IntSlider (timeAftMove, 0, 5, TimeAftToolTip);
		serializedObject.ApplyModifiedProperties ();

		//Show the movementTime variable, as well as allow it to be changed using a slider.
		EditorGUILayout.IntSlider (timeForMove, 0, 20, TimeToolTip);
		serializedObject.ApplyModifiedProperties ();

		//Show the seed, and have it be able to be changed
		myScript.seed = EditorGUILayout.IntField (SeedToolTip, myScript.seed);
		serializedObject.ApplyModifiedProperties ();

		//Create a horizontal box to hold two buttons
		GUILayout.BeginHorizontal ("Box");
		//First button will use the seed
		if (GUILayout.Button ("Use Seed")) {
			myScript.GetNodes ();
		}
		//Second button will generate a random see and then use it.
		if (GUILayout.Button ("Use Random Seed")) {
			myScript.RandomSeed ();
			myScript.GetNodes ();
		}
		GUILayout.EndHorizontal ();
		serializedObject.ApplyModifiedProperties ();
	}
}
