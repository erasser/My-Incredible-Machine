using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_rb.IsSleeping())
            _rb.WakeUp();
    }
}
