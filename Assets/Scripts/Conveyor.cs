using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float beltSpeed = 1;
    public Material beltMaterial;
    SurfaceEffector2D _surfaceEffector;

    void Start()
    {
        beltSpeed = - beltSpeed;
        _surfaceEffector = GetComponent<SurfaceEffector2D>();
    }

    void Update()
    {
        beltMaterial.mainTextureOffset = new(beltMaterial.mainTextureOffset.x + Time.deltaTime * beltSpeed, beltMaterial.mainTextureOffset.y);
    }

    public void Flip()
    {
        _surfaceEffector.speed = - _surfaceEffector.speed;
    }

}
