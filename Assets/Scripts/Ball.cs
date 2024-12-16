using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_rb.IsSleeping())
            _rb.WakeUp();
    }
}
