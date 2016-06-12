using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour
{

    [System.Serializable]
    public struct Axes
    {
        public bool x;
        public bool y;
        public bool z;
    }

    public Axes axes;

    public bool stopAfterTime;

    private Quaternion originalRotation;
    public float rotationSpeedMin;
    public float rotationSpeedMax;
    private float rotationSpeed;
    public float maxSpinTime;

    

    private float timer;

    private bool stopped;
    private bool startSlowing;


    // Use this for initialization
    void Start ()
    {
        originalRotation = transform.rotation;

        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
        timer = maxSpinTime;
        float randomTurn = Random.Range(0f, 1f);
        if (randomTurn > 0.5f)
        {
            rotationSpeed = -rotationSpeed;
        }
            
        
	}

    void Update()
    {
        if (stopped)
            return;

        if (timer > 0 || !stopAfterTime)
        {
            timer -= Time.deltaTime;
            float xRot = 0;
            float yRot = 0;
            float zRot = 0;

            if (axes.x)
                xRot = rotationSpeed * Time.deltaTime;
            if (axes.y)
                yRot = rotationSpeed * Time.deltaTime;
            if (axes.z)
                zRot = rotationSpeed * Time.deltaTime;

            transform.Rotate(xRot, yRot, zRot);
            return;
        }

        if (timer <= 0)
        {
            float xRot = 0;
            float yRot = 0;
            float zRot = 0;

            if (axes.x)
                xRot = rotationSpeed * Time.deltaTime;
            if (axes.y)
                yRot = rotationSpeed * Time.deltaTime;
            if (axes.z)
                zRot = rotationSpeed * Time.deltaTime;

            transform.Rotate(xRot, yRot, zRot);

            float angle = Quaternion.Angle(originalRotation, transform.rotation);

            if (angle < 0.2f)
            {
                transform.rotation = originalRotation;
                stopped = true;
            }

        }
    } 

   

        
    }
