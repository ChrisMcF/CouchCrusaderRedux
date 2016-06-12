using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransparencyController : MonoBehaviour {
    
    private List<Renderer> transparentRenderers;
    [Tooltip("The layer containing all objects that may obstruct the view of players")]
    public LayerMask transparencyLayer;
    [Range(0,1)]
    public float transparencyLevel;
    [Tooltip("How often to check for obstacles ")]
    public float loopTimer = 0.3f;


    // Used Shaders
    private Shader transparentLegacy;
    private Shader diffuseLegacy;

    void Start ()
    {

        transparentRenderers = new List<Renderer>();

        transparentLegacy = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        diffuseLegacy = Shader.Find("Legacy Shaders/Diffuse");


        InvokeRepeating("CheckForObstacles", loopTimer, loopTimer);
    }
	
	
    void CheckForObstacles()
    {
        RestoreAlpha();

        for (int i = GameController.gameController.players.Count - 1; i >= 0; i--)
        {
            Player player = GameController.gameController.players[i];
            Vector3 dir = (player.playerObject.transform.position + new Vector3(0,1,0)) - Camera.main.transform.position;

            //Debug.DrawRay(Camera.main.transform.position, dir * 100f, Color.red, 0.5f);

            float distance = Vector3.Distance((player.playerObject.transform.position + new Vector3(0, 1, 0)), Camera.main.transform.position) - 1f;


            RaycastHit[] rayHits;
            rayHits = Physics.SphereCastAll(Camera.main.transform.position, 1f, dir, distance, transparencyLayer);
            //rayHits = Physics.RaycastAll(Camera.main.transform.position, dir, distance, transparencyLayer);

            foreach (RaycastHit rayHit in rayHits)
            {
                Renderer renderer = rayHit.collider.gameObject.GetComponent<Renderer>();
                transparentRenderers.Add(renderer);
            }
            if (transparentRenderers.Count > 0 )
                AdjustAlpha(transparentRenderers);
        }
    }

    void RestoreAlpha()
    {
        if (transparentRenderers.Count < 1)
            return;


        foreach (Renderer r in transparentRenderers)
        {
            foreach (Material material in r.materials)
            {

                Color c = material.color;
                c.a = 1;
                material.color = c;

                if (r.material.shader.name == "Standard")
                {
                    material.SetFloat("_Mode", 0);
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                }
                else if (r.material.shader.name == "Legacy Shaders/Transparent/Diffuse")
                {
                   material.shader = diffuseLegacy;
                }
            }

        }
        transparentRenderers.Clear();
    }

   void AdjustAlpha(List<Renderer> rends)
    {
        foreach (Renderer r in rends)
        {
            foreach (Material material in r.materials)
            {
                if (material.shader.name == "Standard")
                {
                    material.SetFloat("_Mode", 3);
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                }
                else if (r.material.shader.name == "Legacy Shaders/Diffuse")
                {
                    material.shader = transparentLegacy;
                }

                Color c = material.color;
                c.a = transparencyLevel - (0.02f * transparentRenderers.Count);
                material.color = c;
            }
        }
        
        
    }

}

