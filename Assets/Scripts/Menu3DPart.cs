using UnityEngine;

public class Menu3DPart : MonoBehaviour
{
    public Part partPrefab;

    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 12, Space.World);
    }
}
