using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float beltSpeed = 1;
    public Material beltMaterial;
    public float wheelSpeed = 100;
    public List<Transform> wheels;
    SurfaceEffector2D _surfaceEffector;

    void Start()
    {
        beltSpeed = - beltSpeed;
        _surfaceEffector = GetComponent<SurfaceEffector2D>();
    }

    void Update()
    {
        beltMaterial.mainTextureOffset = new(beltMaterial.mainTextureOffset.x + Time.deltaTime * beltSpeed, beltMaterial.mainTextureOffset.y);

        foreach (Transform wheelTransform in wheels)
            wheelTransform.Rotate(wheelTransform.forward, - wheelSpeed * Time.deltaTime);
    }

    public void Flip()
    {
        _surfaceEffector.speed = - _surfaceEffector.speed;
    }

}
