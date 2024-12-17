using UnityEngine;
// TODO: Max speed (cca 40)

public class Ball : MonoBehaviour
{
    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

}
