using UnityEngine;
using static GameController;

public class RaycastCollider : MonoBehaviour
{
    bool _isOverlapping;
    GameObject _overlappingObject;
    Part _part;

    void Start()
    {
        _part = transform.parent.GetComponent<Part>();
    }

    void OnTriggerEnter(Collider other)
    {
        _part.ToggleCollisionEffect(true);
    }

    void OnTriggerExit(Collider other)
    {
        _part.ToggleCollisionEffect(false);

        if (IsSelected())
            _part.ToggleSelectionEffect(true);
    }

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
