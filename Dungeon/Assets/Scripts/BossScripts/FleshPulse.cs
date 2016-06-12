using UnityEngine;
using System.Collections;

public class FleshPulse : MonoBehaviour
{

    private MeshRenderer rend;
    ProceduralMaterial mat;


    void Start()
    {
        rend = GetComponent<MeshRenderer>();

        
        mat = (ProceduralMaterial)rend.material;
    }

    public void SetNerves(float val)
    {
        mat.SetProceduralFloat("Nerves", val);
        mat.RebuildTextures();
    }
}
