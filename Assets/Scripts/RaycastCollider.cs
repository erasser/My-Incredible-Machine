using UnityEngine;
using static GameController;

// TODO: Zkusit overlap s více než 1 objektem současně

public class RaycastCollider : MonoBehaviour
{
    bool _isOverlapping;
    Vector2 _pushDirection;
    GameObject _overlappingObject;
    // Transform _rbTransform;
    Part _part;

    void Start()
    {
        _part = transform.parent.GetComponent<Part>();
        // _rbTransform = _part.GetComponent<Rigidbody2D>().transform;
    }

    void FixedUpdate()
    {
        // ProcessPush();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsDragged())
            return;

        _overlappingObject = other.gameObject;
        _isOverlapping = true;
        _part.ToggleCollisionEffect(true);
    }

    void OnTriggerExit(Collider other)
    {
        _isOverlapping = false;

        if (IsSelected())
        {
            _part.ToggleCollisionEffect(false);
            _part.ToggleSelectionEffect(true);
        }
    }

    public void CheckOverlap()
    {
        if (!_isOverlapping)
            return;

        // Start pushing
        _pushDirection = _part.transform.position - _overlappingObject.transform.position;

        // _pushDirection = Mathf.Abs(_pushDirection.x) < Mathf.Abs(_pushDirection.y) ? _pushDirection.x * Vector2.right : _pushDirection.y * Vector2.up;
    }

    // void ProcessPush()
    // {
    //     if (_isOverlapping && !IsDragged() && IsThisDraggedObject())
    //         _rbTransform.Translate(Time.fixedDeltaTime * 10 * _pushDirection.normalized, Space.World);
    // }

    bool IsDragged()
    {
        return Dragging && IsThisDraggedObject();
    }

    bool IsThisDraggedObject()
    {
        return DraggedObject == _part;
    }

    bool IsSelected()
    {
        return SelectedPart == _part;
    }
}
