using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float beltSpeed = 1;
    public Material beltMaterial;

    void Start()
    {
        beltSpeed = - beltSpeed;
    }

    void Update()
    {
        beltMaterial.mainTextureOffset = new(beltMaterial.mainTextureOffset.x + Time.deltaTime * beltSpeed, beltMaterial.mainTextureOffset.y);
    }
}
