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

    // TODO: Aplikovat pouze na míče/pohybující se věci
    void OnTriggerEnter2D(Collider2D col)
    {
        _rb.velocity = Vector2.zero;
        var rb = col.GetComponent<Rigidbody2D>();
        var tr = transform;
        var dot = Vector2.Dot(rb.velocity.normalized, tr.up);

        var spd = dot < 0 ? speedMultiplier : .5f;

        rb.velocity = Vector2.Reflect(rb.velocity * spd, tr.up);
    }
}
