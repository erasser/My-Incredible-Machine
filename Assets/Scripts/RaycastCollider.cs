using UnityEngine;
using static GameController;

// TODO: Zkusit overlap s více než 1 objektem současně

public class RaycastCollider : MonoBehaviour
{
    bool _isOverlapping;
    Vector2 _pushDirection;
    GameObject _overlappingObject;
    Transform _rbTransform;
    Outline _outline;
    GameObject _parent;

    void Start()
    {
        _parent = transform.parent.gameObject;
        _rbTransform = _parent.GetComponent<Rigidbody2D>().transform;

        PrepareOutline();
    }

    void FixedUpdate()
    {
        ProcessPull();
    }

    void OnTriggerEnter(Collider other)
    {
        _overlappingObject = other.gameObject;
        _isOverlapping = true;

        if (IsDragged())
            _outline.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        _isOverlapping = false;

        _outline.enabled = false;
    }

    public void CheckOverlap()
    {
        if (!_isOverlapping)
            return;

        // Start pushing
        var dir = transform.position - _overlappingObject.transform.position;
        _pushDirection = new(dir.x, dir.y);
    }

    void ProcessPull()
    {
        if (!_isOverlapping || IsDragged())
            return;

        _rbTransform.Translate(Time.fixedDeltaTime * 30 * _pushDirection.normalized);
    }

    void PrepareOutline()
    {
        _outline = _parent.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.red;
        _outline.OutlineWidth = 3;
        _outline.enabled = false;
    }

    bool IsDragged()
    {
        return Dragging && DraggedObject == _parent;
    }
}