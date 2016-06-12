using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class TronTrailSection
{
	public Vector3 point;
    public Vector3 upDir;
    public float time;
    public TronTrailSection() {

    }
    public TronTrailSection(Vector3 p, float t) {
        point = p;
        time = t;
    }
}
[RequireComponent(typeof(MeshFilter))]

public class WeaponTrail : MonoBehaviour {

    #region Public
    public float height = 2.0f;
    public float time = 2.0f;
    public bool alwaysUp = false;
    public float minDistance = 0.1f;  
	public float timeTransitionSpeed = 1f;
    public float desiredTime = 2.0f;
    public Color startColor = Color.white;
    public Color endColor = new Color(1, 1, 1, 0);
    #endregion
    
    #region Temporary
    Vector3 position;
    float now = 0;
    TronTrailSection currentSection;
    Matrix4x4 localSpaceTransform;
    #endregion

    #region Internal
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;
    private Vector2[] uv;
    #endregion

    #region Customisers 
    private MeshRenderer meshRenderer;
    private Material trailMaterial;
    #endregion

    private List<TronTrailSection> sections = new List<TronTrailSection>();

    void Awake() {

        MeshFilter meshF = GetComponent(typeof(MeshFilter)) as MeshFilter;
        mesh = meshF.mesh;
        meshRenderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        trailMaterial = meshRenderer.material;

    }
    public void StartTrail(float timeToTweenTo, float fadeInTime){		
		desiredTime = timeToTweenTo;
		if (time != desiredTime){
			timeTransitionSpeed = Mathf.Abs(desiredTime -time) / fadeInTime;
		}
		if (time <= 0){
			time = 0.01f;
		}
    }
    public void SetTime(float trailTime, float timeToTweenTo, float tweenSpeed) {
		time = trailTime;
		desiredTime = timeToTweenTo;
		timeTransitionSpeed = tweenSpeed;
		if (time <= 0){
			ClearTrail();
		}
    }
	public void FadeOut(float fadeTime){
		desiredTime = 0;
		if (time >0){
			timeTransitionSpeed = time / fadeTime;
		}
	}
    public void SetTrailColor(Color color){
        trailMaterial.SetColor("_TintColor", color);
    }
    public void Itterate(float itterateTime) { 
   
        position = transform.position;
        now = itterateTime;
		
        if (sections.Count == 0 || (sections[0].point - position).sqrMagnitude > minDistance * minDistance) {
            TronTrailSection section = new TronTrailSection();
            section.point = position;
            if (alwaysUp)
                section.upDir = Vector3.up;
            else
                section.upDir = transform.TransformDirection(Vector3.up);
             
            section.time = now;
            sections.Insert(0, section);
            
        }
    }
    public void UpdateTrail(float currentTime, float deltaTime) { 

        mesh.Clear();

        while (sections.Count > 0 && currentTime > sections[sections.Count - 1].time + time) {
            sections.RemoveAt(sections.Count - 1);
        }
    
        if (sections.Count < 2)
            return;

        vertices = new Vector3[sections.Count * 2];
        colors = new Color[sections.Count * 2];
        uv = new Vector2[sections.Count * 2];
        currentSection = sections[0];
        localSpaceTransform = transform.worldToLocalMatrix;


        for (var i = 0; i < sections.Count; i++) {
	
            currentSection = sections[i];

            float u = 0.0f;
            if (i != 0)
                u = Mathf.Clamp01((currentTime - currentSection.time) / time);
            Vector3 upDir = currentSection.upDir;


            vertices[i * 2 + 0] = localSpaceTransform.MultiplyPoint(currentSection.point);
            vertices[i * 2 + 1] = localSpaceTransform.MultiplyPoint(currentSection.point + upDir * height);
            uv[i * 2 + 0] = new Vector2(u, 0);
            uv[i * 2 + 1] = new Vector2(u, 1);

            Color interpolatedColor = Color.Lerp(startColor, endColor, u);
            colors[i * 2 + 0] = interpolatedColor;
            colors[i * 2 + 1] = interpolatedColor;
        }
		
        int[] triangles = new int[(sections.Count - 1) * 2 * 3];
        for (int i = 0; i < triangles.Length / 6; i++) {
            triangles[i * 6 + 0] = i * 2;
            triangles[i * 6 + 1] = i * 2 + 1;
            triangles[i * 6 + 2] = i * 2 + 2;

            triangles[i * 6 + 3] = i * 2 + 2;
            triangles[i * 6 + 4] = i * 2 + 1;
            triangles[i * 6 + 5] = i * 2 + 3;
        }
		
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.uv = uv;
        mesh.triangles = triangles;

        if (time > desiredTime){
			time -= deltaTime*timeTransitionSpeed;
			if(time <= desiredTime) time = desiredTime;
        } else if (time < desiredTime){
			time += deltaTime*timeTransitionSpeed;
			if(time >= desiredTime) time = desiredTime;
        }
    }
    public void ClearTrail() {
		desiredTime = 0;
		time = 0;
        if (mesh != null) {
            mesh.Clear();
            sections.Clear();
        }
    }
}

