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
        if (_rb.velocity.sqrMagnitude > 1600)
            _rb.velocity = SetVector3Length(_rb.velocity, 40);
    }

    Vector3 SetVector3Length(Vector3 vector, float length)
    {
        return vector.normalized * length;
    }
}
