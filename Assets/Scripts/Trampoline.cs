using UnityEngine;
// TODO: Use event to get rid of GetComponent<RB>

public class Trampoline : MonoBehaviour
{
    public float speedMultiplier = 1.4f;
    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _rb.velocity = Vector2.zero;
        var rb = col.GetComponent<Rigidbody2D>();
        var tr = transform;
        var dot = Vector2.Dot(rb.velocity.normalized, tr.up);

        if (dot < 0)
            rb.velocity = Vector2.Reflect(rb.velocity * speedMultiplier, tr.up);
    }
}
