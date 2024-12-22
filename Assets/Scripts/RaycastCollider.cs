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
        if (!IsDragged())
            return;

        _overlappingObject = other.gameObject;
        _isOverlapping = true;
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
        _pushDirection = _parent.transform.position - _overlappingObject.transform.position;
    }

    void ProcessPull()
    {
        if (_isOverlapping && !IsDragged() && IsThisDraggedObject())
            _rbTransform.Translate(Time.fixedDeltaTime * 10 * _pushDirection.normalized, Space.World);
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
        return Dragging && IsThisDraggedObject();
    }

    bool IsThisDraggedObject()
    {
        return DraggedObject == _parent;
    }
}
